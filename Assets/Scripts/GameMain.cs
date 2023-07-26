using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameMain : MonoBehaviour {
    [SerializeField] private GameMap _gameMap = null;
    [SerializeField] private RoomHandler _roomHandler = null;

    private void Start() {
        InitializeData();
        _gameMap.GenerateMap(OnRoomMoveRequested);
        _gameMap.SetRoomInteractive();

        _roomHandler.SetOnRoomExit(_gameMap.OnRoomCleared);
    }

    private void InitializeData() {
        GameVariableContainer container = new GameVariableContainer();
        CommandDataParser parser = new CommandDataParser(container);
        GameData gameData = new GameData(parser);
        
    }

    public void OnRoomMoveRequested(EncounterMarker target) {
        if (!_gameMap.CanMoveTo(target)) return;

        _gameMap.OnRoomChanged(target);
        _roomHandler.StartEnterRoom(target.EncounterType);
    }
}
