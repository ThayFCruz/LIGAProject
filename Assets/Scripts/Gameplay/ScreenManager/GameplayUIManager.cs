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
    private int maxHealth;
    private int currentHealth;
    [SerializeField] private Sprite fullHeartSprite;
    [SerializeField] private Sprite emptyHeartSprite;
    [SerializeField] private Image[] hearts;
    
    [Space(10)]
    [Header("Distance elements")]
    private float maxDistance;
    private float currentDistance;
    private bool hasMaxDistance;
    [SerializeField] private Image filledDistanceImage;
    [SerializeField] private TextMeshProUGUI distanceText;
    
    [Space(10)]
    [Header("PowerUps elements")]
    [SerializeField] private Image powerUpImage;
    [SerializeField] private Sprite[] powerUpSprites;
    
    Tweener _tweener;
    private void Start()
    {
        GameManager.OnGetInvinciblePowerUp += (status) => ActivatePowerUp(status, GameManager.PowerUpType.INVINCIBLE);
        GameManager.OnGetSmallPowerUp += (status) => ActivatePowerUp(status, GameManager.PowerUpType.SMALL_OBSTACLES);
        GameManager.OnTakeDamage += TakeDamage;
        powerUpImage.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        UpdateDistance(PlayerController.GetDistanceFromStart());
    }

    public void Init(int levelMaxHealth, float levelMaxDistance, bool levelHasMaxDistance = true)
    {
        maxHealth = levelMaxHealth;
        maxDistance = levelMaxDistance;
        hasMaxDistance = levelHasMaxDistance;
        SetHeartSprite(maxHealth);
        filledDistanceImage.gameObject.SetActive(hasMaxDistance);
        filledDistanceImage.fillAmount = 0;
    }

    private void SetHeartSprite(int health)
    {
        currentHealth = health;
        for (int i = 0; i< hearts.Length; i++) 
        {
            hearts[i].gameObject.SetActive(maxHealth > i);
            hearts[i].sprite = currentHealth  > i ? fullHeartSprite : emptyHeartSprite;
        }
    }
    
    private void TakeDamage()
    {
        if (GameManager.Instance.IsInvincible) return;
        //crash animation
        SetHeartSprite(currentHealth - 1); 
        if (currentHealth <= 0)
        {
            GameManager.Instance.GameOver();
        }
    }

    private void UpdateDistance(float distance)
    {
        distanceText.text = distance + " km";
        
        if (!hasMaxDistance) return;

        float fill = distance / maxDistance;
        filledDistanceImage.fillAmount = fill;

        if (fill >= 1)
        {
            GameManager.Instance.GameOver();
        }
    }

    private void ActivatePowerUp(bool status, GameManager.PowerUpType type)
    {
        powerUpImage.sprite = powerUpSprites[(int)type];
        if(status)
            _tweener = powerUpImage.DOFade(0.5f, 0.2f).SetLoops(-1, LoopType.Yoyo);
        else
            _tweener.Kill();
        
        powerUpImage.gameObject.SetActive(status);
    }
}

