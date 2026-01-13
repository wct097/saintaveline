using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Full screen map overlay with pan and zoom controls.
/// Shows labeled points from the MapLabeling system.
/// </summary>
public class FullMapUI : MonoBehaviour
{
    [Header("Map Display")]
    [SerializeField] private RawImage _mapImage;
    [SerializeField] private RenderTexture _mapRenderTexture;
    [SerializeField] private Camera _mapCamera;
    [SerializeField] private RectTransform _mapContainer;

    [Header("Zoom Settings")]
    [SerializeField] private float _minZoom = 20f;
    [SerializeField] private float _maxZoom = 100f;
    [SerializeField] private float _zoomSpeed = 10f;
    [SerializeField] private float _defaultZoom = 50f;

    [Header("Pan Settings")]
    [SerializeField] private float _panSpeed = 50f;
    [SerializeField] private float _mousePanSpeed = 1f;

    [Header("UI Elements")]
    [SerializeField] private GameObject _labeledPointPrefab;
    [SerializeField] private RectTransform _labeledPointsContainer;
    [SerializeField] private GameObject _legendPanel;
    [SerializeField] private TextMeshProUGUI _zoomLevelText;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _zoomInButton;
    [SerializeField] private Button _zoomOutButton;
    [SerializeField] private Button _centerOnPlayerButton;

    private float _currentZoom;
    private Vector3 _originalCameraPosition;
    private Vector3 _panOffset;
    private bool _isDragging;
    private Vector3 _lastMousePosition;
    private Transform _playerTransform;
    private Dictionary<string, GameObject> _labelMarkers = new Dictionary<string, GameObject>();
    private PlayerStats _playerStats;

    private CanvasGroup _canvasGroup;
    private bool _initializedFromCode;
    public bool IsActive { get; private set; }

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
        {
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    private void Start()
    {
        // Skip if already initialized from code
        if (_initializedFromCode) return;

        if (_mapImage != null && _mapRenderTexture != null)
        {
            _mapImage.texture = _mapRenderTexture;
        }

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _playerTransform = player.transform;
            _playerStats = player.GetComponent<PlayerStats>();
        }

        if (_mapCamera != null)
        {
            _originalCameraPosition = _mapCamera.transform.position;
        }

        _currentZoom = _defaultZoom;

        SetupButtons();
        Hide();
    }

    private void SetupButtons()
    {
        if (_closeButton != null)
        {
            _closeButton.onClick.AddListener(OnCloseClicked);
        }

        if (_zoomInButton != null)
        {
            _zoomInButton.onClick.AddListener(ZoomIn);
        }

        if (_zoomOutButton != null)
        {
            _zoomOutButton.onClick.AddListener(ZoomOut);
        }

        if (_centerOnPlayerButton != null)
        {
            _centerOnPlayerButton.onClick.AddListener(CenterOnPlayer);
        }
    }

