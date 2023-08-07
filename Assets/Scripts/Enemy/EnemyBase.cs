using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : EntityBase {
    [SerializeField] private EnemyIntent[] _intents;
    public EnemyIntent[] Intents {
        get { return _intents; }
    }

    private void Start() {
        int intentCounts = _intents.Length;
        for (int i = 0; i < intentCounts; ++i) {
            int minAttack = _intents[i].minAttack;
            int maxAttack = _intents[i].maxAttack;
            int minBlock = _intents[i].minBlock;
            int maxBlock = _intents[i].maxBlock;

            _intents[i].attackAmount = Random.Range(minAttack, maxAttack);
            _intents[i].blockAmount = Random.Range(minBlock, maxBlock);
        }
    }

    public void Behaviour(EntityBase player) {
        int intentCounts = _intents.Length;
        EnemyIntent behaviour = _intents[Random.Range(0, intentCounts)];

        if (behaviour.attackAmount > 0) {
            BattleCommand.Attack attackCommand = new BattleCommand.Attack();
            StartCoroutine(attackCommand.Execute(this, player, behaviour.attackAmount));
        }
        if (behaviour.blockAmount > 0) {
            BattleCommand.Attack blockCommand = new BattleCommand.Attack();
            StartCoroutine(blockCommand.Execute(this, player, behaviour.blockAmount));
        }
    }
}
