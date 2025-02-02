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
    [SerializeField] private AudioClip _playSound;
    [SerializeField] private AudioClip _selectSound;
    [SerializeField] private AudioClip _menuMusic;
    private int _selectedLevel = 0;

    void Start()
    {
        _selectedLevel = PlayerPrefs.GetInt(Constants.current_level, 0);
        _levelSelectPlatforms[_selectedLevel].SelectLevel(true);
        foreach (var level in _levelSelectPlatforms)
        {
            level.SetActions(OnSelectLevel);
        }
        SoundManager.PlayMusic(_menuMusic, true);
        _playButton.onClick.AddListener(OnClickPlay);
    }

    private void OnSelectLevel(int level)
    {
        _selectedLevel = level;
        SoundManager.PlayEffect(_selectSound);
        for (int i = 0; i < _levelSelectPlatforms.Count; i++)
        {
            _levelSelectPlatforms[i].SelectLevel(i == level);
        }
    }

    private void OnClickPlay()
    {
        PlayerPrefs.SetInt(Constants.current_level, _selectedLevel);
        SoundManager.PlayEffect(_playSound);
        SceneManager.LoadScene("GameplayScene");
    }

}
