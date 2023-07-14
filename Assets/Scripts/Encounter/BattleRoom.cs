using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRoom : RoomBase {
    [SerializeField] private Transform _playerPosition = null;
    private EntityBase _player;
    private EntityBase _enemy;
    private CardHandler _cardHandler;
    private CardContainer _cardDeck;

    private void Awake() {
        var playerPrefab = Resources.Load<EntityBase>("Characters/character_yuri");
        _player = Instantiate(playerPrefab, _playerPosition.position, Quaternion.identity, _playerPosition);
        _cardHandler = GetComponentInChildren<CardHandler>();
    }

    private void Start() {

    }

    public override void OnEncounter() {
        
    }
}
