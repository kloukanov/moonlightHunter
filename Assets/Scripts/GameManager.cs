using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    public event EventHandler OnFoodAdded;
    public event EventHandler OnBonesAdded;

    private static int _foodCollected = 0;
    private static int _bonesCollected = 0;

    private float _gamePlayingTimer;
    private float _gamePlayingTimerMax = 60f;


    private void Awake()
    {
        Instance = this;
        _gamePlayingTimer = _gamePlayingTimerMax;
    }

    private void Update()
    {
        _gamePlayingTimer -= Time.deltaTime;
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