    private void Update()
    {
        if (!IsActive) return;

        HandleZoomInput();
        HandlePanInput();
        HandleMousePan();
        UpdateZoomText();

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.M))
        {
            if (MapController.Instance != null)
            {
                MapController.Instance.CloseFullMap();
            }
            else
            {
                Hide();
            }
        }
    }

    private void HandleZoomInput()
    {
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollDelta) > 0.01f)
        {
            _currentZoom -= scrollDelta * _zoomSpeed;
            _currentZoom = Mathf.Clamp(_currentZoom, _minZoom, _maxZoom);
            ApplyZoom();
        }

        if (Input.GetKey(KeyCode.Equals) || Input.GetKey(KeyCode.KeypadPlus))
        {
            _currentZoom -= _zoomSpeed * Time.unscaledDeltaTime;
            _currentZoom = Mathf.Clamp(_currentZoom, _minZoom, _maxZoom);
            ApplyZoom();
        }
        else if (Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus))
        {
            _currentZoom += _zoomSpeed * Time.unscaledDeltaTime;
            _currentZoom = Mathf.Clamp(_currentZoom, _minZoom, _maxZoom);
            ApplyZoom();
        }
    }

    private void HandlePanInput()
    {
        Vector3 panDelta = Vector3.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            panDelta.z += _panSpeed * Time.unscaledDeltaTime;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            panDelta.z -= _panSpeed * Time.unscaledDeltaTime;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            panDelta.x -= _panSpeed * Time.unscaledDeltaTime;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            panDelta.x += _panSpeed * Time.unscaledDeltaTime;
        }

        if (panDelta != Vector3.zero)
        {
            _panOffset += panDelta;
            ApplyPan();
        }
    }

    private void HandleMousePan()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isDragging = true;
            _lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
        }

        if (_isDragging)
        {
            Vector3 delta = Input.mousePosition - _lastMousePosition;
            _panOffset.x -= delta.x * _mousePanSpeed;
            _panOffset.z -= delta.y * _mousePanSpeed;
            ApplyPan();
            _lastMousePosition = Input.mousePosition;
        }
    }

    private void ApplyZoom()
    {
        if (_mapCamera != null)
        {
            _mapCamera.orthographicSize = _currentZoom;
        }
    }

    private void ApplyPan()
    {
        if (_mapCamera != null)
        {
            Vector3 newPosition = _originalCameraPosition + _panOffset;
            newPosition.y = _mapCamera.transform.position.y;
            _mapCamera.transform.position = newPosition;
        }
    }

    private void UpdateZoomText()
    {
        if (_zoomLevelText != null)
        {
            float zoomPercent = 100f * (_maxZoom - _currentZoom) / (_maxZoom - _minZoom);
            _zoomLevelText.text = $"Zoom: {zoomPercent:F0}%";
        }
    }

    public void Show()
    {
        IsActive = true;
        gameObject.SetActive(true);

        if (_mapCamera != null)
        {
            _mapCamera.enabled = true;
        }

        if (_canvasGroup != null)
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        CenterOnPlayer();
        RefreshLabeledPoints();
    }

    public void Hide()
    {
        IsActive = false;

        if (_mapCamera != null)
        {
            _mapCamera.enabled = false;
        }

        if (_canvasGroup != null)
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        gameObject.SetActive(false);
    }

    public void ZoomIn()
    {
        _currentZoom = Mathf.Max(_minZoom, _currentZoom - _zoomSpeed);
        ApplyZoom();
    }

    public void ZoomOut()
    {
        _currentZoom = Mathf.Min(_maxZoom, _currentZoom + _zoomSpeed);
        ApplyZoom();
    }

    public void CenterOnPlayer()
    {
        if (_playerTransform == null || _mapCamera == null) return;

        _panOffset = Vector3.zero;
        Vector3 playerPos = _playerTransform.position;
        _originalCameraPosition = new Vector3(playerPos.x, _mapCamera.transform.position.y, playerPos.z);
        ApplyPan();
    }

    private void OnCloseClicked()
    {
        if (MapController.Instance != null)
        {
            MapController.Instance.CloseFullMap();
        }
        else
        {
            Hide();
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void RefreshLabeledPoints()
    {
        ClearLabelMarkers();

        if (_playerStats == null || _labeledPointsContainer == null) return;

        foreach (var kvp in _playerStats.LabeledPoints)
        {
            CreateLabelMarker(kvp.Key, kvp.Value);
        }
    }

    private void CreateLabelMarker(string labelName, Vector3 worldPosition)
    {
        if (_labeledPointPrefab == null || _labeledPointsContainer == null) return;

        GameObject marker = Instantiate(_labeledPointPrefab, _labeledPointsContainer);
        marker.name = $"Label_{labelName}";

        var textComponent = marker.GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = labelName;
        }

        _labelMarkers[labelName] = marker;
    }

    private void ClearLabelMarkers()
    {
        foreach (var marker in _labelMarkers.Values)
        {
            if (marker != null)
            {
                Destroy(marker);
            }
        }
        _labelMarkers.Clear();
    }

    public void ToggleLegend()
    {
        if (_legendPanel != null)
        {
            _legendPanel.SetActive(!_legendPanel.activeSelf);
        }
    }

    private void OnDestroy()
    {
        ClearLabelMarkers();
    }

    /// <summary>
    /// Initialize FullMapUI from code (used by MapSystemInitializer).
    /// </summary>
    public void InitializeFromCode(RawImage mapImage, RenderTexture renderTexture, Camera mapCamera,
        RectTransform mapContainer, TextMeshProUGUI zoomText, Button closeButton)
    {
        _initializedFromCode = true;

        _mapImage = mapImage;
        _mapRenderTexture = renderTexture;
        _mapCamera = mapCamera;
        _mapContainer = mapContainer;
        _zoomLevelText = zoomText;
        _closeButton = closeButton;

        // Find player
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _playerTransform = player.transform;
            _playerStats = player.GetComponent<PlayerStats>();
        }

        if (_mapImage != null && _mapRenderTexture != null)
        {
            _mapImage.texture = _mapRenderTexture;
        }

        if (_mapCamera != null)
        {
            _originalCameraPosition = _mapCamera.transform.position;
        }

        _currentZoom = _defaultZoom;

        if (_closeButton != null)
        {
            _closeButton.onClick.AddListener(OnCloseClicked);
        }
    }
}
