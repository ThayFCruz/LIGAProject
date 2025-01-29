using System.Collections;
using System.Collections.Generic;
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
        transform.localScale = status? smallScale : defaultScale;
    }
}
