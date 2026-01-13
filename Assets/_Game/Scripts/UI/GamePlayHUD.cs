using TMPro;
using UnityEngine;

public class GamePlayHUD : MonoBehaviour
{
    public GameObject player;
    public TextMeshProUGUI healthText;
    private PlayerStats playerStats;

    public SonNPC sonNPC;
    public TextMeshProUGUI sonHealthText;

    // Ammo display
    public TextMeshProUGUI ammoText;
    private CharacterEntity playerEntity;


    void Start()
    {
        playerStats = player.GetComponent<PlayerStats>();
        playerEntity = player.GetComponent<CharacterEntity>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || healthText == null) return;
        if (playerStats != null)
        {
            healthText.text = "Health: " + playerStats.Health.ToString() + "/" + playerStats.MaxHealth.ToString();
        }

        if (sonNPC == null) return;
        sonHealthText.text = "Son Health: " + sonNPC.Health.ToString() + "/" + sonNPC.MaxHealth.ToString();

        // Update ammo display
        UpdateAmmoDisplay();
    }

    private void UpdateAmmoDisplay()
    {
        if (ammoText == null) return;

        if (playerEntity != null && playerEntity.EquippedItem2 is Pistol1Entity pistol)
        {
            if (pistol.IsReloading)
            {
                ammoText.text = "Reloading...";
            }
            else
            {
                ammoText.text = $"Ammo: {pistol.CurrentAmmo}/{pistol.MagazineSize}";
            }
            ammoText.gameObject.SetActive(true);
        }
        else
        {
            ammoText.gameObject.SetActive(false);
        }
    }
}