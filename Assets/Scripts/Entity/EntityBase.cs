using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EntityInfo {
    public int minHealth;
    public int maxHealth;
    public int health;
    public int block;
    public Dictionary<string, int> effectMap;
}

public class EntityBase : MonoBehaviour {
    [SerializeField] private EntityInfo _info;

    private void Awake() {
        _info.effectMap = new Dictionary<string, int>();
        _info.effectMap.Add("dexterity", 0);
        _info.effectMap.Add("strength", 0);
        _info.effectMap.Add("vulnerable", 0);
        _info.effectMap.Add("weak", 0);
        _info.effectMap.Add("frail", 0);
    }

    public int GetEffectAmount(string effectName) {
        return _info.effectMap[effectName];
    }

    public void TakeDamage(int amount) {
        if (_info.effectMap["vulnerable"] > 0) {
            amount = Mathf.FloorToInt(amount * 1.5f);
        }
        Debug.Log(gameObject.name + " has recieved " + amount.ToString() + " damage");
    }

    public void GainBlock(int amount) {
        Debug.Log("Current Block: " + amount.ToString());
        _info.block += amount;
    }

    public void ChangeEffectValue(string key, int amount) {
        _info.effectMap[key] = amount;
    }

    private void OnMouseOver() {
        BattleRoom.SelectedEnemy = this;
    }

    private void OnMouseExit() {
        BattleRoom.SelectedEnemy = null;
    }
}
