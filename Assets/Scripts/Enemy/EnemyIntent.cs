using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EffectConfig {
    public string effectName;
    public int amount;
}

[System.Serializable]
public struct EnemyIntent {
    public string intentName;
    public string behaviourName;
    public int minAttack;
    public int maxAttack;
    public int minBlock;
    public int maxBlock;
    public EffectConfig[] effectConfig;
    public bool certainly;
    [HideInInspector] public int attackAmount;
    [HideInInspector] public int blockAmount;
}
