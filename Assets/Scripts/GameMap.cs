using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RieslingUtils;

public class GameMap : MonoBehaviour {
    private CustomGrid<Encounter> _mapGrid;
    [SerializeField] private MapGenerator _generator = null;

    private void Start() {
        List<int>[] pathList = new List<int>[6];
        _mapGrid = _generator.GeneratePath(pathList);
        _generator.GenerateNodes(_mapGrid, pathList);
        _generator.GenerateVertices(_mapGrid, pathList);

        StartHighlightNode(0);
    }

    public void StartHighlightNode(int floor) {
        for (int i = 0; i < _mapGrid.Width; ++i) {
            Encounter node = _mapGrid.GetElement(floor, i);
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
}
