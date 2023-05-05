using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameplayManagerUI : MonoBehaviour, IHasProgress
{
    [SerializeField] private TextMeshProUGUI _totalFood;
    [SerializeField] private TextMeshProUGUI _totalBones;
    private float _totalMaxFood = 10f;

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    private void Start()
    {
        GameManager.Instance.OnFoodAdded += GameManager_OnFoodAdded;
        GameManager.Instance.OnBonesAdded += GameManager_OnBonesAdded;
    }

    private void GameManager_OnBonesAdded(object sender, System.EventArgs e)
    {
        _totalBones.text = GameManager.Instance.GetBonesCollected().ToString();
    }

    private void GameManager_OnFoodAdded(object sender, System.EventArgs e)
    {
        _totalFood.text = GameManager.Instance.GetFoodCollected().ToString();
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = GameManager.Instance.GetFoodCollected() / _totalMaxFood
        });
    }
}
