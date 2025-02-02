using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : Singleton<GameManager>
{
    
    [SerializeField] GameplayUIManager _uiManager;
    [SerializeField] private List<LevelSO> _levels;
    [SerializeField] private AudioClip _gameplayMusic;
    [SerializeField] private AudioClip _startGameSOund;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _powerUpSound;
    
    private int _currentHealth;
    private float _powerUpCooldown;
    private float _currentDistance;
    
    private bool _smallPowerUpOn;
    public bool IsSmallPowerUpOn => _smallPowerUpOn;
    
    public bool _isInvincible = false;
    public bool IsInvincible => _isInvincible;
    
    private bool _isPlaying = false;
    public bool IsPlaying => _isPlaying;
    
    public bool HasPowerUpOn => _isInvincible || _smallPowerUpOn;
    
    private LevelSO _currentLevel;
    public LevelSO CurrentLevel => _currentLevel;
    
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
        _currentHealth = _currentLevel.healthQtt; ;
        SoundManager.PlayEffect(_startGameSOund);
        Invoke(nameof(StartMatch), _startGameSOund.length);
        SoundManager.PlayMusic(_gameplayMusic, true);
    }

    private void StartMatch()
    {
        _isPlaying = true;
        _audioSource.Play();
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
             _currentDistance = PlayerController.GetDistanceFromStart();
            _uiManager.UpdateDistance(_currentDistance);
            if (_currentLevel.distance > 0 && _currentDistance >= _currentLevel.distance)
            {
                GameOver(true);
            }
        }
    }
    public void ActivatePowerUp(PowerUpType type, float cooldown)
    {
        OnGetPowerUp?.Invoke(true, type);
        SoundManager.PlayEffect(_powerUpSound);
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
        if (completed)
        {
            AnalyticsManager.Instance.CompletedLevel(_currentLevel.level.ToString());
        }
        else
        {
            AnalyticsManager.Instance.FailLevel(_currentLevel.level.ToString(), _currentDistance);
        }
        _isInvincible = completed;
        _audioSource.Stop();
        _isPlaying = false;
        OnFinishLevel?.Invoke(completed);
    }
}
