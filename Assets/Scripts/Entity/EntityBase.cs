using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EntityInfo {
    public int maxHealth;
    public int health;
    public int block;
    public int strength;
    public int dexterity;
    public int weak;
    public int vulnerable;
}

public class EntityBase : MonoBehaviour {
    [SerializeField] private EntityInfo _info;
}
