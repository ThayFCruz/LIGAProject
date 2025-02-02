using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static float PosX;
    private Rigidbody2D _playerRb;
    private static float _initialPoint;
    
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;
    
    [Header("Sounds")]
    [SerializeField] private AudioClip _jumpSound;
    [SerializeField] private AudioClip _crashSound;
    [SerializeField] private AudioClip _deathSound;
    
    public float _jumpForce = 400f;
    public float _bigSize = 1.5f;
    public float _regularSize = 0.6f;

    private bool _onGround;
    
    Tweener _tweenerInvincible;
	
    public void Start()
    {
        GameManager.OnTakeDamage += OnTakeDamage;
        GameManager.OnGetPowerUp += OnGetPowerUp;
        _playerRb = GetComponent<Rigidbody2D>();
        _playerRb.velocity = new Vector2(0, 0);
        _playerRb.gravityScale = GameManager.Instance.CurrentLevel.gravity;
        _initialPoint = transform.position.x;
        PosX = _initialPoint;
    }
    
    public static float GetDistanceFromStart()
    {
        return (PosX - _initialPoint)/250;
    }
    
    public void FixedUpdate()
    {
        if (!GameManager.Instance.IsPlaying)
        {
            _animator.SetFloat("speed", 0); 
            return;
        }
        _playerRb.velocity = new Vector2(GameManager.Instance.CurrentLevel.speed, _playerRb.velocity.y);
        _animator.SetFloat("speed", _playerRb.velocity.x); 
        PosX = transform.position.x;
    }

    private void OnGetPowerUp(bool isInvincible, GameManager.PowerUpType type)
    {
        if (type != GameManager.PowerUpType.INVINCIBLE) return;
        if (isInvincible)
        {
            transform.DOScale(_bigSize, 0.5f).OnComplete(() =>
            {
                _tweenerInvincible = _spriteRenderer.DOFade(0.7f, 0.1f).SetLoops(-1, LoopType.Yoyo);
            });
            _playerRb.mass = 2f;
           
        }
        else
        {
            transform.DOScale(_regularSize, 0.1f);
            _spriteRenderer.DOFade(1, 0.5f);
            _tweenerInvincible.Kill();
            _playerRb.mass = 0.6f;
        }
        _animator.SetBool("invincible", isInvincible);
    }

    private void OnTakeDamage(int life)
    {
        if (life > 0)
        {
            SoundManager.PlayEffect(_crashSound);
            _animator.SetTrigger("Crash");
        }
        else
        {
            SoundManager.PlayEffect(_deathSound);
            GameOver();
        }
    }

    public void OnJump()
    {
        if (!_onGround || !GameManager.Instance.IsPlaying) return;
        SoundManager.PlayEffect(_jumpSound);
        _playerRb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Force);
    }

    public void GameOver()
    {
        _tweenerInvincible.Kill();
        _animator.SetTrigger("Dead");
        _playerRb.velocity = new Vector2(0, 0);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            _animator.SetBool("grounded", true);
            _onGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _animator.SetBool("grounded", false);
            _onGround = false;
        }
    }

    private void OnDestroy()
    {
        GameManager.OnTakeDamage -= OnTakeDamage;
        GameManager.OnGetPowerUp -= OnGetPowerUp;
    }
}
