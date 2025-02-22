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

public class LevelSelectPlatform : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI _levelName;
    [SerializeField] private TextMeshProUGUI _levelDistance;
    [SerializeField] private TextMeshProUGUI _maxDistance;
    [SerializeField] private LevelSO _levelSO;

    private Action<Constants.Levels> hoverAction;
    private Action clickAction;
    
   
    void Start()
    {
        _levelName.text = _levelSO.levelName;
        if (_levelSO.distance > 0)
        {
            _levelDistance.gameObject.SetActive(true);
            _levelDistance.text = _levelSO.distance + " Km";
        }
        _maxDistance.text = PlayerPrefs.GetFloat(Constants.record +_levelSO.level).ToString("0.00") + " Km";
    }

    public void SelectLevel(bool status)
    {
        if (status)
        {
            transform.DOLocalMoveY(340f, 0.2f);
        }
        else
        {
            transform.DOLocalMoveY(100f, 0.2f);
        }
    }

    public void SetActions(Action<Constants.Levels> actionOnSelect)
    {
        hoverAction = actionOnSelect;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverAction?.Invoke(_levelSO.level);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        hoverAction?.Invoke(_levelSO.level);
    }
}
