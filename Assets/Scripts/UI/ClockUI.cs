using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockUI : MonoBehaviour
{
    [SerializeField] private GameObject _dayTimeUI;
    [SerializeField] private GameObject _nightTimeUI;

    [SerializeField] private Image _dayTimerImage;
    [SerializeField] private Image _nightTimerImage;


    private void Awake()
    {
        _dayTimeUI.SetActive(true);
        _nightTimeUI.SetActive(false);
    }

    private void Start()
    {
        GameManager.Instance.OnTimerFinished += GameManager_OnTimerFinished;
    }

    private void GameManager_OnTimerFinished(object sender, System.EventArgs e)
    {
        if (_dayTimeUI.activeInHierarchy)
        {
            _nightTimeUI.SetActive(true);
            _dayTimeUI.SetActive(false);
        }
        else
        {
            _dayTimeUI.SetActive(true);
            _nightTimeUI.SetActive(false);
        }

        _nightTimerImage.fillAmount = 0f;
        _dayTimerImage.fillAmount = 0f;

    }

    private void Update()
    {
        if (_dayTimeUI.activeInHierarchy)
        {
            _dayTimerImage.fillAmount = GameManager.Instance.GetGamePlayingTimerNormalized(); 
        }
        else
        {
            _nightTimerImage.fillAmount = GameManager.Instance.GetGamePlayingTimerNormalized();  
        }
    }
}