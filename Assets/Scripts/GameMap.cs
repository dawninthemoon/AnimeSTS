using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RieslingUtils;

public class CustomGrid<T> where T : MonoBehaviour {
    public int Width { get; set; }
    public int Height { get; set; }
    public Vector3 OriginPosition { get; set; }
    private T[,] _gridArray;
    private float _offsetX;
    private float _offsetY;

    public CustomGrid(int width, int height, Vector3 origin, float offsetX, float offsetY) {
        Width = width;
        Height = height;
        OriginPosition = origin;

        _offsetX = offsetX;
        _offsetY = offsetY;

        _gridArray = new T[height, width];
    }

    public T GetElement(int row, int column) {
        return _gridArray[row, column];
    }

    public void SetElement(int row, int column, T value) {
        _gridArray[row, column] = value;
    }

    public Vector3 RowcolToPoint(int row, int column) {
        float x = column * _offsetX;
        float y = row * _offsetY;
        return OriginPosition + new Vector3(x, y);
    }

    public List<int> GetAdjustNode(int row, int column, bool ascending) {
        List<int> colList = new List<int>();
        for (int x = column - 1; x <= column + 1; ++x) {
            if (x < 0 || x >= Width) continue;

            int nextRow = (ascending ? row + 1 : row - 1);
            if (GetElement(nextRow, x)) {
                colList.Add(x);
            }
        }
        return colList;
    }
}

public struct Rowcol {
    public int row;
    public int column;
    public Rowcol(int r = 0, int c = 0) {
        row = r;
        column = c;
    }
}

public class GameMap : MonoBehaviour {
    [Range(4, 7), SerializeField] private int _width = 4;
    [Range(10, 20), SerializeField] private int _height = 10;
    [SerializeField] private Encounter[] _encounterPrefabs = null;
    [SerializeField] private Transform _originPosition = null;
    [SerializeField] private float _offsetX = 1f;
    [SerializeField] private float _offsetY = 1f;

    private GameObject _vertexDotPrefab;
    private CustomGrid<Encounter> _mapGrid;
    List<int>[] _pathList;
    private int CurrentFloor {
        get;
        set;
    }

    private void Start() {
        _vertexDotPrefab = Resources.Load<GameObject>("Map/vertexDot");
        CurrentFloor = 0;
        GeneratePath();
        GenerateNodes();
        GenerateVertices();

        for (int i = 0; i < _width; ++i) {
            Encounter node = _mapGrid.GetElement(CurrentFloor, i);
            if (node) {
                StartCoroutine(HighlightNode(node));
            }
        }
    }

    private IEnumerator HighlightNode(Encounter node) {
        Transform t = node.transform;
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
    
    private void GeneratePath() {
        _mapGrid = new CustomGrid<Encounter>(_width, _height, _originPosition.position, _offsetX, _offsetY);
        
        _pathList = new List<int>[6];
        for (int t = 0; t < 6; ++t) {
            _pathList[t] = new List<int>();

            int col = Random.Range(0, _width);
            if (t < 2 && _mapGrid.GetElement(0, col)) {
                t--;
                continue;
            }
            for (int row = 0; row < _height; ++row) {
                if (row > 0) {
                    col += Random.Range(-1, 2);
                    col = Mathf.Clamp(col, 0, _width - 1);
                }

                _pathList[t].Add(col);
            }
        }
    }

    private Encounter CreateEncounter(int row, int col) {
        Vector3 nodePosition = _mapGrid.RowcolToPoint(row, col).Jiggle(0.3f);

        EncounterType encounterType = GetEncounterType(row + 1);
        Encounter nodePrefab = _encounterPrefabs[(int)encounterType];

        Encounter node = Instantiate(nodePrefab, nodePosition, Quaternion.identity);
        node.EncounterType = encounterType;
        return node;
    }

    private void GenerateNodes() {
        // Instantiate Nodes
        for (int t = 0; t < 6; ++t) {
            for (int floor = 0; floor < _height; ++floor) {
                int x = _pathList[t][floor];
                if (!_mapGrid.GetElement(floor, x)) {
                    Encounter newEncounter = CreateEncounter(floor, x);
                    _mapGrid.SetElement(floor, x, newEncounter);
                }
            }
        }

        // Adjust Nodes
        for (int t = 0; t < 6; ++t) {
            for (int floor = 0; floor < _height - 1; ++floor) {
                int x = _pathList[t][floor];
                int nextX = _pathList[t][floor + 1];

                Encounter cur = _mapGrid.GetElement(floor, x);
                Rowcol next = new Rowcol(floor + 1, nextX);

                cur.ConnectNode(next);
            }
        }
    }

    private void GenerateVertices() {
        Queue<Rowcol> bfsQueue = new Queue<Rowcol>();

        for (int t = 0; t < 6; ++t) {
            Dictionary<Rowcol, bool> visitMarker = new Dictionary<Rowcol, bool>();

            Rowcol initialRowcol = new Rowcol(0, _pathList[t][0]);
            MakeVertex(initialRowcol, new Rowcol(1, _pathList[t][1]));
        }
    }

    private void MakeVertex(Rowcol from, Rowcol to) {
        Encounter prev = _mapGrid.GetElement(from.row, from.column);
        Encounter cur = _mapGrid.GetElement(to.row, to.column);

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
        
        foreach (Rowcol nextRowcol in cur.AdjustSet) { 
            MakeVertex(to, nextRowcol);
        }
    }

    private EncounterType GetEncounterType(int floor) {
        if (floor == 1) {
            return EncounterType.MONSTER;
        }
        else if (floor == 9) {
            return EncounterType.CHEST;
        }
        else if (floor == _height) {
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

    private void OnDrawGizmos() {
        if (_pathList == null) return;

        Color[] colorArr = new Color[6] {
            Color.red,
            Color.yellow,
            Color.green,
            Color.cyan,
            Color.magenta,
            Color.white
        };

        for (int t = 0; t < 6; ++t) {
            for (int floor = 0; floor < _height - 1; ++floor) {
                int x = _pathList[t][floor];
                int nextX = _pathList[t][floor + 1];
                
                Vector3 cur = _mapGrid.GetElement(floor, x).transform.position;
                Vector3 next = _mapGrid.GetElement(floor + 1, nextX).transform.position;

                Gizmos.color = colorArr[t];
                Gizmos.DrawLine(cur, next);
            }
        }
    }
}
