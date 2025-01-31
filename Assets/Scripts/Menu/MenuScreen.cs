using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScreen : MonoBehaviour
{
    [SerializeField] private List<LevelSelectPlatform> _levelSelectPlatforms = new List<LevelSelectPlatform>();
    [SerializeField] private Button _playButton;

    private int _selectedLevel = 0;

    void Start()
    {
        _selectedLevel = PlayerPrefs.GetInt(Constants.current_level, 0);
        _levelSelectPlatforms[_selectedLevel].SelectLevel(true);
        foreach (var level in _levelSelectPlatforms)
        {
            level.SetActionOnSelect(OnSelectLevel);
        }

        _playButton.onClick.AddListener(OnClickPlay);
    }

    private void OnSelectLevel(int level)
    {
        _selectedLevel = level;
        for (int i = 0; i < _levelSelectPlatforms.Count; i++)
        {
            _levelSelectPlatforms[i].SelectLevel(i == level);
        }
    }

    private void OnClickPlay()
    {
        PlayerPrefs.SetInt(Constants.current_level, _selectedLevel);
        SceneManager.LoadScene("GameplayScene");
    }

}
