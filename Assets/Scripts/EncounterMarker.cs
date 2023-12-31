using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EncounterType {
    MONSTER,
    EVENT,
    SHOP,
    REST,
    CHEST,
    ELITE,
    BOSS
}

public class EncounterMarker : MonoBehaviour {
    public EncounterType EncounterType {
        get;
        private set;
    }

    public Rowcol Rowcol {
        get;
        private set;
    }

    private HashSet<EncounterMarker> _adjustSet;
    public HashSet<EncounterMarker> AdjustSet {
        get { return _adjustSet; }
    }

    private OnMarkerSelected _requestedSelectedCallback;

    private void Awake() {
        _adjustSet = new HashSet<EncounterMarker>();
    }

    public void Initialize(EncounterType type, Rowcol rc, OnMarkerSelected callback) {
        EncounterType = type;
        Rowcol = rc;
        _requestedSelectedCallback = callback;
    }

    public void ConnectNode(EncounterMarker node) {
        if (!_adjustSet.Contains(node)) {
            _adjustSet.Add(node);
        }
    }

    private void OnMouseDown() {
        _requestedSelectedCallback.Invoke(this);
    }
}
