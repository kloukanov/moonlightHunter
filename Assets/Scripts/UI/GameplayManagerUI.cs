using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameplayManagerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _totalFood;
    [SerializeField] private TextMeshProUGUI _totalBones;


    private void Start()
    {
        GameManager.Instance.OnFoodAdded += GameManager_OnFoodAdded;
        GameManager.Instance.OnBonesAdded += GameManager_OnBonesAdded;
    }

    private void GameManager_OnBonesAdded(object sender, System.EventArgs e)
    {
        _totalBones.text = "Bones Gathered: " + GameManager.Instance.GetBonesCollected().ToString();
    }

    private void GameManager_OnFoodAdded(object sender, System.EventArgs e)
    {
        _totalFood.text = "Food Gathered: " + GameManager.Instance.GetFoodCollected().ToString();
    }
}
