using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEditorHandler : MonoBehaviour {
    [SerializeField] private CardView _cardPreview = null;
    [SerializeField] private EntityBase _playerStatus = null;

    private CardVariableParser _variableParser;
    private void Start() {
        _variableParser = new CardVariableParser(_playerStatus);
        
    }
}
