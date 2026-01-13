using UnityEngine;

/// <summary>
/// Controls footstep sounds with surface detection and AudioManager integration.
/// Uses raycast to detect surface type and varies pitch/clip selection.
/// Can also emit sound stimuli for AI detection.
/// </summary>
public class FootstepController : MonoBehaviour
{
    [Header("Footstep Clips")]
    [SerializeField] private AudioClip[] _concreteSteps;
    [SerializeField] private AudioClip[] _woodSteps;
    [SerializeField] private AudioClip[] _dirtSteps;
    [SerializeField] private AudioClip[] _metalSteps;
    [SerializeField] private AudioClip[] _carpetSteps;

    [Header("Step Settings")]
    [SerializeField] private float _stepInterval = 0.5f;
    [SerializeField] private float _runningStepInterval = 0.35f;
    [SerializeField] private float _crouchingStepInterval = 0.7f;
    [SerializeField] [Range(0f, 1f)] private float _volume = 0.6f;
    [SerializeField] [Range(0f, 0.3f)] private float _pitchVariation = 0.1f;

    [Header("Detection")]
    [SerializeField] private float _raycastDistance = 2f;
    [SerializeField] private LayerMask _groundLayers = -1;

    [Header("AI Stimulus")]
    [SerializeField] private bool _emitStimulusOnStep = true;
    [SerializeField] private float _normalStepHearingRange = 8f;
    [SerializeField] private float _runningStepHearingRange = 15f;
    [SerializeField] private float _crouchingStepHearingRange = 3f;

    private CharacterController _controller;
    private float _stepTimer;
    private Vector3 _lastPosition;
    private bool _isEnabled = true;

    // Movement state tracking
    private bool _isRunning;
    private bool _isCrouching;

    public bool IsEnabled
    {
        get => _isEnabled;
        set => _isEnabled = value;
    }

    public bool IsRunning
    {
        get => _isRunning;
        set => _isRunning = value;
    }

    public bool IsCrouching
    {
        get => _isCrouching;
        set => _isCrouching = value;
    }

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _lastPosition = transform.position;
    }

    private void Update()
    {
        if (!_isEnabled) return;
        if (_controller == null) return;

        Vector3 move = transform.position - _lastPosition;
        move.y = 0f;

        if (move.magnitude > 0.01f && _controller.isGrounded)
        {
            _stepTimer -= Time.deltaTime;
            if (_stepTimer <= 0f)
            {
                PlayFootstep();
                _stepTimer = GetCurrentStepInterval();
            }
        }
        else
        {
            // Reset timer when stopped to play step immediately when starting
            _stepTimer = Mathf.Min(_stepTimer, 0.1f);
        }

        _lastPosition = transform.position;
    }

    private float GetCurrentStepInterval()
    {
        if (_isCrouching) return _crouchingStepInterval;
        if (_isRunning) return _runningStepInterval;
        return _stepInterval;
    }

    private void PlayFootstep()
    {
        SurfaceType surface = DetectSurface();
        AudioClip clip = GetRandomClipForSurface(surface);

        if (clip == null) return;

        float pitch = Random.Range(1f - _pitchVariation, 1f + _pitchVariation);
        float adjustedVolume = _volume;

        // Adjust volume based on movement state
        if (_isCrouching)
        {
            adjustedVolume *= 0.4f;
        }
        else if (_isRunning)
        {
            adjustedVolume *= 1.2f;
        }

        // Play through AudioManager if available, otherwise use local source
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(clip, transform.position, adjustedVolume, pitch);
        }
        else
        {
            // Fallback to AudioSource.PlayClipAtPoint
            AudioSource.PlayClipAtPoint(clip, transform.position, adjustedVolume);
        }

        // Emit stimulus for AI
        if (_emitStimulusOnStep)
        {
            EmitFootstepStimulus();
        }
    }

    private SurfaceType DetectSurface()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, _raycastDistance, _groundLayers))
        {
            // First check for SurfaceIdentifier component
            SurfaceIdentifier identifier = hit.collider.GetComponent<SurfaceIdentifier>();
            if (identifier != null)
            {
                return identifier.surfaceType;
            }

            // Fallback: Check tag for surface type
            string tag = hit.collider.tag;
            switch (tag)
            {
                case "Wood":
                    return SurfaceType.Wood;
                case "Dirt":
                case "Grass":
                    return SurfaceType.Dirt;
                case "Metal":
                    return SurfaceType.Concrete; // Metal mapped to concrete if no metal clips
                case "Carpet":
                    return SurfaceType.Wood; // Carpet mapped to wood if no carpet clips
            }
        }

        return SurfaceType.Concrete;
    }

    private AudioClip GetRandomClipForSurface(SurfaceType surface)
    {
        AudioClip[] clips = surface switch
        {
            SurfaceType.Wood => _woodSteps,
            SurfaceType.Dirt => _dirtSteps,
            _ => _concreteSteps
        };

        // Handle extended surface types if clips are available
        if (surface == SurfaceType.Concrete && _metalSteps != null && _metalSteps.Length > 0)
        {
            // Check if we're actually on metal
            Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, _raycastDistance, _groundLayers))
            {
                if (hit.collider.CompareTag("Metal"))
                {
                    clips = _metalSteps;
                }
            }
        }

        if (clips == null || clips.Length == 0)
        {
            clips = _concreteSteps;
        }

        if (clips == null || clips.Length == 0) return null;

        return clips[Random.Range(0, clips.Length)];
    }

    private void EmitFootstepStimulus()
    {
        float hearingRange = _normalStepHearingRange;

        if (_isCrouching)
        {
            hearingRange = _crouchingStepHearingRange;
        }
        else if (_isRunning)
        {
            hearingRange = _runningStepHearingRange;
        }

        SoundStimulus stimulus = new SoundStimulus
        {
            Position = transform.position,
            Kind = StimulusKind.Shout, // Using Shout as a general "noise" type
            HearingRange = hearingRange
        };

        StimulusBus.Emit(stimulus);
    }

    /// <summary>
    /// Manually trigger a footstep sound.
    /// Useful for animation events.
    /// </summary>
    public void TriggerFootstep()
    {
        PlayFootstep();
    }
}
