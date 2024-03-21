using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] Image healthBar;
    [SerializeField] TMP_Text currentAmmoText;
    [SerializeField] TMP_Text maxAmmoText;

    FPSController player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<FPSController>();
    }

    public void SetAmmo(int maxAmmo, int ammo)
    {
        maxAmmoText.text = maxAmmo.ToString();
        currentAmmoText.text = ammo.ToString();
    }

    public void SetHealth(float maxHealth, float health)
    {
        healthBar.fillAmount = health/maxHealth;
    }
}
