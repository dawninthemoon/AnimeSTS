using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBase : MonoBehaviour {
    [SerializeField] private int _health = 10;
    private int _block;
    private int _strength;
    private int _dexterity;
    private int _weak;
    private int _vulnerable;
}
