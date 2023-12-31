using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleRoom : RoomBase {
    [SerializeField] private Transform _playerPosition = null;
    [SerializeField] private Transform[] _enemyPositons = null;
    [SerializeField] private Button _endturnButton = null;
    private EntityBase _player;
    private EnemyHandler _enemyHandler;
    private CardHandler _cardHandler;
    public static EntityBase SelectedEnemy;
    private CommandExecuter _commandExecuter;
    private CombatUIHandler _combatUIHandler;
    private CostHandler _costHandler;

    public override void InitializeData(GameData data) {
        base.InitializeData(data);

        _costHandler = GetComponent<CostHandler>();
        _combatUIHandler = GetComponent<CombatUIHandler>();
        _commandExecuter = GetComponent<CommandExecuter>();
        _cardHandler = GetComponentInChildren<CardHandler>();
        _enemyHandler = GetComponentInChildren<EnemyHandler>();

        data.EnemyHandler = _enemyHandler;

        var playerPrefab = Resources.Load<EntityBase>("Entities/Characters/character_yuri");
        _player = Instantiate(playerPrefab, _playerPosition.position, Quaternion.identity, _playerPosition);

        _cardHandler.Initialize();
        _cardHandler.SetCallback(OnCardUsed, RedrawCardView);

        _endturnButton.onClick.AddListener(EndTurn);
    }

    private void InitializeBattle() {
        _cardHandler.InitializeBattle(_gameData);
        _enemyHandler.InitializeBattle(_enemyPositons);

        _combatUIHandler.InitializeUI(_player, _enemyHandler.EnemyList, _cardHandler.CardContainer, _gameData.Parser);
        _combatUIHandler.UpdateCardPileUI();
        _combatUIHandler.UpdateCostUI(_costHandler.CurrentCost, _costHandler.MaxCost);
        _costHandler.ChargeCost();
        _endturnButton.onClick.AddListener(EndTurn);
    }

    public override void OnEncounter() {
        InitializeBattle();
        _cardHandler.IsInteractive = true;
        _combatUIHandler.IsInteractive = true;
    }

    private void EndTurn() {
        _cardHandler.IsInteractive = false;
        _combatUIHandler.IsInteractive = false;

        _enemyHandler.ExecuteEnemyBehaviour(_player);
        
        _cardHandler.IsInteractive = true;
        _combatUIHandler.IsInteractive = true;
    }

    private void RedrawCardView(CardBase card) {
        string costText = _costHandler.GetRequireCost(card).ToString();
        card.ShowCard(costText, _gameData.Parser, _player);
    }

    private bool OnCardUsed(CardBase card) {
        if (!_costHandler.TryUseCard(card)) {
            return false;
        }

        CommandInfo[] commands = card.IsUpgraded ? card.Info.upgradeCommands : card.Info.baseCommands;
        var variableData = _gameData.Parser.ParseVariable(card.Info.variables, _player);

        _gameData.CurrentVariableData = variableData;

        _commandExecuter.ExecuteCard(commands, _gameData, _player, SelectedEnemy);
        _combatUIHandler.UpdateCardPileUI();
        _combatUIHandler.UpdateCostUI(_costHandler.CurrentCost, _costHandler.MaxCost);

        return true;
    }
}
