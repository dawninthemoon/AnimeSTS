using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEncounterConfig : MonoBehaviour {
    [System.Serializable]
    public struct Party {
        public EnemyBase.EnemyType[] enemies;
    }

    [SerializeField] private Party[] _enemyParties;

    public EnemyBase.EnemyType[] GetRandomParty() {
        int selectedIndex = Random.Range(0, _enemyParties.Length);
        return _enemyParties[selectedIndex].enemies;
    }
}
