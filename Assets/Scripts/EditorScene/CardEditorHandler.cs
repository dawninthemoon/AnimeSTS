using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEditorHandler : MonoBehaviour {
    [SerializeField] private CardEditorView _cardEditorView = null;
    [SerializeField] private CardView _cardPreview = null;
    [SerializeField] private EntityBase _playerStatus = null;

    private CardVariableParser _variableParser;
    private void Start() {
        _variableParser = new CardVariableParser(_playerStatus);
    }

    public void OnApplyButtonPressed() {
        string name = _cardEditorView.CardName;
        string portraitName = _cardEditorView.PortraitName;
        string cost = _cardEditorView.Cost;
        CardInfo.Color color = _cardEditorView.Color;
        CardInfo.Rarity rarity = _cardEditorView.Rarity;
        CardInfo.Type type = _cardEditorView.Type;
        string description = _cardEditorView.Description;

        _cardPreview.ShowCardFrame(color, rarity, type);
        _cardPreview.ShowCardData(name, color, type, portraitName, cost);
        _cardPreview.ShowCardText(description);
    }
}
