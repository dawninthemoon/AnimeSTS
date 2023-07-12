using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomHandler : MonoBehaviour, ISystemHandler {
    [SerializeField] private Transform _mapSceneParent = null;
    [SerializeField] private Transform _battleSceneParent = null;
    [SerializeField] private Transform _eventSceneParent = null;
    private Transform _currentRoomParent = null;

    public void ChangeRoomSetting() {

    }
}
