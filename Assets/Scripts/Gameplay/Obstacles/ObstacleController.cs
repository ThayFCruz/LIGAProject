using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ObstacleController : CollidableObjects
{
    [SerializeField] private Vector2 smallScale;
    [SerializeField] private Vector2 defaultScale;
    void Start()
    {
        GameManager.OnGetSmallPowerUp += SmallPowerUp;
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

    private void SmallPowerUp(bool status)
    {
        transform.DOScale(status ? smallScale : defaultScale, 0.1f);
    }

    public virtual void Disable()
    {
        base.Disable();
        transform.localScale = defaultScale;
    }

    private void OnDestroy()
    {
        GameManager.OnGetSmallPowerUp -= SmallPowerUp;
    }
}
