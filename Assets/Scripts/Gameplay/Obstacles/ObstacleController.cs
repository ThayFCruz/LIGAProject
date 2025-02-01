using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ObstacleController : CollidableObjects
{
    [SerializeField] private Vector2 smallScale;
    [SerializeField] private Vector2 defaultScale;
    
    protected override void Enable()
    {
        transform.localScale = GameManager.Instance.IsSmallPowerUpOn? smallScale : defaultScale;
        base.Enable();
    }

    protected override void OnPlayerEnter()
    {
        if (GameManager.Instance.IsInvincible)
        {
            Disable();
            return;
        }
        
        GameManager.Instance.TakeDamage();
    }

    protected override void OnGetPowerUp(bool status, GameManager.PowerUpType type)
    {
        if (type == GameManager.PowerUpType.SMALL_OBSTACLES)
        {
            transform.DOScale(status ? smallScale : defaultScale, 0.1f);
        }
    }
    
}
