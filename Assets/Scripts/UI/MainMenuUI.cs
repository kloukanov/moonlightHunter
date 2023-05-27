using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{

    [SerializeField] private Button _playButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _quitButton;

    private void Awake()
    {
        _playButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.GameScene);
        });

        _optionsButton.onClick.AddListener(() =>
        {
            // TODO: option menu
        });

        _quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        Time.timeScale = 1f;

    }


}
