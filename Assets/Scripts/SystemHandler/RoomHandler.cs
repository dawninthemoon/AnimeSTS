using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RoomHandler : MonoBehaviour {
    [SerializeField] private GameObject _mapSceneParent = null;
    private GameObject _currentRoomParent;
    public delegate void OnRoomExit();
    private OnRoomExit _requestedRoomExitCallback;

    private BattleRoom _battleRoomEncounter;
    private ShopRoom _shopRoomEncounter;
    private ChestRoom _chestRoomEncounter;
    private EventRoom _eventRoomEncounter;

    [Inject]
    private void Initialize(RoomBase[] encounters) {
        foreach (RoomBase encounter in encounters) {
            if (encounter is BattleRoom) {
                _battleRoomEncounter = encounter as BattleRoom;
            }
            else if (encounter is EventRoom) {
                _eventRoomEncounter = encounter as EventRoom;
            }
            else if (encounter is ShopRoom) {
                _shopRoomEncounter = encounter as ShopRoom;
            }
            else if (encounter is ChestRoom) {
                _chestRoomEncounter = encounter as ChestRoom;
            }
        }
    }

    private void Start() {
        _currentRoomParent = _mapSceneParent;
    }

    public void SetOnRoomExit(OnRoomExit callback) {
        _requestedRoomExitCallback = callback;
    }

    public void StartEnterRoom(EncounterType encounterType) {
        switch (encounterType) {
        case EncounterType.MONSTER:
            EnterRoom(_battleRoomEncounter);
            break;
        case EncounterType.ELITE:
            EnterRoom(_battleRoomEncounter);
            break;
        case EncounterType.SHOP:
            EnterRoom(_shopRoomEncounter);
            break;
        case EncounterType.CHEST:
            EnterRoom(_chestRoomEncounter);
            break;
        case EncounterType.EVENT:
            EnterRoom(_eventRoomEncounter);
            break;
        }
    }

    private void EnterRoom(RoomBase room) {
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
