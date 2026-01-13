#nullable enable
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Automatically creates the entire map system at runtime.
/// Attach this to any GameObject in your scene (e.g., GameManager or an empty "MapSystem" object).
/// </summary>
public class MapSystemInitializer : MonoBehaviour
{
    [Header("Minimap Settings")]
    [SerializeField] private Vector2 _minimapSize = new Vector2(200, 200);
    [SerializeField] private Vector2 _minimapPosition = new Vector2(-20, -20); // From top-right
    [SerializeField] private float _minimapCameraHeight = 50f;
    [SerializeField] private int _minimapRenderTextureSize = 512;
    [SerializeField] private LayerMask _minimapCullingMask = ~0; // Everything by default

    [Header("Full Map Settings")]
    [SerializeField] private int _fullMapRenderTextureSize = 1024;

    [Header("References (Auto-populated if empty)")]
    [SerializeField] private Transform? _playerTransform;

    private Camera? _minimapCamera;
    private Camera? _fullMapCamera;
    private RenderTexture? _minimapRenderTexture;
    private RenderTexture? _fullMapRenderTexture;
    private Canvas? _minimapCanvas;
    private Canvas? _fullMapCanvas;
    private GameObject? _mapControllerObj;
    private GameObject? _fullMapUIObj;

    private void Awake()
    {
        // Find player if not assigned
        if (_playerTransform == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                _playerTransform = player.transform;
            }
        }

