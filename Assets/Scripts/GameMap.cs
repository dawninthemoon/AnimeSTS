using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RieslingUtils;

public class GameMap : MonoBehaviour {
    public static readonly int Width = 7;
    public static readonly int Height = 15;
    private CustomGrid<Encounter> _mapGrid;
    private MapGenerator _generator;
    private Encounter _neowEncounterPrefab;

    public void Awake() {
        _neowEncounterPrefab = Resources.Load<Encounter>("Map/enc_neow");
        _generator = GetComponent<MapGenerator>();
    }

    public Encounter GenerateMap() {
        List<int>[] pathList = new List<int>[6];
        
        _mapGrid = _generator.GeneratePath(pathList);
        _generator.GenerateNodes(_mapGrid, pathList);
        _generator.GenerateVertices(_mapGrid, pathList);

        Encounter neowRoom = Instantiate(_neowEncounterPrefab, Vector3.zero, Quaternion.identity);
        List<Encounter> initialRooms = GetInitialRooms();
        foreach (Encounter room in initialRooms) {
            neowRoom.ConnectNode(room);
        }

        return neowRoom;
    }

    public List<Encounter> GetInitialRooms() {
        List<Encounter> initialRooms = new List<Encounter>();

        for (int i = 0; i < _mapGrid.Width; ++i) {
            Encounter node = _mapGrid.GetElement(0, i);
            if (node) {
                initialRooms.Add(node);
            }
        }

        return initialRooms;
    }
}
