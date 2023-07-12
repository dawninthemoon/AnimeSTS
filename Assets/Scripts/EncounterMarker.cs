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
        set;
    }

    public Rowcol RowcolPosition {
        get;
        set;
    }

    private HashSet<EncounterMarker> _adjustSet;
    public HashSet<EncounterMarker> AdjustSet {
        get { return _adjustSet; }
    }

    private void Awake() {
        _adjustSet = new HashSet<EncounterMarker>();
    }

    public void ConnectNode(EncounterMarker node) {
        if (!_adjustSet.Contains(node)) {
            _adjustSet.Add(node);
        }
    }
}
