using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Singleton controller for managing minimap and full map states.
/// Handles zoom controls and coordinates between minimap and full map views.
/// </summary>
public class MapController : MonoBehaviour
{
    public static MapController Instance { get; private set; }

    [Header("Minimap Settings")]
    [SerializeField] private Camera _minimapCamera;
    [SerializeField] private RawImage _minimapDisplay;
    [SerializeField] private float _minimapMinZoom = 10f;
    [SerializeField] private float _minimapMaxZoom = 50f;
    [SerializeField] private float _minimapZoomSpeed = 5f;
    [SerializeField] private float _defaultMinimapZoom = 25f;

    [Header("Full Map Settings")]
    [SerializeField] private FullMapUI _fullMapUI;

    private float _currentMinimapZoom;
    private bool _isFullMapOpen;
    private bool _initializedFromCode;

    public Camera MinimapCamera => _minimapCamera;
    public bool IsFullMapOpen => _isFullMapOpen;
    public float CurrentMinimapZoom => _currentMinimapZoom;
    public bool IsFullMapReady => _fullMapUI != null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        // Skip if already initialized from code
        if (_initializedFromCode) return;

        // Try to find minimap camera if not assigned
        if (_minimapCamera == null)
        {
            // Look for a camera named "MinimapCamera" or with Minimap in the name
            foreach (var cam in FindObjectsByType<Camera>(FindObjectsSortMode.None))
            {
                if (cam.gameObject.name.Contains("Minimap") || cam.gameObject.name.Contains("minimap"))
                {
                    _minimapCamera = cam;
                    break;
                }
            }
        }

        // Try to find FullMapUI if not assigned
        if (_fullMapUI == null)
        {
            _fullMapUI = FindFirstObjectByType<FullMapUI>();
            if (_fullMapUI == null)
            {
                Debug.LogWarning("MapController: FullMapUI not found. Full map feature will be disabled.");
            }
        }

        _currentMinimapZoom = _defaultMinimapZoom;
        if (_minimapCamera != null)
        {
            _minimapCamera.orthographicSize = _currentMinimapZoom;
        }
    }

    private void Update()
    {
        if (_isFullMapOpen) return;

        HandleMinimapZoom();
    }

    private void HandleMinimapZoom()
    {
        if (_minimapCamera == null) return;

        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollDelta) > 0.01f)
        {
            _currentMinimapZoom -= scrollDelta * _minimapZoomSpeed;
            _currentMinimapZoom = Mathf.Clamp(_currentMinimapZoom, _minimapMinZoom, _minimapMaxZoom);
            _minimapCamera.orthographicSize = _currentMinimapZoom;
        }

        if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            ZoomIn();
        }
        else if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            ZoomOut();
        }
    }

    public void ZoomIn()
    {
        if (_minimapCamera == null) return;
        _currentMinimapZoom = Mathf.Max(_minimapMinZoom, _currentMinimapZoom - _minimapZoomSpeed);
        _minimapCamera.orthographicSize = _currentMinimapZoom;
    }

    public void ZoomOut()
    {
        if (_minimapCamera == null) return;
        _currentMinimapZoom = Mathf.Min(_minimapMaxZoom, _currentMinimapZoom + _minimapZoomSpeed);
        _minimapCamera.orthographicSize = _currentMinimapZoom;
    }

    public void OpenFullMap()
    {
        if (_fullMapUI == null) return;

        _isFullMapOpen = true;
        _fullMapUI.Show();
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseFullMap()
    {
        if (_fullMapUI == null) return;

        _isFullMapOpen = false;
        _fullMapUI.Hide();
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Ensure InputManager state is reset
        if (InputManager.Instance != null && InputManager.Instance.CurrentState == InputState.FullMap)
        {
            InputManager.Instance.SetInputState(InputState.Gameplay);
        }
    }

    public void ToggleFullMap()
    {
        if (_isFullMapOpen)
        {
            CloseFullMap();
        }
        else
        {
            OpenFullMap();
        }
    }

    public void SetMinimapZoom(float zoom)
    {
        _currentMinimapZoom = Mathf.Clamp(zoom, _minimapMinZoom, _minimapMaxZoom);
        if (_minimapCamera != null)
        {
            _minimapCamera.orthographicSize = _currentMinimapZoom;
        }
    }

    /// <summary>
    /// Initialize MapController from code (used by MapSystemInitializer).
    /// </summary>
    public void InitializeFromCode(Camera minimapCamera, FullMapUI fullMapUI)
    {
        _initializedFromCode = true;
        _minimapCamera = minimapCamera;
        _fullMapUI = fullMapUI;
        _currentMinimapZoom = _defaultMinimapZoom;
        if (_minimapCamera != null)
        {
            _minimapCamera.orthographicSize = _currentMinimapZoom;
        }
    }
}
