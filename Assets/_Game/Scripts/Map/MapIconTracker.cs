using UnityEngine;

/// <summary>
/// Types of map icons for different entity types.
/// </summary>
public enum MapIconType
{
    Player,
    FamilyMember,
    Enemy,
    LabeledPoint,
    Objective
}

/// <summary>
/// Tracks an entity's position and displays an icon on the minimap.
/// Attach this component to NPCs or objects that should appear on the map.
/// </summary>
public class MapIconTracker : MonoBehaviour
{
    [Header("Icon Settings")]
    [SerializeField] private MapIconType _iconType = MapIconType.Enemy;
    [SerializeField] private GameObject _iconPrefab;
    [SerializeField] private Color _iconColor = Color.red;
    [SerializeField] private float _iconScale = 1f;
    [SerializeField] private float _iconHeightOffset = 10f;

    [Header("Visibility")]
    [SerializeField] private bool _alwaysVisible = true;
    [SerializeField] private float _visibilityRange = 50f;

    private GameObject _iconInstance;
    private SpriteRenderer _iconRenderer;
    private Transform _playerTransform;
    private BaseNPC _npc;

    public MapIconType IconType => _iconType;
    public bool IsVisible { get; private set; }

    private void Start()
    {
        _npc = GetComponent<BaseNPC>();

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _playerTransform = player.transform;
        }

        CreateIcon();
        ApplyIconColor();
    }

    private void CreateIcon()
    {
        if (_iconPrefab != null)
        {
            _iconInstance = Instantiate(_iconPrefab);
        }
        else
        {
            _iconInstance = GameObject.CreatePrimitive(PrimitiveType.Quad);
            Destroy(_iconInstance.GetComponent<Collider>());
        }

        _iconInstance.name = $"MapIcon_{gameObject.name}";
        _iconInstance.layer = LayerMask.NameToLayer("Minimap");

        _iconRenderer = _iconInstance.GetComponent<SpriteRenderer>();
        if (_iconRenderer == null)
        {
            var meshRenderer = _iconInstance.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.material = new Material(Shader.Find("Unlit/Color"));
                meshRenderer.material.color = _iconColor;
            }
        }

        _iconInstance.transform.localScale = Vector3.one * _iconScale;
        _iconInstance.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

    private void ApplyIconColor()
    {
        Color color = _iconType switch
        {
            MapIconType.Player => Color.white,
            MapIconType.FamilyMember => Color.green,
            MapIconType.Enemy => Color.red,
            MapIconType.LabeledPoint => Color.yellow,
            MapIconType.Objective => Color.cyan,
            _ => _iconColor
        };

        if (_iconRenderer != null)
        {
            _iconRenderer.color = color;
        }
        else if (_iconInstance != null)
        {
            var meshRenderer = _iconInstance.GetComponent<MeshRenderer>();
            if (meshRenderer != null && meshRenderer.material != null)
            {
                meshRenderer.material.color = color;
            }
        }
    }

    private void Update()
    {
        if (_iconInstance == null) return;

        UpdateIconPosition();
        UpdateVisibility();
        UpdateIconRotation();
    }

    private void UpdateIconPosition()
    {
        Vector3 iconPosition = transform.position;
        iconPosition.y += _iconHeightOffset;
        _iconInstance.transform.position = iconPosition;
    }

    private void UpdateIconRotation()
    {
        float yRotation = transform.eulerAngles.y;
        _iconInstance.transform.rotation = Quaternion.Euler(90f, yRotation, 0f);
    }

    private void UpdateVisibility()
    {
        if (_npc != null && !_npc.IsAlive)
        {
            SetVisible(false);
            return;
        }

        if (_alwaysVisible)
        {
            SetVisible(true);
            return;
        }

        if (_playerTransform == null)
        {
            SetVisible(true);
            return;
        }

        float distance = Vector3.Distance(transform.position, _playerTransform.position);
        SetVisible(distance <= _visibilityRange);
    }

    private void SetVisible(bool visible)
    {
        IsVisible = visible;
        if (_iconInstance != null)
        {
            _iconInstance.SetActive(visible);
        }
    }

    public void SetIconType(MapIconType type)
    {
        _iconType = type;
        ApplyIconColor();
    }

    public void SetIconColor(Color color)
    {
        _iconColor = color;
        ApplyIconColor();
    }

    private void OnDestroy()
    {
        if (_iconInstance != null)
        {
            Destroy(_iconInstance);
        }
    }
}
