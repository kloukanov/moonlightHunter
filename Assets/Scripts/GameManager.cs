using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    public event EventHandler OnFoodAdded;
    public event EventHandler OnBonesAdded;
    public event EventHandler OnTimerFinished;

    private bool _isWerewolf = false; 

    private static int _foodCollected = 0;
    private static int _bonesCollected = 0;

    private float _gamePlayingTimer;
    private float _gamePlayingTimerMax = 10f;


    private void Awake()
    {
        Instance = this;
        _gamePlayingTimer = _gamePlayingTimerMax;
    }

    private void Update()
    {
        _gamePlayingTimer -= Time.deltaTime;
        if(_isWerewolf == false && _gamePlayingTimer <= 0)
        {
            // turn into werewolf
            _isWerewolf = true;
            OnTimerFinished?.Invoke(this, EventArgs.Empty);
        }
    }


    public float GetGamePlayingTimerNormalized()
    {
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
}
