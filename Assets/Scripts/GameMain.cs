using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameMain : MonoBehaviour {
    [SerializeField] private GameMap _gameMap = null;
    [SerializeField] private RoomHandler _roomHandler = null;
    private GameData _gameData;

    private void Awake() {
        InitializeData();
        _roomHandler.InitializeData(_gameData);
    }

    private void Start() {
        _gameMap.GenerateMap(OnRoomMoveRequested);
        _roomHandler.SetOnRoomExit(_gameMap.OnRoomCleared);

        _gameMap.SetRoomInteractive();
    }

    private void InitializeData() {
        TxtReader reader = new TxtReader();
        string gameMetaData = reader.ReadFromFile(Application.dataPath + "/Resources/GameData.txt");

        GameVariableContainer container = new GameVariableContainer();
        CommandDataParser parser = new CommandDataParser(container);
        
        CardDeck cardDeck = new CardDeck();
        cardDeck.InitializeDeck();

        parser.ParseAndAdd(gameMetaData);
        _gameData = new GameData(parser, cardDeck);
    }

    public void OnRoomMoveRequested(EncounterMarker target) {
        if (!_gameMap.CanMoveTo(target)) return;

        _gameMap.OnRoomChanged(target);
        _roomHandler.StartEnterRoom(target.EncounterType);
    }
}
