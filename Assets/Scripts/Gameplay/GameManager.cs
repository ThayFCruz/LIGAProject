using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : Singleton<GameManager>
{
    
    [SerializeField] GameplayUIManager _uiManager;
    private float _powerUpTime;
    protected override bool DestroyOnLoad => false;
    
    [SerializeField] private List<LevelSO> _levels;
    
    private float _powerUpCooldown;

    private bool _activatePowerUp;
    
    public bool _isInvincible = false;
    public bool IsInvincible => _isInvincible;
    
    private bool _isPlaying = false;
    public bool IsPlaying => _isPlaying;

    private int _currentHealth;

    [SerializeField] private LevelSO _currentLevel;
    public LevelSO CurrentLevel => _currentLevel;
    
    public static event Action<bool> OnGetSmallPowerUp;
    public static event Action<bool> OnGetInvinciblePowerUp;
    public static event Action<int> OnTakeDamage;
    
    public static event Action<bool> OnFinishLevel;
    public enum PowerUpType
    {
        INVINCIBLE,
        SMALL_OBSTACLES
    }

    private void Start()
    {
        // _currentLevel = _levels[PlayerPrefs.GetInt(Constants.current_level, 0)];
        _uiManager.Init();
        _currentHealth = _currentLevel.healthQtt;
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
            if (_currentLevel.distance > 0 && distance >= _currentLevel.distance)
            {
                GameOver(true);
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
            GameOver(false);
    }

    public void GameOver(bool completed)
    {
        _isPlaying = false;
        OnFinishLevel?.Invoke(completed);
    }
}
