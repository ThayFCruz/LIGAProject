using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : Singleton<GameManager>
{
    
    [SerializeField] GameplayUIManager _uiManager;
    private float _powerUpTime;
    [Range (0, 3)]
    [SerializeField] private int _maxHealth;
    [SerializeField] private float _levelMaxDistance;
    
    private float _powerUpCooldown;

    private bool _activatePowerUp;
    
    public bool _isInvincible = false;
    public bool IsInvincible => _isInvincible;
    
    private bool _isPlaying = false;
    public bool IsPlaying => _isPlaying;

    private int _currentHealth;
    
    public static event Action<bool> OnGetSmallPowerUp;
    public static event Action<bool> OnGetInvinciblePowerUp;
    public static event Action<int> OnTakeDamage;
    public enum PowerUpType
    {
        INVINCIBLE,
        SMALL_OBSTACLES
    }

    private void Start()
    {
        _uiManager.Init(_maxHealth, _levelMaxDistance);
        _currentHealth = _maxHealth;
        Invoke(nameof(StartMatch), 2f);
    }

    private void StartMatch()
    {
        _isPlaying = true;
    }

    private void Update()
    {
        if(_isPlaying && _activatePowerUp) {
            _powerUpCooldown -= Time.deltaTime;
            if (_powerUpCooldown <= 0)
            {
                _activatePowerUp = false;
                if (_isInvincible)
                {
                    _isInvincible = false;
                    OnGetInvinciblePowerUp?.Invoke(false);
                }
                else
                {
                    OnGetSmallPowerUp?.Invoke(false);
                }
            }
            
        }

        if (_isPlaying)
        {
             float distance = PlayerController.GetDistanceFromStart();
            _uiManager.UpdateDistance(distance);
            if (distance >= _levelMaxDistance)
            {
                GameOver();
            }
        }
    }
    public void ActivatePowerUp(PowerUpType type, float cooldown)
    {
        _activatePowerUp = true;
        if (type == PowerUpType.INVINCIBLE)
        {
            _isInvincible = true;
            OnGetInvinciblePowerUp?.Invoke(true);
        }
        else
            OnGetSmallPowerUp?.Invoke(true);
        _powerUpCooldown = cooldown;
    }
    
    public void TakeDamage()
    {
        if (_isInvincible) return;
        _currentHealth--;
        OnTakeDamage?.Invoke(_currentHealth);
        if(_currentHealth <= 0)
            GameOver();
    }

    public void GameOver()
    {
        _isPlaying = false;
    }
}
