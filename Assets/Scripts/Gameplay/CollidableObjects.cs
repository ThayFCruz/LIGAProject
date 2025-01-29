using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollidableObjects : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            OnPlayerEnter();
        }
    }
    
    
    protected virtual void Enable()
    {
        gameObject.SetActive(true);
    }
    
    public virtual void Disable()
    {
        gameObject.SetActive(false);
    }

    protected abstract void OnPlayerEnter();
    
}
