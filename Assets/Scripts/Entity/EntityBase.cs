using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EntityInfo {
    public int minHealth;
    public int maxHealth;
    public Dictionary<string, int> effectMap;
}

public class EntityBase : ObserverSubject {
    [SerializeField] private EntityInfo _info;
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    public int Block { get; private set; }

    private void Awake() {
        _info.effectMap = new Dictionary<string, int>();
        _info.effectMap.Add("dexterity", 0);
        _info.effectMap.Add("strength", 0);
        _info.effectMap.Add("vulnerable", 0);
        _info.effectMap.Add("weak", 0);
        _info.effectMap.Add("frail", 0);
        
        CurrentHealth = MaxHealth = Random.Range(_info.minHealth, _info.maxHealth);
    }

    public int GetEffectAmount(string effectName) {
        return _info.effectMap[effectName];
    }

    public void TakeDamage(int amount) {
        if (_info.effectMap["vulnerable"] > 0) {
            amount = Mathf.FloorToInt(amount * 1.5f);
        }
        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
        Notify();
    }

    public void GainBlock(int amount) {
        Block += amount;
        Notify();
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
