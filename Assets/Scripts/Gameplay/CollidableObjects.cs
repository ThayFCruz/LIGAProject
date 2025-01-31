using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollidableObjects : MonoBehaviour
{
    [SerializeField] private float size;
    [SerializeField] private float spaceAfter;
    [SerializeField] private ObstaclesGenerator.TypeCollidableObject obstacleType;
    
    public float Size => size;
    public float SpaceAfter => spaceAfter;
    
    public float Position { get; private set; }
    
    public bool isActive { get; private set; }
    
    public ObstaclesGenerator.TypeCollidableObject TypeCO => obstacleType;
    
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
    
    public virtual void Disable()
    {
        isActive = false;
        gameObject.SetActive(false);
    }

    protected abstract void OnPlayerEnter();

    public void Set(float position)
    {
        Enable();
        Position = position;
        transform.localPosition = new Vector3(position, 0);
    }
}
