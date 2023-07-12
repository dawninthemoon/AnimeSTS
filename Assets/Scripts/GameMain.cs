using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameMain : MonoBehaviour {
    [SerializeField] private GameMap _gameMap = null;
    private Encounter _currentRoom;

    private void Awake() {
        var prefab = Resources.Load<Encounter>("Map/enc_neow");
        _currentRoom = Instantiate(prefab, Vector3.zero, Quaternion.identity);
    }

    private void Start() {
        _currentRoom = _gameMap.GenerateMap();
        StartHighlightRoom();
    }
    
    public void StartHighlightRoom() {
        foreach (Encounter room in _currentRoom.AdjustSet) {
            StartCoroutine(HighlightRoom(room));
        }
    }

    private IEnumerator HighlightRoom(Encounter room) {
        Transform t = room.transform;
        float minScale = 0.7f;
        float maxScale = 1.2f;

        float sign = 1f;
        Vector3 amount = new Vector3(0.01f, 0.01f);
        WaitForSeconds waitDelay = new WaitForSeconds(0.1f);

        t.localScale = new Vector3(minScale, minScale);
        while (true) {
            t.localScale += amount * sign;
            if (t.localScale.x > maxScale || t.localScale.x < minScale) {
                sign = -sign;
                if (sign > 0f)
                    yield return waitDelay;
            }

            yield return null;
        }
    }
    
}
