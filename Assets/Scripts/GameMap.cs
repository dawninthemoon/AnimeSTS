using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameMap : MonoBehaviour {
    public static readonly int Width = 7;
    public static readonly int Height = 15;
    private CustomGrid<EncounterMarker> _mapGrid;
    private MapGenerator _generator;
    private EncounterMarker _neowEncounterPrefab;
    private EncounterMarker _currentRoom;
    [Inject] private RoomHandler _roomHandler;

    public void Awake() {
        _neowEncounterPrefab = Resources.Load<EncounterMarker>("Map/enc_neow");
        _generator = GetComponent<MapGenerator>();
    }

    public void GenerateMap() {
        List<int>[] pathList = new List<int>[6];
        
        _mapGrid = _generator.GeneratePath(pathList);
        _generator.GenerateNodes(_mapGrid, pathList);
        _generator.GenerateVertices(_mapGrid, pathList);

        _currentRoom = Instantiate(_neowEncounterPrefab, Vector3.zero, Quaternion.identity);
        List<EncounterMarker> initialRooms = GetInitialRooms();
        foreach (EncounterMarker room in initialRooms) {
            _currentRoom.ConnectNode(room);
        }
    }

    public void SetRoomInteractive() {
        foreach (EncounterMarker room in _currentRoom.AdjustSet) {
            StartCoroutine(HighlightRoom(room));
        }
    }

    private IEnumerator HighlightRoom(EncounterMarker room) {
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

    private List<EncounterMarker> GetInitialRooms() {
        List<EncounterMarker> initialRooms = new List<EncounterMarker>();

        for (int i = 0; i < _mapGrid.Width; ++i) {
            EncounterMarker node = _mapGrid.GetElement(0, i);
            if (node) {
                initialRooms.Add(node);
            }
        }

        return initialRooms;
    }
}
