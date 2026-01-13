using UnityEngine;

/// <summary>
/// Singleton for playing UI sounds (button clicks, inventory open/close, etc.).
/// Provides static methods for easy access from UI scripts.
/// </summary>
public class UISounds : MonoBehaviour
{
    public static UISounds Instance { get; private set; }

    [Header("Button Sounds")]
    [SerializeField] private AudioClip _buttonClick;
    [SerializeField] private AudioClip _buttonHover;

    [Header("Inventory Sounds")]
    [SerializeField] private AudioClip _inventoryOpen;
    [SerializeField] private AudioClip _inventoryClose;
    [SerializeField] private AudioClip _itemPickup;
    [SerializeField] private AudioClip _itemDrop;
    [SerializeField] private AudioClip _itemEquip;

    [Header("Menu Sounds")]
    [SerializeField] private AudioClip _menuOpen;
    [SerializeField] private AudioClip _menuClose;
    [SerializeField] private AudioClip _tabSwitch;

    [Header("Notification Sounds")]
    [SerializeField] private AudioClip _notification;
    [SerializeField] private AudioClip _errorSound;
    [SerializeField] private AudioClip _successSound;

    [Header("Volume")]
    [SerializeField] [Range(0f, 1f)] private float _uiVolume = 0.7f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip == null) return;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX2D(clip, _uiVolume);
        }
        else
        {
            // Fallback: Create temporary audio source
            GameObject tempObj = new GameObject("TempUIAudio");
            AudioSource source = tempObj.AddComponent<AudioSource>();
            source.clip = clip;
            source.volume = _uiVolume;
            source.spatialBlend = 0f;
            source.Play();
            Destroy(tempObj, clip.length + 0.1f);
        }
    }

    // Button sounds
    public void PlayButtonClick() => PlaySound(_buttonClick);
    public void PlayButtonHover() => PlaySound(_buttonHover);

    // Inventory sounds
    public void PlayInventoryOpen() => PlaySound(_inventoryOpen);
    public void PlayInventoryClose() => PlaySound(_inventoryClose);
    public void PlayItemPickup() => PlaySound(_itemPickup);
    public void PlayItemDrop() => PlaySound(_itemDrop);
    public void PlayItemEquip() => PlaySound(_itemEquip);

    // Menu sounds
    public void PlayMenuOpen() => PlaySound(_menuOpen);
    public void PlayMenuClose() => PlaySound(_menuClose);
    public void PlayTabSwitch() => PlaySound(_tabSwitch);

    // Notification sounds
    public void PlayNotification() => PlaySound(_notification);
    public void PlayError() => PlaySound(_errorSound);
    public void PlaySuccess() => PlaySound(_successSound);

    // Static convenience methods
    public static void ButtonClick()
    {
        if (Instance != null) Instance.PlayButtonClick();
    }

    public static void ButtonHover()
    {
        if (Instance != null) Instance.PlayButtonHover();
    }

    public static void InventoryOpen()
    {
        if (Instance != null) Instance.PlayInventoryOpen();
    }

    public static void InventoryClose()
    {
        if (Instance != null) Instance.PlayInventoryClose();
    }

    public static void ItemPickup()
    {
        if (Instance != null) Instance.PlayItemPickup();
    }

    public static void ItemDrop()
    {
        if (Instance != null) Instance.PlayItemDrop();
    }

    public static void ItemEquip()
    {
        if (Instance != null) Instance.PlayItemEquip();
    }

    public static void MenuOpen()
    {
        if (Instance != null) Instance.PlayMenuOpen();
    }

    public static void MenuClose()
    {
        if (Instance != null) Instance.PlayMenuClose();
    }

    public static void TabSwitch()
    {
        if (Instance != null) Instance.PlayTabSwitch();
    }

    public static void Notification()
    {
        if (Instance != null) Instance.PlayNotification();
    }

    public static void Error()
    {
        if (Instance != null) Instance.PlayError();
    }

    public static void Success()
    {
        if (Instance != null) Instance.PlaySuccess();
    }
}
