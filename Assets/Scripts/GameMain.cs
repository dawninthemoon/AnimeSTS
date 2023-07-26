using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameMain : MonoBehaviour {
    [SerializeField] private GameMap _gameMap = null;
    [SerializeField] private RoomHandler _roomHandler = null;
    private GameData _gameData;

    private void Start() {
        InitializeData();
        _gameMap.GenerateMap(OnRoomMoveRequested);
        _gameMap.SetRoomInteractive();

        _roomHandler.SetOnRoomExit(_gameMap.OnRoomCleared);
    }

    private void InitializeData() {
        TxtReader reader = new TxtReader();
        string gameMetaData = reader.ReadFromFile(Application.dataPath + "/Resources/GameData.txt");

        GameVariableContainer container = new GameVariableContainer();
        CommandDataParser parser = new CommandDataParser(container);

        parser.ParseAndAdd(gameMetaData);
        _gameData = new GameData(parser);
    }

    public void OnRoomMoveRequested(EncounterMarker target) {
        if (!_gameMap.CanMoveTo(target)) return;

        _gameMap.OnRoomChanged(target);
        _roomHandler.StartEnterRoom(target.EncounterType, _gameData);
    }
}
