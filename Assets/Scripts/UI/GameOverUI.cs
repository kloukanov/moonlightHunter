using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _totalDaysSurvived;
    [SerializeField] private Button _playAgainButton;
    [SerializeField] private Button _mainMenuButton;

    private void Awake()
    {
        _playAgainButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.GameScene);
        });

        _mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }

    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        Hide();
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameOver())
        {
            Show();
            _totalDaysSurvived.text = "you survived for " + GameManager.Instance.GetTotalDaysSurvived().ToString() + " days";
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

}
