using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PowerUpController : CollidableObjects
{
    [SerializeField] private GameManager.PowerUpType powerUpType;
    [SerializeField] private float cooldown;

    protected override void OnPlayerEnter()
    {
        GameManager.Instance.ActivatePowerUp(powerUpType, cooldown);
        Disable();
    }
}
