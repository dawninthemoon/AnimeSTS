using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomHandler : MonoBehaviour, ISystemHandler {
    [SerializeField] private GameObject _mapSceneParent = null;
    [SerializeField] private GameObject _battleSceneParent = null;
    [SerializeField] private GameObject _eventSceneParent = null;
    private GameObject _currentRoomParent = null;

    public void ChangeRoomSetting(EncounterType encounterType) {
        _mapSceneParent.SetActive(false);

        switch (encounterType) {
        case EncounterType.MONSTER:
        case EncounterType.ELITE:
        case EncounterType.CHEST:
            _battleSceneParent.SetActive(true);
            break;
        case EncounterType.EVENT:
            _eventSceneParent.SetActive(true);
            break;
        }
    }
}
