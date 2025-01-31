using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstaclesGenerator : MonoBehaviour
{

    Queue<CollidableObjects> _objectsQueue = new Queue<CollidableObjects>();
    List<CollidableObjects> _pool = new List<CollidableObjects>();
    
    [SerializeField] private List<InformationsByType> typeInformations;
    [Space] 
        
    [SerializeField] private float startPosition;
    // [SerializeField] private float spaceBetween;

    [Space]
    [Header("DistanceBased")] 
        
    [SerializeField] private float distanceToSpawn = 2;
    [SerializeField] private float distanceToDeactivate = 20;

    private Dictionary<TypeCollidableObject, InformationsByType> typeDictionary =
        new Dictionary<TypeCollidableObject, InformationsByType>();
    
    PoolObjects pool;
    
    private float[] _weights;
    private TypeCollidableObject _nextToSpawnType;
    private float _nextToSpawnPosition;
    private float _oldestPos;
    private bool _spawning;
    private Transform _cameraTr;
    
    
    [Serializable]
    public struct InformationsByType
    {
        public TypeCollidableObject type;
        public List<CollidableObjects> prefabs;
        public float spaceBefore;
        public Transform container;
    }
    
    public enum TypeCollidableObject
    {
        OBSTACLE,
        POWER_UP
    }

    private void Start()
    {
        foreach (var info in typeInformations)
        {
            typeDictionary.Add(info.type, info);
        }
        if (Camera.main != null) _cameraTr = Camera.main.transform;
        _nextToSpawnPosition = startPosition;

        _nextToSpawnType = TypeCollidableObject.OBSTACLE;
        var lastBlock = typeDictionary[TypeCollidableObject.OBSTACLE].prefabs[0];
        _oldestPos = lastBlock.Size + lastBlock.SpaceAfter + startPosition;
        Debug.Log("oldest pos: " + _oldestPos);
        _spawning = true;
    }
    
    

    private void SpawnObject()
    {
        CollidableObjects obj = GetNextObject();
        obj.Set(_nextToSpawnPosition);
        _objectsQueue.Enqueue(obj);
       
        _nextToSpawnPosition += obj.Size + obj.SpaceAfter +  typeDictionary[_nextToSpawnType].spaceBefore;

    }

    private CollidableObjects GetNextObject()
    {
        CollidableObjects obj = _pool.Find(x => x.TypeCO == _nextToSpawnType && !x.isActive);
        
        if (obj ==null)
        {
            int index = Random.Range(0,2);
            obj = Instantiate(typeDictionary[_nextToSpawnType].prefabs[index], typeDictionary[_nextToSpawnType].container);
            _pool.Add(obj);
        }
        
        float nextTypeChance = Random.Range(0, 1f);
        _nextToSpawnType = nextTypeChance < 0.95f ? TypeCollidableObject.OBSTACLE : TypeCollidableObject.POWER_UP;
        return obj;
    }
        
    private void DeactivateOldestObject()
    {
        CollidableObjects obstacle =  _objectsQueue.Dequeue();
        obstacle.Disable();

        CollidableObjects lastBlock = _objectsQueue.Peek();
        _oldestPos = (lastBlock).Position + lastBlock.Size + lastBlock.SpaceAfter;
    }
        
    void Update()
    {
        if (!_spawning) return;
	        
        float cameraX = _cameraTr.position.x;

        if (cameraX > _oldestPos && cameraX - _oldestPos > distanceToDeactivate)
        {
            DeactivateOldestObject();
        }
                
        if(Mathf.Abs(_nextToSpawnPosition - cameraX) <= distanceToSpawn)
            SpawnObject();
    }
}
