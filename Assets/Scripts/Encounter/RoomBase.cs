using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomBase : MonoBehaviour {
    protected GameData _gameData;
    public void InitializeData(GameData data) {
        _gameData = data;
    }
    public abstract void OnEncounter();
}
