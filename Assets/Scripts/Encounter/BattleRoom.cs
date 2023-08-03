using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRoom : RoomBase {
    [SerializeField] private Transform _playerPosition = null;
    [SerializeField] private Transform _enemyPositon = null;
    private EntityBase _player;
    private List<EntityBase> _enemyList;
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

        _enemyList = new List<EntityBase>();

        var playerPrefab = Resources.Load<EntityBase>("Entities/Characters/character_yuri");
        _player = Instantiate(playerPrefab, _playerPosition.position, Quaternion.identity, _playerPosition);

        _cardHandler.Initialize();
        _cardHandler.SetCallback(OnCardUsed, RedrawCardView);
    }

    private void InitializeBattle() {
        _cardHandler.InitializeBattle(_gameData);
        _combatUIHandler.InitializeUI(_player, _enemyList);
        _combatUIHandler.UpdateCardPileUI(_cardHandler.CardContainer);
        _combatUIHandler.UpdateCostUI(_costHandler.CurrentCost, _costHandler.MaxCost);
        _costHandler.ChargeCost();
    }

    public override void OnEncounter() {
        var enemyPrefab = Resources.Load<EntityBase>("Entities/Enemies/enemy_humTank");
        _enemyList.Add(Instantiate(enemyPrefab, _enemyPositon.position, Quaternion.identity, _enemyPositon));

        InitializeBattle();
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
        _gameData.CurrentEnemyList = _enemyList;

        _commandExecuter.ExecuteCard(commands, _gameData, _player, SelectedEnemy);
        _combatUIHandler.UpdateCardPileUI(_cardHandler.CardContainer);
        _combatUIHandler.UpdateCostUI(_costHandler.CurrentCost, _costHandler.MaxCost);

        return true;
    }
}
