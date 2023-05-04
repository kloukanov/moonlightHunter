using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, IHasProgress
{

    [SerializeField] private EntitySO _entitySO;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    private string _name;
    private float _maxHealth;
    private float _health;
    private float _attackStrength;
    private List<CollectableObjectSO> _collectableObjectSOList;

    private void Awake()
    {
        _name = _entitySO.objectName;
        _maxHealth = _entitySO.maxHealth;
        _health = _entitySO.health;
        _attackStrength = _entitySO.attackStrength;
        _collectableObjectSOList = _entitySO.collectableObjectSOList;
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void SpawnLoot()
    {
        foreach(CollectableObjectSO collectableObjectSO in _collectableObjectSOList)
        {
            Debug.Log("dropping loot: " + _collectableObjectSOList[0].objectName);
            CollectableObject.SpawnCollectableObject(collectableObjectSO, this.transform);
        }
    }

    public string GetName()
    {
        return _name;
    }

    public void AddHealth(float healthValue)
    {
        if (_health + healthValue <= _maxHealth)
        {
            _health += healthValue;
        }
        else
        {
            _health = _maxHealth;
        }
    }

    public void DealDamage(Entity other)
    {
        other.DeductHealth(_attackStrength);
    }

    public void DeductHealth(float deductHealthValue)
    {
        Debug.Log("dealing damage of " + deductHealthValue + " from health of " + _health);
        _health -= deductHealthValue;
        Debug.Log("health after damage " + _health);

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = _health / _maxHealth
        });

        if(_health <= 0)
        {
            SpawnLoot();
            DestroySelf();
        }
    }
}
