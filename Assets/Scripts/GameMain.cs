using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameMain : MonoBehaviour {
    [SerializeField] private GameMap _gameMap = null;

    private void Start() {
        _gameMap.GenerateMap();
        _gameMap.SetRoomInteractive();
    }
}
