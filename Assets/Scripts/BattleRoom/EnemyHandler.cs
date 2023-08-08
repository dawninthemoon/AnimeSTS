using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour {
    [SerializeField] private EnemyEncounterConfig _encounterConfig = null;

    public List<EnemyBase> EnemyList {
        get;
        private set;
    } = new List<EnemyBase>();

    public void InitializeBattle(Transform[] enemyPositions) {
        var enemyPrefab = Resources.Load<EnemyBase>("Entities/Enemies/enemy_humTank");

        var party = _encounterConfig.GetRandomParty();
        for (int i = 0; i < party.Length; ++i) {
            EnemyBase enemy = Instantiate(enemyPrefab, enemyPositions[i].position, Quaternion.identity, enemyPositions[i]);
            EnemyList.Add(enemy);
        }
    }

    public void ExecuteEnemyBehaviour(EntityBase player) {
        StartCoroutine(StartEnemyBehaviour(player));
    }

    public EnemyBase GetRandomEnemy() {
        if (EnemyList.Count < 0) {
            return null;
        }
        int index = Random.Range(0, EnemyList.Count);
        return EnemyList[index];
    }

    private IEnumerator StartEnemyBehaviour(EntityBase player) {
        WaitForSeconds delay = new WaitForSeconds(0.5f);
        foreach (EnemyBase enemy in EnemyList) {
            enemy.Behaviour(player);
            yield return delay;
        }
    }
}
