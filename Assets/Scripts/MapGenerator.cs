using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RieslingUtils;

public class MapGenerator : MonoBehaviour {
    [SerializeField] private Encounter[] _encounterPrefabs = null;
    [SerializeField] private Transform _originPosition = null;
    [SerializeField] private float _offsetX = 1f;
    [SerializeField] private float _offsetY = 1f;
    private GameObject _vertexDotPrefab;

    private void Awake() {
        _vertexDotPrefab = Resources.Load<GameObject>("Map/vertexDot");
    }

    public CustomGrid<Encounter> GeneratePath(List<int>[] pathList) {
        CustomGrid<Encounter> mapGrid = new CustomGrid<Encounter>(GameMap.Width, GameMap.Height, _originPosition.position, _offsetX, _offsetY);
        
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

    public void GenerateNodes(CustomGrid<Encounter> mapGrid, List<int>[] pathList) {
        // Instantiate Nodes
        for (int t = 0; t < 6; ++t) {
            for (int floor = 0; floor < GameMap.Height; ++floor) {
                int x = pathList[t][floor];
                if (!mapGrid.GetElement(floor, x)) {
                    Vector3 nodePosition = mapGrid.RowcolToPoint(floor, x).Jiggle(0.3f);
                    Encounter newEncounter = CreateEncounter(nodePosition, floor, x);
                    mapGrid.SetElement(floor, x, newEncounter);
                }
            }
        }

        // Adjust Nodes
        for (int t = 0; t < 6; ++t) {
            for (int floor = 0; floor < GameMap.Height - 1; ++floor) {
                int x = pathList[t][floor];
                int nextX = pathList[t][floor + 1];

                Encounter cur = mapGrid.GetElement(floor, x);
                Rowcol next = new Rowcol(floor + 1, nextX);

                cur.ConnectNode(mapGrid.GetElement(next));
            }
        }
    }

    private Encounter CreateEncounter(Vector3 position, int row, int col) {
        EncounterType encounterType = GetEncounterType(row + 1);
        Encounter nodePrefab = _encounterPrefabs[(int)encounterType];

        Encounter node = Instantiate(nodePrefab, position, Quaternion.identity);
        node.EncounterType = encounterType;
        node.RowcolPosition = new Rowcol(row, col);
        return node;
    }

    public void GenerateVertices(CustomGrid<Encounter> mapGrid, List<int>[] pathList) {
        Queue<Rowcol> bfsQueue = new Queue<Rowcol>();

        for (int t = 0; t < 6; ++t) {
            Dictionary<Rowcol, bool> visitMarker = new Dictionary<Rowcol, bool>();

            Rowcol initialRowcol = new Rowcol(0, pathList[t][0]);
            MakeVertex(mapGrid, initialRowcol, new Rowcol(1, pathList[t][1]));
        }
    }

    private void MakeVertex(CustomGrid<Encounter> mapGrid, Rowcol from, Rowcol to) {
        Encounter prev = mapGrid.GetElement(from);
        Encounter cur = mapGrid.GetElement(to);

        Vector3 prevPosition = prev.transform.position;
        Vector3 currentPosition = cur.transform.position;

        Vector3 diff = currentPosition - prevPosition;
        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg - 90f;
        
        float distance = Vector3.Distance(prevPosition, currentPosition);
        int dotCounts = (int)(distance * 7f);

        for (int i = 2; i < dotCounts - 1; ++i) {
            Vector3 dotPosition = Vector3.Lerp(prevPosition, currentPosition, (float)i / dotCounts);
            float dotAngle = angle + Random.Range(-15f, 15f);
            Instantiate(_vertexDotPrefab, dotPosition, Quaternion.Euler(0f, 0f, dotAngle));
        }
        
        foreach (Encounter nextNode in cur.AdjustSet) { 
            MakeVertex(mapGrid, to, nextNode.RowcolPosition);
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
