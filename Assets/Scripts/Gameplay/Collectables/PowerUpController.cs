using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : CollidableObjects
{
    [SerializeField] private GameManager.PowerUpType type;

    protected override void OnPlayerEnter()
    {
        GameManager.Instance.ActivatePowerUp(type);
        Disable();
    }
}
