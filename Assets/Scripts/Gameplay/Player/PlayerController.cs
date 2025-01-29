using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static float PosX;
    public static float PosY;
    public static Transform PlayerTransform;
    private Rigidbody2D _playerRb;
    private static float _initialPoint;
    
    private SpriteRenderer _spriteRenderer;
    
    public float _regularSpeed = 5f;
    public float _speedMultiplier = 2f;
    public float _currentSpeed = 5f;
    public float _jumpForce = 400f;
    public float _bigSize = 1.5f;
    public float _regularSize = 0.6f;
    
    Tweener _tweenerInvincible;
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
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _initialPoint = transform.position.x;
    }
    
    public static float GetDistanceFromStart()
    {
        return PosX - _initialPoint;
    }
    
    public void FixedUpdate()
    {
        if (!GameManager.Instance.IsPlaying) return;
        _playerRb.velocity = new Vector2(_currentSpeed, _playerRb.velocity.y);
        var position = transform.position;
        PosX = position.x;
        PosY = position.y;
        
        
    }

    private void OnGetInvinciblePowerUp(bool isInvincible)
    {
        if (isInvincible)
        {
            PlayerTransform.DOScale(_bigSize, 0.5f).OnComplete(() =>
            {
                _tweenerInvincible = _spriteRenderer.DOFade(0, 0.05f).SetLoops(-1, LoopType.Yoyo);
            });
            _currentSpeed = _regularSpeed * _speedMultiplier;
            _playerRb.mass = 2f;
           
        }
        else
        {
            PlayerTransform.DOScale(_regularSize, 0.5f);
            _spriteRenderer.DOFade(1, 0.05f);
            _currentSpeed = _regularSpeed / _speedMultiplier;
            _tweenerInvincible.Kill();
            _playerRb.mass = 0.6f;
        }
    }

    private void OnTakeDamage()
    {
        //damage animation
    }

    public void OnJump()
    {
        //jump animation
        _playerRb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Force);
    }

    public void GameOver()
    {
        //dead animation
        _tweenerInvincible.Kill();
        _playerRb.velocity = new Vector2(0, 0);
    }
}
