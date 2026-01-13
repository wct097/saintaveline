using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Displays a legend explaining the map icon meanings.
/// Can be toggled on/off in the full map view.
/// </summary>
public class MapLegend : MonoBehaviour
{
    [System.Serializable]
    public class LegendEntry
    {
        public MapIconType iconType;
        public string label;
        public Color color;
        public Sprite iconSprite;
    }

    [Header("Legend Entries")]
    [SerializeField] private LegendEntry[] _defaultEntries = new LegendEntry[]
    {
        new LegendEntry { iconType = MapIconType.Player, label = "You", color = Color.white },
        new LegendEntry { iconType = MapIconType.FamilyMember, label = "Family Member", color = Color.green },
        new LegendEntry { iconType = MapIconType.Enemy, label = "Enemy", color = Color.red },
        new LegendEntry { iconType = MapIconType.LabeledPoint, label = "Marked Location", color = Color.yellow },
        new LegendEntry { iconType = MapIconType.Objective, label = "Objective", color = Color.cyan }
    };

    [Header("UI References")]
    [SerializeField] private GameObject _entryPrefab;
    [SerializeField] private Transform _entriesContainer;
    [SerializeField] private Button _toggleButton;
    [SerializeField] private GameObject _legendPanel;

    private bool _isVisible = true;

    private void Start()
    {
        if (_toggleButton != null)
        {
            _toggleButton.onClick.AddListener(Toggle);
        }

        PopulateLegend();
    }

    private void PopulateLegend()
    {
        if (_entryPrefab == null || _entriesContainer == null) return;

        foreach (Transform child in _entriesContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var entry in _defaultEntries)
        {
            CreateLegendEntry(entry);
        }
    }

    private void CreateLegendEntry(LegendEntry entry)
    {
        if (_entryPrefab == null || _entriesContainer == null) return;

        GameObject entryObj = Instantiate(_entryPrefab, _entriesContainer);

        var iconImage = entryObj.transform.Find("Icon")?.GetComponent<Image>();
        if (iconImage != null)
        {
            if (entry.iconSprite != null)
            {
                iconImage.sprite = entry.iconSprite;
            }
            iconImage.color = entry.color;
        }

        var labelText = entryObj.transform.Find("Label")?.GetComponent<TextMeshProUGUI>();
        if (labelText != null)
        {
            labelText.text = entry.label;
        }
    }

    public void Show()
    {
        _isVisible = true;
        if (_legendPanel != null)
        {
            _legendPanel.SetActive(true);
        }
    }

    public void Hide()
    {
        _isVisible = false;
        if (_legendPanel != null)
        {
            _legendPanel.SetActive(false);
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

    public void SetVisible(bool visible)
    {
        if (visible)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
}
