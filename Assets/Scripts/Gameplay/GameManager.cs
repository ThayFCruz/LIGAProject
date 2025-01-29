using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static event Action<bool> OnGetSmallPowerUp;
    public static event Action<bool> OnGetInvinciblePowerUp;
    public static event Action OnTakeDamage;
    
    [SerializeField] GameplayUIManager uiManager;

    private bool isInvincible = false;
    public bool IsInvincible => isInvincible;
    
    private bool isPlaying = false;
    public bool IsPlaying => isPlaying;

    [SerializeField] private float powerUpTime;
    [Range (0, 3)]
    [SerializeField] private int maxHealth;
    [SerializeField] private float levelMaxDistance;
    private float powerUpCooldown;

    private bool activatePowerUp;
    public enum PowerUpType
    {
        INVINCIBLE,
        SMALL_OBSTACLES
    }

    private void Start()
    {
        uiManager.Init(maxHealth, levelMaxDistance);
        isPlaying = true;
    }

    private void Update()
    {
        if(isPlaying && activatePowerUp) {
            powerUpCooldown -= Time.deltaTime;
            if (powerUpCooldown <= 0)
            {
                activatePowerUp = false;
                if (isInvincible)
                {
                    isInvincible = false;
                    OnGetInvinciblePowerUp?.Invoke(false);
                }
                else
                {
                    OnGetSmallPowerUp?.Invoke(false);
                }
            }
            
        }
    }
    public void ActivatePowerUp(PowerUpType type)
    {
        activatePowerUp = true;
        if (type == PowerUpType.INVINCIBLE)
        {
            isInvincible = true;
            OnGetInvinciblePowerUp?.Invoke(true);
        }
        else
            OnGetSmallPowerUp?.Invoke(true);
        powerUpCooldown = powerUpTime;
    }
    
    public void TakeDamage()
    {
        if (isInvincible) return;
            OnTakeDamage?.Invoke();
    }

    public void GameOver()
    {
        isPlaying = false;
    }
}
