using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class EntitySO : ScriptableObject
{
    public string objectName;
    public float maxHealth;
    public float health;
    public float attackStrenght;
}
