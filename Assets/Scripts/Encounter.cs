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

    private HashSet<Rowcol> _adjustSet;
    public HashSet<Rowcol> AdjustSet {
        get { return _adjustSet; }
    }

    private void Awake() {
        _adjustSet = new HashSet<Rowcol>();
    }

    public void ConnectNode(Rowcol rc) {
        if (!_adjustSet.Contains(rc)) {
            _adjustSet.Add(rc);
        }
    }
}
