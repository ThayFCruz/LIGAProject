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
    private static Transform PlayerTransform;
    private Rigidbody2D _playerRb;
    private static float _initialPoint;
    
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;
    
    
    public float _regularSpeed = 5f;
    public float _speedMultiplier = 2f;
    public float _currentSpeed = 5f;
    public float _jumpForce = 400f;
    public float _bigSize = 1.5f;
    public float _regularSize = 0.6f;

    private bool _onGround;
    
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
        _playerRb.velocity = new Vector2(0, 0);
        _currentSpeed = _regularSpeed;
        _initialPoint = transform.position.x;
    }
    
    public static float GetDistanceFromStart()
    {
        return PosX - _initialPoint;
    }
    
    public void FixedUpdate()
    {
        if (!GameManager.Instance.IsPlaying)
        {
            _animator.SetFloat("speed", 0); 
            return;
        }
        _playerRb.velocity = new Vector2(_currentSpeed, _playerRb.velocity.y);
        _animator.SetFloat("speed", _playerRb.velocity.x); 
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
                _tweenerInvincible = _spriteRenderer.DOFade(0.7f, 0.1f).SetLoops(-1, LoopType.Yoyo);
            });
            _currentSpeed = _regularSpeed * _speedMultiplier;
            _playerRb.mass = 2f;
           
        }
        else
        {
            PlayerTransform.DOScale(_regularSize, 0.5f);
            _spriteRenderer.DOFade(1, 0.5f);
            _currentSpeed = _regularSpeed;
            _tweenerInvincible.Kill();
            _playerRb.mass = 0.6f;
        }
        _animator.SetBool("invincible", isInvincible);
    }

    private void OnTakeDamage(int life)
    {
        //damage animation
        if (life > 0)
        {
            _animator.SetTrigger("Crash");
        }
        else
        {
            GameOver();
        }
    }

    public void OnJump()
    {
        if (!_onGround) return;
        //jump animation
        _playerRb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Force);
        _animator.SetTrigger("jump");
    }

    public void GameOver()
    {
        //dead animation
        _tweenerInvincible.Kill();
        _animator.SetTrigger("Dead");
        _playerRb.velocity = new Vector2(0, 0);
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            _onGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _onGround = false;
        }
    }
}