        CreateMapSystem();
    }

    private void CreateMapSystem()
    {
        // Create render textures
        _minimapRenderTexture = new RenderTexture(_minimapRenderTextureSize, _minimapRenderTextureSize, 16);
        _minimapRenderTexture.name = "MinimapRenderTexture";

        _fullMapRenderTexture = new RenderTexture(_fullMapRenderTextureSize, _fullMapRenderTextureSize, 16);
        _fullMapRenderTexture.name = "FullMapRenderTexture";

        // Create minimap camera
        CreateMinimapCamera();

        // Create full map camera
        CreateFullMapCamera();

        // Create minimap UI
        CreateMinimapUI();

        // Create full map UI
        CreateFullMapUI();

        // Create and configure MapController
        CreateMapController();
    }

    private void CreateMinimapCamera()
    {
        var camObj = new GameObject("MinimapCamera");
        camObj.transform.SetParent(transform);

        _minimapCamera = camObj.AddComponent<Camera>();
        _minimapCamera.orthographic = true;
        _minimapCamera.orthographicSize = 25f;
        _minimapCamera.cullingMask = _minimapCullingMask;
        _minimapCamera.targetTexture = _minimapRenderTexture;
        _minimapCamera.clearFlags = CameraClearFlags.SolidColor;
        _minimapCamera.backgroundColor = new Color(0.1f, 0.1f, 0.15f, 1f);
        _minimapCamera.depth = -10; // Render before main camera

        // Position above player looking down
        if (_playerTransform != null)
        {
            camObj.transform.position = _playerTransform.position + Vector3.up * _minimapCameraHeight;
        }
        else
        {
            camObj.transform.position = Vector3.up * _minimapCameraHeight;
        }
        camObj.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        // Add follow script
        var follower = camObj.AddComponent<MinimapCameraFollower>();
        follower.Initialize(_playerTransform, _minimapCameraHeight);
    }

    private void CreateFullMapCamera()
    {
        var camObj = new GameObject("FullMapCamera");
        camObj.transform.SetParent(transform);

        _fullMapCamera = camObj.AddComponent<Camera>();
        _fullMapCamera.orthographic = true;
        _fullMapCamera.orthographicSize = 50f;
        _fullMapCamera.cullingMask = _minimapCullingMask;
        _fullMapCamera.targetTexture = _fullMapRenderTexture;
        _fullMapCamera.clearFlags = CameraClearFlags.SolidColor;
        _fullMapCamera.backgroundColor = new Color(0.1f, 0.1f, 0.15f, 1f);
        _fullMapCamera.depth = -10;
        _fullMapCamera.enabled = false; // Only enable when full map is open

        // Position above center of map
        if (_playerTransform != null)
        {
            camObj.transform.position = _playerTransform.position + Vector3.up * _minimapCameraHeight;
        }
        else
        {
            camObj.transform.position = Vector3.up * _minimapCameraHeight;
        }
        camObj.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

    private void CreateMinimapUI()
    {
        // Create canvas for minimap
        var canvasObj = new GameObject("MinimapCanvas");
        canvasObj.transform.SetParent(transform);

        _minimapCanvas = canvasObj.AddComponent<Canvas>();
        _minimapCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        _minimapCanvas.sortingOrder = 10;

        var scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        canvasObj.AddComponent<GraphicRaycaster>();

        // Create minimap container (top-right corner)
        var containerObj = new GameObject("MinimapContainer");
        containerObj.transform.SetParent(canvasObj.transform, false);

        var containerRect = containerObj.AddComponent<RectTransform>();
        containerRect.anchorMin = new Vector2(1, 1);
        containerRect.anchorMax = new Vector2(1, 1);
        containerRect.pivot = new Vector2(1, 1);
        containerRect.anchoredPosition = _minimapPosition;
        containerRect.sizeDelta = _minimapSize;

        // Create background
        var bgObj = new GameObject("Background");
        bgObj.transform.SetParent(containerObj.transform, false);

        var bgRect = bgObj.AddComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.sizeDelta = new Vector2(10, 10); // Padding
        bgRect.anchoredPosition = Vector2.zero;

        var bgImage = bgObj.AddComponent<Image>();
        bgImage.color = new Color(0, 0, 0, 0.7f);

        // Create minimap display
        var displayObj = new GameObject("MinimapDisplay");
        displayObj.transform.SetParent(containerObj.transform, false);

        var displayRect = displayObj.AddComponent<RectTransform>();
        displayRect.anchorMin = Vector2.zero;
        displayRect.anchorMax = Vector2.one;
        displayRect.sizeDelta = Vector2.zero;
        displayRect.anchoredPosition = Vector2.zero;

        var rawImage = displayObj.AddComponent<RawImage>();
        rawImage.texture = _minimapRenderTexture;

        // Create player indicator (center dot)
        var indicatorObj = new GameObject("PlayerIndicator");
        indicatorObj.transform.SetParent(containerObj.transform, false);

        var indicatorRect = indicatorObj.AddComponent<RectTransform>();
        indicatorRect.anchorMin = new Vector2(0.5f, 0.5f);
        indicatorRect.anchorMax = new Vector2(0.5f, 0.5f);
        indicatorRect.sizeDelta = new Vector2(10, 10);
        indicatorRect.anchoredPosition = Vector2.zero;

        var indicatorImage = indicatorObj.AddComponent<Image>();
        indicatorImage.color = Color.green;

        // Create border
        var borderObj = new GameObject("Border");
        borderObj.transform.SetParent(containerObj.transform, false);

        var borderRect = borderObj.AddComponent<RectTransform>();
        borderRect.anchorMin = Vector2.zero;
        borderRect.anchorMax = Vector2.one;
        borderRect.sizeDelta = Vector2.zero;
        borderRect.anchoredPosition = Vector2.zero;

        var borderImage = borderObj.AddComponent<Image>();
        borderImage.color = new Color(0.3f, 0.3f, 0.3f, 1f);
        borderImage.raycastTarget = false;
        // Make it just an outline by using a sprite with only borders, or we can skip this
        borderImage.fillCenter = false;
    }

    private void CreateFullMapUI()
    {
        // Create canvas for full map
        var canvasObj = new GameObject("FullMapCanvas");
        canvasObj.transform.SetParent(transform);

        _fullMapCanvas = canvasObj.AddComponent<Canvas>();
        _fullMapCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        _fullMapCanvas.sortingOrder = 100; // Above everything

        var scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        canvasObj.AddComponent<GraphicRaycaster>();

        // Create FullMapUI component
        _fullMapUIObj = canvasObj;
        var fullMapUI = canvasObj.AddComponent<FullMapUI>();

        // Create full screen background
        var bgObj = new GameObject("Background");
        bgObj.transform.SetParent(canvasObj.transform, false);

        var bgRect = bgObj.AddComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.sizeDelta = Vector2.zero;
        bgRect.anchoredPosition = Vector2.zero;

        var bgImage = bgObj.AddComponent<Image>();
        bgImage.color = new Color(0, 0, 0, 0.9f);

        // Create map container (centered, with margins)
        var containerObj = new GameObject("MapContainer");
        containerObj.transform.SetParent(canvasObj.transform, false);

        var containerRect = containerObj.AddComponent<RectTransform>();
        containerRect.anchorMin = new Vector2(0.05f, 0.1f);
        containerRect.anchorMax = new Vector2(0.95f, 0.9f);
        containerRect.sizeDelta = Vector2.zero;
        containerRect.anchoredPosition = Vector2.zero;

        // Create map display
        var displayObj = new GameObject("MapDisplay");
        displayObj.transform.SetParent(containerObj.transform, false);

        var displayRect = displayObj.AddComponent<RectTransform>();
        displayRect.anchorMin = Vector2.zero;
        displayRect.anchorMax = Vector2.one;
        displayRect.sizeDelta = Vector2.zero;
        displayRect.anchoredPosition = Vector2.zero;

        var rawImage = displayObj.AddComponent<RawImage>();
        rawImage.texture = _fullMapRenderTexture;

        // Create title
        var titleObj = new GameObject("Title");
        titleObj.transform.SetParent(canvasObj.transform, false);

        var titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 1f);
        titleRect.anchorMax = new Vector2(0.5f, 1f);
        titleRect.pivot = new Vector2(0.5f, 1f);
        titleRect.anchoredPosition = new Vector2(0, -20);
        titleRect.sizeDelta = new Vector2(300, 50);

        var titleText = titleObj.AddComponent<TextMeshProUGUI>();
        titleText.text = "MAP";
        titleText.fontSize = 36;
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.color = Color.white;

        // Create close button
        var closeObj = new GameObject("CloseButton");
        closeObj.transform.SetParent(canvasObj.transform, false);

        var closeRect = closeObj.AddComponent<RectTransform>();
        closeRect.anchorMin = new Vector2(1, 1);
        closeRect.anchorMax = new Vector2(1, 1);
        closeRect.pivot = new Vector2(1, 1);
        closeRect.anchoredPosition = new Vector2(-20, -20);
        closeRect.sizeDelta = new Vector2(40, 40);

        var closeImage = closeObj.AddComponent<Image>();
        closeImage.color = new Color(0.8f, 0.2f, 0.2f, 1f);

        var closeButton = closeObj.AddComponent<Button>();
        closeButton.targetGraphic = closeImage;

        var closeTextObj = new GameObject("Text");
        closeTextObj.transform.SetParent(closeObj.transform, false);

        var closeTextRect = closeTextObj.AddComponent<RectTransform>();
        closeTextRect.anchorMin = Vector2.zero;
        closeTextRect.anchorMax = Vector2.one;
        closeTextRect.sizeDelta = Vector2.zero;
        closeTextRect.anchoredPosition = Vector2.zero;

        var closeText = closeTextObj.AddComponent<TextMeshProUGUI>();
        closeText.text = "X";
        closeText.fontSize = 24;
        closeText.alignment = TextAlignmentOptions.Center;
        closeText.color = Color.white;

        // Create instructions text
        var instructionsObj = new GameObject("Instructions");
        instructionsObj.transform.SetParent(canvasObj.transform, false);

        var instructionsRect = instructionsObj.AddComponent<RectTransform>();
        instructionsRect.anchorMin = new Vector2(0.5f, 0);
        instructionsRect.anchorMax = new Vector2(0.5f, 0);
        instructionsRect.pivot = new Vector2(0.5f, 0);
        instructionsRect.anchoredPosition = new Vector2(0, 20);
        instructionsRect.sizeDelta = new Vector2(600, 30);

        var instructionsText = instructionsObj.AddComponent<TextMeshProUGUI>();
        instructionsText.text = "WASD/Arrows: Pan | Scroll/+/-: Zoom | M/Esc: Close";
        instructionsText.fontSize = 16;
        instructionsText.alignment = TextAlignmentOptions.Center;
        instructionsText.color = new Color(0.7f, 0.7f, 0.7f, 1f);

        // Create zoom text
        var zoomTextObj = new GameObject("ZoomText");
        zoomTextObj.transform.SetParent(canvasObj.transform, false);

        var zoomTextRect = zoomTextObj.AddComponent<RectTransform>();
        zoomTextRect.anchorMin = new Vector2(0, 0);
        zoomTextRect.anchorMax = new Vector2(0, 0);
        zoomTextRect.pivot = new Vector2(0, 0);
        zoomTextRect.anchoredPosition = new Vector2(20, 20);
        zoomTextRect.sizeDelta = new Vector2(150, 30);

        var zoomText = zoomTextObj.AddComponent<TextMeshProUGUI>();
        zoomText.text = "Zoom: 100%";
        zoomText.fontSize = 16;
        zoomText.alignment = TextAlignmentOptions.Left;
        zoomText.color = Color.white;

        // Initialize FullMapUI with references
        fullMapUI.InitializeFromCode(rawImage, _fullMapRenderTexture, _fullMapCamera!, containerRect, zoomText, closeButton);

        // Start hidden
        canvasObj.SetActive(false);
    }

    private void CreateMapController()
    {
        _mapControllerObj = new GameObject("MapController");
        _mapControllerObj.transform.SetParent(transform);

        var mapController = _mapControllerObj.AddComponent<MapController>();
        mapController.InitializeFromCode(_minimapCamera!, _fullMapUIObj!.GetComponent<FullMapUI>());
    }

    private void OnDestroy()
    {
        // Clean up render textures
        if (_minimapRenderTexture != null)
        {
            _minimapRenderTexture.Release();
            Destroy(_minimapRenderTexture);
        }

        if (_fullMapRenderTexture != null)
        {
            _fullMapRenderTexture.Release();
            Destroy(_fullMapRenderTexture);
        }
    }
}

/// <summary>
/// Simple script to make the minimap camera follow the player.
/// </summary>
public class MinimapCameraFollower : MonoBehaviour
{
    private Transform? _target;
    private float _height;

    public void Initialize(Transform? target, float height)
    {
        _target = target;
        _height = height;
    }

    private void LateUpdate()
    {
        if (_target == null) return;

        Vector3 newPos = _target.position;
        newPos.y = _height;
        transform.position = newPos;

        // Rotate to match player's Y rotation (optional - makes map rotate with player)
        // transform.rotation = Quaternion.Euler(90f, _target.eulerAngles.y, 0f);
    }
}
