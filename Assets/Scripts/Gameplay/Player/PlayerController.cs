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
        _playerRb = GetComponent<Rigidbody2D>();
        initialPoint = transform.position.x;
    }
    
    public static float GetDistanceFromStart()
    {
        return PosX - initialPoint;
    }
    
    public void FixedUpdate()
    {
        _playerRb.velocity = new Vector2(baseSpeed, _playerRb.velocity.y);
        var position = transform.position;
        PosX = position.x;
        PosY = position.y;
    }

    public void GameOver()
    {
        _playerRb.velocity = new Vector2(0, 0);
    }
}
