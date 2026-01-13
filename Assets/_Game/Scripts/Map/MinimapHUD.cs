using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// HUD element for displaying the minimap in the corner of the screen.
/// Uses a RawImage to display the minimap render texture.
/// </summary>
public class MinimapHUD : MonoBehaviour
{
    [Header("Display Settings")]
    [SerializeField] private RawImage _minimapImage;
    [SerializeField] private RenderTexture _minimapRenderTexture;
    [SerializeField] private Image _borderImage;
    [SerializeField] private Image _compassNorthIndicator;

    [Header("Transform Reference")]
    [SerializeField] private Transform _playerTransform;

    [Header("Visibility")]
    [SerializeField] private bool _showOnStart = true;

    private bool _isVisible;
    private CanvasGroup _canvasGroup;

    public bool IsVisible => _isVisible;

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
        if (_minimapImage != null && _minimapRenderTexture != null)
        {
            _minimapImage.texture = _minimapRenderTexture;
        }

        if (_playerTransform == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                _playerTransform = player.transform;
            }
        }

        if (_showOnStart)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Update()
    {
        if (!_isVisible || _playerTransform == null) return;

        UpdateCompass();
    }

    private void UpdateCompass()
    {
        if (_compassNorthIndicator == null) return;

        float playerYRotation = _playerTransform.eulerAngles.y;
        _compassNorthIndicator.rectTransform.localRotation = Quaternion.Euler(0, 0, playerYRotation);
    }

    public void Show()
    {
        _isVisible = true;
        if (_canvasGroup != null)
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        _isVisible = false;
        if (_canvasGroup != null)
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }

    public void Toggle()
    {
        if (_isVisible)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    public void SetRenderTexture(RenderTexture renderTexture)
    {
        _minimapRenderTexture = renderTexture;
        if (_minimapImage != null)
        {
            _minimapImage.texture = _minimapRenderTexture;
        }
    }
}
