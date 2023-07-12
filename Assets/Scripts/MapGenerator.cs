using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RieslingUtils;

public class MapGenerator : MonoBehaviour {
    [SerializeField] private EncounterMarker[] _encounterPrefabs = null;
    [SerializeField] private Transform _originPosition = null;
    [SerializeField] private Transform _mapSceneParent = null;
    [SerializeField] private float _offsetX = 1f;
    [SerializeField] private float _offsetY = 1f;
    private GameObject _vertexDotPrefab;
    private GameObject _circleMarkerPrefab;


    private void Awake() {
        _vertexDotPrefab = Resources.Load<GameObject>("Map/vertexDot");
        _circleMarkerPrefab = Resources.Load<GameObject>("Map/circleMarker");
    }

    public CustomGrid<EncounterMarker> GeneratePath(List<int>[] pathList) {
        CustomGrid<EncounterMarker> mapGrid = new CustomGrid<EncounterMarker>(GameMap.Width, GameMap.Height, _originPosition.position, _offsetX, _offsetY);
        
        for (int i = 0; i < pathList.Length; ++i) {
            pathList[i] = new List<int>();

            int col = Random.Range(0, GameMap.Width);
            if (i < 2 && mapGrid.GetElement(0, col)) {
                i--;
                continue;
            }
            for (int row = 0; row < GameMap.Height; ++row) {
                if (row > 0) {
                    col += Random.Range(-1, 2);
                    col = Mathf.Clamp(col, 0, GameMap.Width - 1);
                }

                pathList[i].Add(col);
            }
        }
        return mapGrid;
    }

    public void WriteRoomComplete(Vector3 position) {
        Instantiate(_circleMarkerPrefab, position, Quaternion.identity, _mapSceneParent);
    }

    public void GenerateNodes(CustomGrid<EncounterMarker> mapGrid, List<int>[] pathList, OnMarkerSelected callback) {
        // Instantiate Nodes
        for (int t = 0; t < 6; ++t) {
            for (int floor = 0; floor < GameMap.Height; ++floor) {
                int x = pathList[t][floor];
                if (!mapGrid.GetElement(floor, x)) {
                    Vector3 nodePosition = mapGrid.RowcolToPoint(floor, x).Jiggle(0.3f);
                    EncounterMarker newEncounter = CreateEncounter(nodePosition, floor, x, callback);
                    mapGrid.SetElement(floor, x, newEncounter);
                }
            }
        }

        for (int t = 0; t < 6; ++t) {
            for (int floor = 0; floor < GameMap.Height - 1; ++floor) {
                int x = pathList[t][floor];
                int nextX = pathList[t][floor + 1];

                EncounterMarker cur = mapGrid.GetElement(floor, x);
                EncounterMarker next = mapGrid.GetElement(new Rowcol(floor + 1, nextX));

                cur.ConnectNode(next);
            }
        }
    }
    private EncounterMarker CreateEncounter(Vector3 position, int row, int col, OnMarkerSelected callback) {
        EncounterType encounterType = GetEncounterType(row + 1);
        EncounterMarker nodePrefab = _encounterPrefabs[(int)encounterType];

        EncounterMarker node = Instantiate(nodePrefab, position, Quaternion.identity, _mapSceneParent);
        node.Initialize(encounterType, new Rowcol(row, col), callback);

        return node;
    }

    public void GenerateVertices(CustomGrid<EncounterMarker> mapGrid, List<int>[] pathList) {
        Queue<Rowcol> bfsQueue = new Queue<Rowcol>();

        for (int t = 0; t < 6; ++t) {
            Rowcol initialRowcol = new Rowcol(0, pathList[t][0]);
            MakeVertex(mapGrid, initialRowcol, new Rowcol(1, pathList[t][1]));
        }
    }

    private void MakeVertex(CustomGrid<EncounterMarker> mapGrid, Rowcol from, Rowcol to) {
        EncounterMarker prev = mapGrid.GetElement(from);
        EncounterMarker cur = mapGrid.GetElement(to);

        Vector3 prevPosition = prev.transform.position;
        Vector3 currentPosition = cur.transform.position;

        Vector3 diff = currentPosition - prevPosition;
        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg - 90f;
        
        float distance = Vector3.Distance(prevPosition, currentPosition);
        int dotCounts = (int)(distance * 7f);

        for (int i = 2; i < dotCounts - 1; ++i) {
            Vector3 dotPosition = Vector3.Lerp(prevPosition, currentPosition, (float)i / dotCounts);
            float dotAngle = angle + Random.Range(-15f, 15f);
            Instantiate(_vertexDotPrefab, dotPosition, Quaternion.Euler(0f, 0f, dotAngle), _mapSceneParent);
        }
        
        foreach (EncounterMarker nextNode in cur.AdjustSet) { 
            MakeVertex(mapGrid, to, nextNode.Rowcol);
        }
    }

    private EncounterType GetEncounterType(int floor) {
        if (floor == 1) {
            return EncounterType.MONSTER;
        }
        else if (floor == 9) {
            return EncounterType.CHEST;
        }
        else if (floor == GameMap.Height) {
            return EncounterType.REST;
        }
        
        int randNum = Random.Range(0, 1000);
        if (randNum < 50) {
            return EncounterType.SHOP;
        }
        else if (randNum < 170) {
            return EncounterType.REST;
        }
        else if (randNum < 390) {
            return EncounterType.EVENT;
        }
        else if (randNum < 518) {
            return EncounterType.ELITE;
        }
        return EncounterType.MONSTER;
    }
}
