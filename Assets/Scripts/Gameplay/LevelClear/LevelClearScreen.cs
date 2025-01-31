using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelClearScreen : MonoBehaviour
{
    [SerializeField] private GameObject _congratulations;
    [SerializeField] private GameObject _fail;

    [SerializeField] private TextMeshProUGUI _currentDistance;
    [SerializeField] private TextMeshProUGUI _powerUpsQtt;
    [SerializeField] private TextMeshProUGUI _record;
    [SerializeField] private TextMeshProUGUI _level;
    
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private Button _homeButton;
    [SerializeField] private Button _replayButton;
    
    [SerializeField] private Image _screen;

    
    void Start()
    {
        _nextLevelButton.onClick.AddListener(OnNextLevel);
        _homeButton.onClick.AddListener(OnHome);
        _replayButton.onClick.AddListener(OnReplay);
    }

    public void InitPanel(float distance, int powerUps, bool completed)
    {
        distance = distance/20;
        _currentDistance.text = distance.ToString("0.00") + " Km";
        _powerUpsQtt.text = powerUps.ToString();
        float record = PlayerPrefs.GetFloat(Constants.record + GameManager.Instance.CurrentLevel, 0);
        if (distance > record)
        {
            PlayerPrefs.SetFloat(Constants.record + GameManager.Instance.CurrentLevel.level, distance);
            _record.text = distance.ToString("0.00") + " Km";
        }
        else
        {
            _record.text = record + " Km";
        }
        _level.text = GameManager.Instance.CurrentLevel.levelName;
        _congratulations.SetActive(completed);
        _nextLevelButton.gameObject.SetActive(GameManager.Instance.CurrentLevel.level != Constants.Levels.INFINITY);
        _fail.SetActive(!completed);
        Invoke(nameof(ShowPanel),1f);
    }

    private void ShowPanel()
    {
        _screen.GetComponent<CanvasGroup>().DOFade(1, 1f);
        _screen.gameObject.gameObject.SetActive(true);
    }

    private void OnNextLevel()
    {
        PlayerPrefs.SetInt(Constants.current_level,  (int)GameManager.Instance.CurrentLevel.level + 1);
        OnReplay();
    }

    private void OnReplay()
    {
        SceneManager.LoadScene("GameplayScene");
    }

    private void OnHome()
    {
        SceneManager.LoadScene("Menu");
    }
}
