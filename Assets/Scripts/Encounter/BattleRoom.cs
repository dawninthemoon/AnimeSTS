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
    private CommandDataParser _variableParser;
    public static EntityBase SelectedEnemy;

    private void Awake() {
        var playerPrefab = Resources.Load<EntityBase>("Entities/Characters/character_yuri");
        _player = Instantiate(playerPrefab, _playerPosition.position, Quaternion.identity, _playerPosition);
        _cardHandler = GetComponentInChildren<CardHandler>();

        _cardHandler.SetCardUseCallback(OnCardUsed);
    }

    private void Start() {

    }

    public override void OnEncounter() {
        var enemyPrefab = Resources.Load<EntityBase>("Entities/Enemies/enemy_humTank");
        _enemy = Instantiate(enemyPrefab, _enemyPositon.position, Quaternion.identity, _enemyPositon);
    }

    private void OnCardUsed(CardInfo card) {
        CommandInfo[] effects = card.isUpgraded ? card.upgradeCommands : card.baseCommands;
        var variableData = _variableParser.ParseVariable(_player, card.variables);
        /*foreach (CommandInfo effect in effects) {
            ApplyEffect(effect.type, variableData[effect.amount]);
        }*/
    }
/*
    private void ApplyEffect(CommandInfo.Type type, int amount) {
        switch (type) {
        case CommandInfo.Type.BLOCK:
            _player.GainBlock(amount);
            break;
        case CommandInfo.Type.ATTACK:
            _enemy.TakeDamage(amount);
            break;
        }
    }*/
}
