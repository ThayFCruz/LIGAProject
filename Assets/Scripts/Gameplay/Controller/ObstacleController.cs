using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ObstacleController : CollidableObjectsController
{
    [SerializeField] private Vector2 smallScale;
    [SerializeField] private Vector2 defaultScale;
    [SerializeField] private AudioClip destroySound;
    private bool canCollide;
    
    protected override void Enable()
    {
        transform.localScale = GameManager.Instance.IsSmallPowerUpOn? smallScale : defaultScale;
        base.Enable();
        canCollide = true;
    }

    protected override void OnPlayerEnter()
    {
        if (!canCollide) return;
        canCollide = false;
        if (GameManager.Instance.IsInvincible)
        {
            SoundManager.PlayEffect(destroySound);
            transform.DOScale(new Vector3(transform.localScale.x, 0,transform.localScale.z), 0.2f).SetDelay(0.2f).OnComplete(() => Disable());

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
