using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class LevelSelectPlatform : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private TextMeshProUGUI _levelName;
    [SerializeField] private TextMeshProUGUI _levelDistance;
    [SerializeField] private TextMeshProUGUI _maxDistance;
    [SerializeField] private LevelSO _levelSO;

    private Action<int> action;
    
   
    void Start()
    {
        _levelName.text = _levelSO.levelName;
        if (_levelSO.distance > 0)
        {
            _levelDistance.gameObject.SetActive(true);
            _levelDistance.text = _levelSO.distance/20 + " Km";
        }
        _maxDistance.text = PlayerPrefs.GetFloat(Constants.record +_levelSO.level).ToString("0.00") + " Km";
    }

    public void SelectLevel(bool status)
    {
        if(status) 
            transform.DOLocalMoveY(170f, 0.2f);
        else
        {
            transform.DOLocalMoveY(0f, 0.2f);
        }
    }

    public void SetActionOnSelect(Action<int> actionOnSelect)
    {
        action = actionOnSelect;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        action?.Invoke((int)_levelSO.level);
    }
    
}
