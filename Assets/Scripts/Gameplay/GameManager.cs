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
    
    private int _currentHealth;

    private float _powerUpCooldown;

    private bool _smallPowerUpOn;
    public bool IsSmallPowerUpOn => _smallPowerUpOn;
    
    public bool _isInvincible = false;
    public bool IsInvincible => _isInvincible;
    
    
    private bool _isPlaying = false;
    public bool IsPlaying => _isPlaying;
    

    private LevelSO _currentLevel;
    public LevelSO CurrentLevel => _currentLevel;

    public bool HasPowerUpOn => _isInvincible || _smallPowerUpOn;
    
    public static event Action<bool, PowerUpType> OnGetPowerUp;
    public static event Action<int> OnTakeDamage;
    
    public static event Action<bool> OnFinishLevel;
    public enum PowerUpType
    {
        INVINCIBLE,
        SMALL_OBSTACLES
    }

    private void Start()
    {
        _currentLevel = _levels[PlayerPrefs.GetInt(Constants.current_level, 0)];
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
        if(_isPlaying && HasPowerUpOn) {
            _powerUpCooldown -= Time.deltaTime;
            if (_powerUpCooldown <= 0)
            {
                OnGetPowerUp?.Invoke(false, _isInvincible? PowerUpType.INVINCIBLE : PowerUpType.SMALL_OBSTACLES);
                
                if (_isInvincible)
                {
                    _isInvincible = false;
                }
                else
                {
                    _smallPowerUpOn = false;
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
        OnGetPowerUp?.Invoke(true, type);
        if (type == PowerUpType.INVINCIBLE)
        {
            _isInvincible = true;
        }
        else
        {
            _smallPowerUpOn = true;
        }
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
