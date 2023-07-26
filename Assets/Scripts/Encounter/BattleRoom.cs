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
    public static EntityBase SelectedEnemy;
    private CommandExecuter _commandExecuter;

    private void Awake() {
        var playerPrefab = Resources.Load<EntityBase>("Entities/Characters/character_yuri");
        _player = Instantiate(playerPrefab, _playerPosition.position, Quaternion.identity, _playerPosition);

        _commandExecuter = GetComponent<CommandExecuter>();
        _cardHandler = GetComponentInChildren<CardHandler>();
    }

    private void Start() {
        _cardHandler.SetCallback(OnCardUsed, RedrawCardView);
        _cardHandler.InitializeBattle(_gameData);
    }

    public override void OnEncounter() {
        var enemyPrefab = Resources.Load<EntityBase>("Entities/Enemies/enemy_humTank");
        _enemy = Instantiate(enemyPrefab, _enemyPositon.position, Quaternion.identity, _enemyPositon);
    }

    private void RedrawCardView(CardBase card) {
        card.ShowCard(_gameData.Parser, _player);
    }

    private void OnCardUsed(CardInfo card) {
        CommandInfo[] commands = card.isUpgraded ? card.upgradeCommands : card.baseCommands;
        var variableData = _gameData.Parser.ParseVariable(card.variables, _player);

        _gameData.CurrentVariableData = variableData; 

        _commandExecuter.ExecuteCard(commands, _gameData);
    }
}
