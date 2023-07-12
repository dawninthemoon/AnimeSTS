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
    private void Initialize(IEncounterable[] encounters) {
        foreach (IEncounterable encounter in encounters) {
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

    public void EnterRoom(EncounterType encounterType) {
        switch (encounterType) {
        case EncounterType.MONSTER:
            ChangeRoomSetting(_battleRoomEncounter.transform.parent.gameObject);
            break;
        case EncounterType.ELITE:
            ChangeRoomSetting(_battleRoomEncounter.transform.parent.gameObject);
            break;
        case EncounterType.SHOP:
            ChangeRoomSetting(_shopRoomEncounter.transform.parent.gameObject);
            break;
        case EncounterType.CHEST:
            ChangeRoomSetting(_chestRoomEncounter.transform.parent.gameObject);
            break;
        case EncounterType.EVENT:
            ChangeRoomSetting(_eventRoomEncounter.transform.parent.gameObject);
            break;
        }
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
