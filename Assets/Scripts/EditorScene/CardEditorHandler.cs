using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEditor {
    public class CardEditorHandler : MonoBehaviour {
        [SerializeField] private CardEditorView _cardEditorView = null;
        [SerializeField] private CardSpriteView _cardPreview = null;
        [SerializeField] private EntityBase _playerStatus = null;
        [SerializeField] private CustomDropdown _cardDropdown = null;
        private List<CardInfo> _cardInfoList;
        private CommandDataParser _variableParser;
        private CardBase _cardBase;

        private void Awake() {
            _cardInfoList = new List<CardInfo>();
            CardInfo[] loadedCardInfo = JsonHelper.LoadJsonFile<CardInfo>(Application.dataPath + "/Resources/Cards/CardInfo.json");
            foreach (CardInfo c in loadedCardInfo) {
                _cardInfoList.Add(c);
            }
        }

        private void Start() {
            GameVariableContainer gameVariables = new GameVariableContainer();
            _variableParser = new CommandDataParser(gameVariables);
        }
        
        private void SaveCurrentSetting() {
            CardInfo cardInfo = new CardInfo();
            cardInfo.cardName = _cardEditorView.CardName;
            cardInfo.portraitName = _cardEditorView.PortraitName;
            cardInfo.cost = _cardEditorView.Cost;
            cardInfo.upgradeCost = _cardEditorView.UpgradedCost;
            cardInfo.color = _cardEditorView.Color;
            cardInfo.rarity = _cardEditorView.Rarity;
            cardInfo.targetType = _cardEditorView.TargetType;
            cardInfo.type = _cardEditorView.Type;
            cardInfo.baseDescription = _cardEditorView.Description;
            cardInfo.upgradeDescription = _cardEditorView.UpgradedDescription;
            cardInfo.variables = _cardEditorView.Variables;
            cardInfo.baseCommands = _cardEditorView.BaseCommands;
            cardInfo.upgradeCommands = _cardEditorView.UpgradedCommands;
            _cardInfoList[_cardDropdown.SelectedIndex] = cardInfo;

            OnCardChanged(cardInfo);
        }

        public void OnApplyButtonPressed() {
            SaveCurrentSetting();
            CardInfo info = _cardInfoList[_cardDropdown.SelectedIndex];
            _cardPreview.ShowCard(info, info.cost, _variableParser, _playerStatus);
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
            string variables = _cardEditorView.Variables;

            if (description == "") return;
            
            var variableData = _variableParser.ParseVariable(variables, _playerStatus);
            foreach (KeyValuePair<string, int> variable in variableData) {
                string formatString = '{' + variable.Key + '}';
                description = description.Replace(formatString, variable.Value.ToString());
            }

            _cardPreview.SetCardText(description);
        }

        public void SetCardList(List<string> cardNameList) {
            cardNameList.Clear();
            for (int i = 0; i < _cardInfoList.Count; ++i) {
                cardNameList.Add(_cardInfoList[i].cardName);
            }
        }

        public void OnNewCardCreated() {
            CardInfo cardInfo = new CardInfo();
            _cardInfoList.Add(cardInfo);
            _cardEditorView.RefreshAll();
        }

        public void ChangeCardInfo() {
            int index = _cardDropdown.SelectedIndex;
            CardInfo cardInfo = _cardInfoList[index];
            _cardEditorView.ChangeInfo(cardInfo);
            OnCardChanged(cardInfo);
            OnApplyButtonPressed();
        }

        public void OnCardChanged(CardInfo value) {
            _cardDropdown.OnCurrentOptionChanged(value.cardName);
        }

        public CardInfo[] GetAllCardInformation() {
            return _cardInfoList.ToArray();
        }
    }
}