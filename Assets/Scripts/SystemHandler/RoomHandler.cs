using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RoomHandler : MonoBehaviour {
    [SerializeField] private GameObject _mapSceneParent = null;
    private GameObject _currentRoomParent;
    public delegate void OnRoomExit();
    private OnRoomExit _requestedRoomExitCallback;

    [SerializeField] private BattleRoom _battleRoomEncounter = null;
    [SerializeField] private ShopRoom _shopRoomEncounter = null;
    [SerializeField] private ChestRoom _chestRoomEncounter = null;
    [SerializeField] private EventRoom _eventRoomEncounter = null;

    private void Start() {
        _currentRoomParent = _mapSceneParent;
    }

    public void InitializeData(GameData data) {
        _battleRoomEncounter.InitializeData(data);
        _shopRoomEncounter.InitializeData(data);
        _chestRoomEncounter.InitializeData(data);
        _eventRoomEncounter.InitializeData(data);
    }

    public void SetOnRoomExit(OnRoomExit callback) {
        _requestedRoomExitCallback = callback;
    }

    public void StartEnterRoom(EncounterType encounterType) {
        RoomBase targetRoom = null;
        switch (encounterType) {
        case EncounterType.MONSTER:
            targetRoom = _battleRoomEncounter;
            break;
        case EncounterType.ELITE:
            targetRoom = _battleRoomEncounter;
            break;
        case EncounterType.SHOP:
            targetRoom = _shopRoomEncounter;
            break;
        case EncounterType.CHEST:
            targetRoom = _chestRoomEncounter;
            break;
        case EncounterType.EVENT:
            targetRoom = _eventRoomEncounter;
            break;
        }
        EnterRoom(targetRoom);
    }

    private void EnterRoom(RoomBase room) {
        if (room == null) {
            Debug.LogError("Room Not Exists");
            return;
        }
        ChangeRoomSetting(room.transform.parent.gameObject);
        room.OnEncounter();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Z))
            ExitRoom();
    }

    public void ExitRoom() {
        ChangeRoomSetting(_mapSceneParent);
        _requestedRoomExitCallback.Invoke();
    }

    private void ChangeRoomSetting(GameObject target) {
        _currentRoomParent.SetActive(false);
        _currentRoomParent = target;
        target.SetActive(true);
    }
}
