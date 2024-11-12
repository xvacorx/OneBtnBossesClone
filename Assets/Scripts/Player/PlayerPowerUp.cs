using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPowerUp : MonoBehaviour
{
    [Header("Power-Up")]
    public float speedBoost = 2f;
    public float energy = 100f;
    public float energyDepletionRate = 20f;
    public float rechargeRate = 10f;
    public float rechargeDelay = 2f;

    [Header("UI")]
    public Slider energyBar;
    public TMP_Text energyText;

    private PlayerMovement playerMovement;
    private bool isPowerUpActive = false;
    private bool canRecharge = true;
    private Collider2D playerCollider;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerCollider = GetComponent<Collider2D>();

        if (energyBar != null)
        {
            energyBar.maxValue = 100f;
            energyBar.value = energy;
        }

        UpdateEnergyUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && energy >= 100 && !isPowerUpActive)
        {
            ActivatePowerUp();
        }

        if (isPowerUpActive)
        {
            ConsumeEnergy();
        }
        else if (energy < 100f && canRecharge)
        {
            RechargeEnergy();
        }

        UpdateEnergyUI();
    }

    private void ActivatePowerUp()
    {
        isPowerUpActive = true;
        canRecharge = false;
        playerMovement.speed *= speedBoost;
        playerMovement.canChangeDirection = false;
        SetInvulnerability(true);
        Invoke(nameof(AllowRecharge), rechargeDelay);
    }

    private void DeactivatePowerUp()
    {
        isPowerUpActive = false;
        playerMovement.speed /= speedBoost;
        playerMovement.canChangeDirection = true;
        SetInvulnerability(false);
    }

    private void ConsumeEnergy()
    {
        energy -= energyDepletionRate * Time.deltaTime;
        if (energy <= 0)
        {
            energy = 0;
            DeactivatePowerUp();
        }
    }

    private void RechargeEnergy()
    {
        energy += rechargeRate * Time.deltaTime;
        if (energy >= 100f)
        {
            energy = 100f;
        }
    }

    private void AllowRecharge()
    {
        canRecharge = true;
    }

    private void UpdateEnergyUI()
    {
        if (energyBar != null)
        {
            energyBar.value = energy;
        }

        if (energyText != null)
        {
            energyText.text = $"Energía: {energy:F0}%";
        }
    }

    public bool IsInvulnerable()
    {
        return isPowerUpActive;
    }

    private void SetInvulnerability(bool state)
    {
        playerCollider.enabled = !state;
    }
}
