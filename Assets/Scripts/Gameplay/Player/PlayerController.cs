using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static float PosX;
    public static float PosY;
    public static Transform PlayerTransform;
    private Rigidbody2D _playerRb;
    private static float initialPoint;
    
    [Header("Player Attributes")]
    public float baseSpeed = 2f;
    
    private void Awake()
    {
        PlayerTransform = transform;
        PosX = 0;
    }
	
    public void Start()
    {
        GameManager.OnTakeDamage += OnTakeDamage;
        GameManager.OnGetInvinciblePowerUp += OnGetInvinciblePowerUp;
        _playerRb = GetComponent<Rigidbody2D>();
        initialPoint = transform.position.x;
    }
    
    public static float GetDistanceFromStart()
    {
        return PosX - initialPoint;
    }
    
    public void FixedUpdate()
    {
        if (!GameManager.Instance.IsPlaying) return;
        _playerRb.velocity = new Vector2(baseSpeed, _playerRb.velocity.y);
        var position = transform.position;
        PosX = position.x;
        PosY = position.y;
    }

    private void OnGetInvinciblePowerUp(bool isInvincible)
    {
        PlayerTransform.localScale = isInvincible? new Vector3(1.4f, 1.4f, 1.4f): new Vector3(0.6f, 0.6f, 0.6f);
    }

    private void OnTakeDamage()
    {
        //damage animation
    }

    private void OnJump()
    {
        //jump animation
    }

    public void GameOver()
    {
        //dead animation
        _playerRb.velocity = new Vector2(0, 0);
    }
}
