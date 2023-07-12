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

public class Encounter : MonoBehaviour {
    public EncounterType EncounterType {
        get;
        set;
    }

    public Rowcol RowcolPosition {
        get;
        set;
    }

    private HashSet<Encounter> _adjustSet;
    public HashSet<Encounter> AdjustSet {
        get { return _adjustSet; }
    }

    private void Awake() {
        _adjustSet = new HashSet<Encounter>();
    }

    public void ConnectNode(Encounter node) {
        if (!_adjustSet.Contains(node)) {
            _adjustSet.Add(node);
        }
    }
}
