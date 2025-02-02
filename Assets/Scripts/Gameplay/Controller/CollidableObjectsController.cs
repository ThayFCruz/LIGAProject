using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class CollidableObjectsController : MonoBehaviour
{
    [SerializeField] private float _spaceAfter;
    [SerializeField] private ObstaclesGenerator.TypeCollidableObject _obstacleType;
    
    public float SpaceAfter => _spaceAfter;
    
    public float Position { get; private set; }
    
    public bool isActive { get; private set; }
    
    public ObstaclesGenerator.TypeCollidableObject TypeCO => _obstacleType;

    private void Start()
    {
        GameManager.OnGetPowerUp += OnGetPowerUp;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            OnPlayerEnter();
        }
    }
    
    protected virtual void Enable()
    {
        isActive = true;
        gameObject.SetActive(true);
    }
    
    public virtual void Disable(bool deactivate = false)
    {
        gameObject.SetActive(false);
        if (deactivate)
        {
            Deactivate();
        }
    }

    public void Deactivate()
    {
        isActive = false;
    }

    protected virtual void OnGetPowerUp(bool status, GameManager.PowerUpType type)
    {
        
    }
    
    protected abstract void OnPlayerEnter();

    public void Set(float position)
    {
        Enable();
        Position = position;
        transform.localPosition = new Vector3(position, 0);
    }

    private void OnDestroy()
    {
        GameManager.OnGetPowerUp -= OnGetPowerUp;
    }
}
