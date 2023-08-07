using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour {
    public List<EnemyBase> EnemyList {
        get;
        private set;
    } = new List<EnemyBase>();

    public void InitializeBattle(Transform enemyPosition) {
        var enemyPrefab = Resources.Load<EnemyBase>("Entities/Enemies/enemy_humTank");
        EnemyList.Add(Instantiate(enemyPrefab, enemyPosition.position, Quaternion.identity, enemyPosition));
    }

    public void ExecuteEnemyBehaviour(EntityBase player) {
        foreach (EnemyBase enemy in EnemyList) {
            enemy.Behaviour(player);
        }
    }

    public EnemyBase GetRandomEnemy() {
        if (EnemyList.Count < 0) {
            return null;
        }
        int index = Random.Range(0, EnemyList.Count);
        return EnemyList[index];
    }
}
