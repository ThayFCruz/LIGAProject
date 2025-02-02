using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ObstaclesGenerator : MonoBehaviour
{

    Queue<CollidableObjectsController> _objectsQueue = new Queue<CollidableObjectsController>();
    List<CollidableObjectsController> _pool = new List<CollidableObjectsController>();
    
    [SerializeField] private List<InformationsByType> _typeInformations;
    
    [SerializeField] private float _startPosition;
    [SerializeField] private float _distanceToSpawn = 2;
    [SerializeField] private float _distanceToDeactivate = 20;

    private Dictionary<TypeCollidableObject, InformationsByType> typeDictionary =
        new Dictionary<TypeCollidableObject, InformationsByType>();
    
    private TypeCollidableObject _nextToSpawnType;
    private float _nextToSpawnPosition;
    private float _oldestPos;
    private bool _spawning;
    private Transform _cameraTr;

    private int powerUpsCount;
    
    
    [Serializable]
    public struct InformationsByType
    {
        public TypeCollidableObject type;
        public List<CollidableObjectsController> prefabs;
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
        InitialSettings();
       
        if (Camera.main != null) _cameraTr = Camera.main.transform;

        GameManager.OnFinishLevel += (x) => { _spawning = false; };
        _spawning = true;
    }

    private void InitialSettings()
    {
        foreach (var info in _typeInformations)
        {
            typeDictionary.Add(info.type, info);
        }

        foreach (var pu in typeDictionary[TypeCollidableObject.POWER_UP].prefabs)
        {
            CollidableObjectsController obj = Instantiate(pu, typeDictionary[TypeCollidableObject.POWER_UP].container);
            _pool.Add(obj);
            obj.Disable(true);
        }
        
        _nextToSpawnPosition = _startPosition;
        _nextToSpawnType = TypeCollidableObject.OBSTACLE;
        _oldestPos =  typeDictionary[TypeCollidableObject.OBSTACLE].prefabs[0].SpaceAfter + _startPosition;
        powerUpsCount = 0;
    }
    
    

    private void SpawnObject()
    {
        CollidableObjectsController obj = GetNextObject();
        obj.Set(_nextToSpawnPosition);
        _objectsQueue.Enqueue(obj);
        
        if (obj.TypeCO == TypeCollidableObject.POWER_UP)
            powerUpsCount++;
        
        float newSpaceBetween = Random.Range(GameManager.Instance.CurrentLevel.distanceBetweenObjects-3, GameManager.Instance.CurrentLevel.distanceBetweenObjects);
        _nextToSpawnPosition += obj.SpaceAfter +  newSpaceBetween + typeDictionary[_nextToSpawnType].spaceBefore;

    }

    private CollidableObjectsController GetNextObject()
    {
        CollidableObjectsController obj = GetObjectFromPool();
        
        if (obj ==null)
        {
            int index = Random.Range(0,2);
            obj = Instantiate(typeDictionary[_nextToSpawnType].prefabs[index], typeDictionary[_nextToSpawnType].container);
            _pool.Add(obj);
        }

        if (powerUpsCount <= 0 && obj.TypeCO != TypeCollidableObject.POWER_UP && !GameManager.Instance.HasPowerUpOn)
        {
            float nextTypeChance = Random.Range(0, 1f);
            _nextToSpawnType = nextTypeChance < 0.97f ? TypeCollidableObject.OBSTACLE : TypeCollidableObject.POWER_UP;
        }
        else
        {
            _nextToSpawnType = TypeCollidableObject.OBSTACLE;
        }
        
        return obj;
    }

    private CollidableObjectsController GetObjectFromPool()
    {
        List<CollidableObjectsController> objs = _pool.FindAll(x => x.TypeCO == _nextToSpawnType && !x.isActive);
        if(objs.Count == 0) return null;
        
        return objs[Random.Range(0, objs.Count)];
    }
        
    private void DeactivateOldestObject()
    {
        CollidableObjectsController obstacle =  _objectsQueue.Dequeue();
        obstacle.Disable(true);
        if(obstacle.TypeCO == TypeCollidableObject.POWER_UP)
            powerUpsCount--;
        CollidableObjectsController lastBlock = _objectsQueue.Peek();
        _oldestPos = (lastBlock).Position + lastBlock.SpaceAfter;
    }
        
    void Update()
    {
        if (!_spawning) return;
	        
        float cameraX = _cameraTr.position.x;

        if (cameraX > _oldestPos && cameraX - _oldestPos > _distanceToDeactivate)
        {
            DeactivateOldestObject();
        }
                
        if(Mathf.Abs(_nextToSpawnPosition - cameraX) <= _distanceToSpawn)
            SpawnObject();
    }
}
