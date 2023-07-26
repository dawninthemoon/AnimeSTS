using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RieslingUtils;

namespace GameEditor {
    public class CardEditorView : MonoBehaviour {
        #region EditorInput
        [SerializeField] private TMP_InputField _nameInput = null;
        [SerializeField] private TMP_InputField _portraitNameInput = null;
        [SerializeField] private TMP_Dropdown _rarityDropdown = null;
        [SerializeField] private TMP_Dropdown _typeDropdown = null;
        [SerializeField] private TMP_Dropdown _targetTypeDropdown = null;
        [SerializeField] private TMP_Dropdown _colorDropdown = null;
        [SerializeField] private TMP_InputField _costInputField = null;
        [SerializeField] private TMP_InputField _upgradeCostInputField = null;
        [SerializeField] private TMP_InputField _baseDescription = null;
        [SerializeField] private TMP_InputField _upgradeDescription = null;
        [SerializeField] private TMP_InputField _variables = null;
        #endregion

        #region Properties
        public string CardName {
            get { return _nameInput.text; }
        }
        public string PortraitName { 
            get { return _portraitNameInput.text; }
        }
        public CardInfo.Rarity Rarity  {
            get { 
                string str = _rarityDropdown.options[_rarityDropdown.value].text;
                return EnumUtil.Parse<CardInfo.Rarity>(str);
            }
        }
        public CardInfo.Type Type {
            get { 
                string str = _typeDropdown.options[_typeDropdown.value].text;
                return EnumUtil.Parse<CardInfo.Type>(str);
            }
        }
        public CardInfo.TargetType TargetType {
            get { 
                string str = _targetTypeDropdown.options[_targetTypeDropdown.value].text;
                return EnumUtil.Parse<CardInfo.TargetType>(str);
            }
        }
        public CardInfo.Color Color {
            get { 
                string str = _colorDropdown.options[_colorDropdown.value].text;
                return EnumUtil.Parse<CardInfo.Color>(str);
            }
        }
        public string Cost {
            get { return _costInputField.text; }
        }
        public string UpgradedCost {
            get { return _upgradeCostInputField.text; }
        }
        public string Description {
            get { return _baseDescription.text; }
        }
        public string UpgradedDescription {
            get { return _upgradeDescription.text; }
        }
        public string Variables {
            get { return _variables.text; }
        }
        public CommandInfo[] BaseCommands {
            get { return _commandEditor.GetCommandInformation(false); }
        }
        public CommandInfo[] UpgradedCommands {
            get { return _commandEditor.GetCommandInformation(true); }
        }
        #endregion

        private CardCommandEditorHandler _commandEditor;
        
        private void Start() {
            _commandEditor = GetComponent<CardCommandEditorHandler>();
        }

        public void RefreshAll() {
            _nameInput.text = "New Content";
            _portraitNameInput.text = "";
            _rarityDropdown.value = 0;
            _typeDropdown.value = 0;
            _targetTypeDropdown.value = 0;
            _colorDropdown.value = 0;
            _costInputField.text = "";
            _upgradeCostInputField.text = "";
            _baseDescription.text = "";
            _upgradeDescription.text = "";
            _variables.text = "";
            _commandEditor.RefreshCommandEditor();
        }

        public void ChangeInfo(CardInfo info) {
            _nameInput.text = info.cardName;
            _portraitNameInput.text = info.portraitName;
            _rarityDropdown.value = GetDropdownValue(_rarityDropdown, info.rarity.ToString());
            _typeDropdown.value = GetDropdownValue(_typeDropdown, info.type.ToString());
            _targetTypeDropdown.value = GetDropdownValue(_targetTypeDropdown, info.targetType.ToString());
            _colorDropdown.value = GetDropdownValue(_colorDropdown, info.color.ToString());
            _costInputField.text = info.cost.ToString();
            _upgradeCostInputField.text = info.upgradeCost.ToString();
            _baseDescription.text = info.baseDescription;
            _upgradeDescription.text = info.upgradeDescription;
            _variables.text = info.variables;
            _commandEditor.RefreshCommandEditor(info);
        }

        private int GetDropdownValue(TMP_Dropdown dropdown, string value) {
            int idx = 0;
            foreach (var option in dropdown.options) {
                if (option.text.Equals(value)) {
                    break;
                }
                ++idx;
            }
            return idx;
        }
    }
}
