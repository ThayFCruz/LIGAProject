using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PowerUpController : CollidableObjects
{
    [SerializeField] private GameManager.PowerUpType _powerUpType;
    [SerializeField] private float _cooldown;
    
    
    protected override void OnPlayerEnter()
    {
        GameManager.Instance.ActivatePowerUp(_powerUpType, _cooldown);
        Disable();
    }
    
    protected override void OnGetPowerUp(bool status, GameManager.PowerUpType type)
    {
        if (!status) return;
        
        Disable();
    }
}
