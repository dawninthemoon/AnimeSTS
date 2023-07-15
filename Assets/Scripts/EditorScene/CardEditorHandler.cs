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

        _cardPreview.ShowCardFrame(color, rarity, type);
        _cardPreview.ShowCardData(name, color, type, portraitName, cost);
        ShowCardDescription();
    }

    public void OnStrengthChanged(TMPro.TMP_InputField field) {
        int strength = int.Parse(field.text);
        _playerStatus.ChangeEffectValue("strength", strength);
        ShowCardDescription();
    }

    public void OnDexterityChanged(TMPro.TMP_InputField field) {
        int dexterity = int.Parse(field.text);
        _playerStatus.ChangeEffectValue("dexterity", dexterity);
        ShowCardDescription();
    }

    private void ShowCardDescription() {
        string description = _cardEditorView.Description;
        string variables = _cardEditorView.variables;

        if (description == "") return;
        
        var variableData = _variableParser.Parse(variables);
        foreach (KeyValuePair<string, int> variable in variableData) {
            string formatString = '{' + variable.Key + '}';
            description = description.Replace(formatString, variable.Value.ToString());
        }

        _cardPreview.ShowCardText(description);
    }
}
