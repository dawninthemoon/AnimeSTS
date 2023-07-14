using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RieslingUtils;

public class BattleRoom : RoomBase {
    [SerializeField] private Transform _playerPosition = null;
    [SerializeField] private Transform _enemyPositon = null;
    private EntityBase _player;
    private EntityBase _enemy;
    private CardHandler _cardHandler;
    private CardContainer _cardDeck;
    public static EntityBase SelectedEnemy;

    private void Awake() {
        var playerPrefab = Resources.Load<EntityBase>("Entities/Characters/character_yuri");
        _player = Instantiate(playerPrefab, _playerPosition.position, Quaternion.identity, _playerPosition);
        _cardHandler = GetComponentInChildren<CardHandler>();
    }

    private void Start() {

    }

    public override void OnEncounter() {
        var enemyPrefab = Resources.Load<EntityBase>("Entities/Enemies/enemy_humTank");
        _enemy = Instantiate(enemyPrefab, _enemyPositon.position, Quaternion.identity, _enemyPositon);
    }
}
