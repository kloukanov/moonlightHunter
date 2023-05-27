using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    public event EventHandler OnStateChanged;
    public event EventHandler OnFoodAdded;
    public event EventHandler OnBonesAdded;
    public event EventHandler OnTimerFinished;

    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    private State _state;

    private bool _isWerewolf = false; 

    private static int _foodCollected = 0;
    private static int _bonesCollected = 0;
    private static int _daysSurvived = 0;

    private float _gamePlayingTimer;
    private float _gamePlayingTimerMax = 10f;


    private void Awake()
    {
        Time.timeScale = 1f;
        Instance = this;
        _gamePlayingTimer = _gamePlayingTimerMax;
        _state = State.GamePlaying;
    }

    private void Start()
    {
        Player.Instance.OnPlayerDead += Player_OnPlayerDead;
    }

    private void Player_OnPlayerDead(object sender, EventArgs e)
    {
        _state = State.GameOver;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        switch (_state)
        {
            case State.GamePlaying:
                _gamePlayingTimer -= Time.deltaTime;

                if (_gamePlayingTimer <= 0)
                {
                    ++_daysSurvived;
                    // turn into werewolf
                    if (_isWerewolf == false)
                    {
                        _isWerewolf = true;
                    }
                    OnTimerFinished?.Invoke(this, EventArgs.Empty);
                }

                break;
            case State.GameOver:
                Time.timeScale = 0f;
                break;
        }
    }

    public bool IsGameOver()
    {
        return _state == State.GameOver;
    }


    public float GetGamePlayingTimerNormalized()
    {
        if(_gamePlayingTimer <= 0)
        { 
            _gamePlayingTimer = _gamePlayingTimerMax;
        }

        return 1 - _gamePlayingTimer / _gamePlayingTimerMax;
    }

    public void AddFood()
    {
        ++_foodCollected;
        OnFoodAdded?.Invoke(this, EventArgs.Empty);
    }

    public void AddBones()
    {
        ++_bonesCollected;
        OnBonesAdded?.Invoke(this, EventArgs.Empty);
    }

    public int GetFoodCollected()
    {
        return _foodCollected;
    }

    public int GetBonesCollected()
    {
        return _bonesCollected;
    }

    public int GetTotalDaysSurvived()
    {
        return _daysSurvived;
    }
}
