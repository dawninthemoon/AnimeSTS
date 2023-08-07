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
    private Animator _animator;
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    public int Block { 
        get { 
            return _info.effectMap["block"]; 
        }
        set {
            _info.effectMap["block"] = value;
        }
    }

    private void Awake() {
        _animator = GetComponent<Animator>();

        _info.effectMap = new Dictionary<string, int>();
        _info.effectMap.Add("dexterity", 0);
        _info.effectMap.Add("strength", 0);
        _info.effectMap.Add("vulnerable", 0);
        _info.effectMap.Add("weak", 0);
        _info.effectMap.Add("frail", 0);
        _info.effectMap.Add("block", 0);
        
        CurrentHealth = MaxHealth = Random.Range(_info.minHealth, _info.maxHealth);
    }

    public int GetEffectAmount(string effectName) {
        int value;
        if (!_info.effectMap.TryGetValue(effectName, out value)) {
            value= 0;
            _info.effectMap.Add(effectName, 0);
        }
        return value;
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

    public void ChangeEffectValue(string effectName, int amount) {
        if (!_info.effectMap.TryGetValue(effectName, out int value)) {
            _info.effectMap.Add(effectName, amount);
        }
        else {
            _info.effectMap[effectName] = amount;
        }
    }

    public void AddEffectValue(string effectName, int amount) {
        int prevValue = GetEffectAmount(effectName);
        ChangeEffectValue(effectName, prevValue + amount);
    }

    public void StartAnimation(string animationName) {
        _animator.Play(animationName);
    }

    private void OnMouseOver() {
        BattleRoom.SelectedEnemy = this;
    }

    private void OnMouseExit() {
        BattleRoom.SelectedEnemy = null;
    }
}
