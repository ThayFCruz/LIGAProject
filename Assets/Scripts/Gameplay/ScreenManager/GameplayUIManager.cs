using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameplayUIManager : MonoBehaviour
{
    [Header("Health Elements")]
    private int _maxHealth;
    [SerializeField] private Sprite _fullHeartSprite;
    [SerializeField] private Sprite _emptyHeartSprite;
    [SerializeField] private Image[] _hearts;
    [SerializeField] private Animator _deadAnimation;
    
    [Space(10)]
    [Header("Distance elements")]
    private float _maxDistance;
    private float _currentDistance;
    private bool _hasMaxDistance;
    [SerializeField] private Image _filledDistanceImage;
    [SerializeField] private TextMeshProUGUI _distanceText;
    
    [Space(10)]
    [Header("PowerUps elements")]
    [SerializeField] private Image _powerUpImage;
    [SerializeField] private Sprite[] _powerUpSprites;
    
    
    Tweener _tweener;
    private void Start()
    {
        GameManager.OnGetInvinciblePowerUp += (status) => ActivatePowerUp(status, GameManager.PowerUpType.INVINCIBLE);
        GameManager.OnGetSmallPowerUp += (status) => ActivatePowerUp(status, GameManager.PowerUpType.SMALL_OBSTACLES);
        GameManager.OnTakeDamage += SetHeartSprite;
        _powerUpImage.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        UpdateDistance(PlayerController.GetDistanceFromStart());
    }

    public void Init(int levelMaxHealth, float levelMaxDistance, bool levelHasMaxDistance = true)
    {
        _maxHealth = levelMaxHealth;
        _maxDistance = levelMaxDistance;
        _hasMaxDistance = levelHasMaxDistance;
        SetHeartSprite(_maxHealth);
        _filledDistanceImage.gameObject.SetActive(_hasMaxDistance);
        _filledDistanceImage.fillAmount = 0;
    }

    public void SetHeartSprite(int health)
    {
        for (int i = 0; i< _hearts.Length; i++) 
        {
            _hearts[i].gameObject.SetActive(_maxHealth > i);
            _hearts[i].sprite = health  > i ? _fullHeartSprite : _emptyHeartSprite;
        }

        if (health <= 0)
        {
            _deadAnimation.gameObject.SetActive(true);
            _deadAnimation.Play("crash");
        }
    }

    public void UpdateDistance(float distance)
    {
        _distanceText.text = distance/20 + " km";
        
        if (!_hasMaxDistance) return;

        float fill = distance / _maxDistance;
        _filledDistanceImage.fillAmount = fill;
    }

    private void ActivatePowerUp(bool status, GameManager.PowerUpType type)
    {
        _powerUpImage.sprite = _powerUpSprites[(int)type];
        if(status)
            _tweener = _powerUpImage.DOFade(0.5f, 0.2f).SetLoops(-1, LoopType.Yoyo);
        else
            _tweener.Kill();
        
        _powerUpImage.gameObject.SetActive(status);
    }
}

