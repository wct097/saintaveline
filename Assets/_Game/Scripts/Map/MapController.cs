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

    public Camera MinimapCamera => _minimapCamera;
    public bool IsFullMapOpen => _isFullMapOpen;
    public float CurrentMinimapZoom => _currentMinimapZoom;

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
}
