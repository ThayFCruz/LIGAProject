using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/LevelSO")]
public class LevelSO : ScriptableObject
{
    public Constants.Levels level;
    public string levelName;
    public float distanceBetweenObjects;
    public int healthQtt;
    public int distance;
    public float gravity;
    public float speed;
}
