using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RieslingUtils;
using System.Linq;

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
        [SerializeField] private TMP_InputField _baseNumOfEffectsInput = null;
        [SerializeField] private TMP_InputField _upgradedNumOfEffectsInput = null;
        #endregion

        [SerializeField] private GameObject _commandOptionPrefab = null;
        private List<TMP_Dropdown> _baseCommandsTypeList, _upgradedCommandsTypeList;
        private List<TMP_InputField> _baseCommandsAmountList, _upgradedCommandsAmountList;

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
            get { return _upgradeCostInputField.text; }
        }
        public string Variables {
            get { return _variables.text; }
        }
        public CommandInfo[] BaseCommands {
            get { return GetCommandsFromEditor(_baseCommandsTypeList, _baseCommandsAmountList); }
        }
        public CommandInfo[] UpgradedCommands {
            get { return GetCommandsFromEditor(_upgradedCommandsTypeList, _upgradedCommandsAmountList); }
        }
        #endregion
        
        private void Start() {
            _baseCommandsTypeList = new List<TMP_Dropdown>();
            _baseCommandsAmountList = new List<TMP_InputField>();
            _upgradedCommandsTypeList = new List<TMP_Dropdown>();
            _upgradedCommandsAmountList = new List<TMP_InputField>();

            var nestedType = typeof(CardCommand).GetNestedTypes(System.Reflection.BindingFlags.Public);
            var commandDropdown = _commandOptionPrefab.GetComponentInChildren<TMP_Dropdown>();
            List<string> optionList = nestedType
                .Where(type => (type.GetInterface("ICardCommand") != null))
                .Select(type => type.Name)
                .ToList();
            commandDropdown.options.Clear();
            commandDropdown.AddOptions(optionList);

            _baseNumOfEffectsInput.onEndEdit.AddListener(OnNumOfBaseCommandsChanged);
            _upgradedNumOfEffectsInput.onEndEdit.AddListener(OnNumOfUpgradedCommandsChanged);
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
            _baseNumOfEffectsInput.text = "0";
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
    
            if (info.baseCommands == null)
                _baseNumOfEffectsInput.text = "0";
            else
                _baseNumOfEffectsInput.text = info.baseCommands.Length.ToString();
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

        private void OnNumOfBaseCommandsChanged(string str) {
            int numofEffects = int.Parse(str);
            numofEffects = Mathf.Clamp(numofEffects, 0, 10);
            int siblingIndex = _baseNumOfEffectsInput.transform.parent.GetSiblingIndex();
            OnNumOfCommandsChanged(numofEffects, siblingIndex, _baseCommandsTypeList, _baseCommandsAmountList);
            
        }

        private void OnNumOfUpgradedCommandsChanged(string str) {
            int numofEffects = int.Parse(str);
            numofEffects = Mathf.Clamp(numofEffects, 0, 10);
            int siblingIndex = _upgradedNumOfEffectsInput.transform.parent.GetSiblingIndex();
            OnNumOfCommandsChanged(numofEffects, siblingIndex, _upgradedCommandsTypeList, _upgradedCommandsAmountList);
        }

        private void OnNumOfCommandsChanged(int numOfEffects, int indexStarts, List<TMP_Dropdown> typeList, List<TMP_InputField> amountList) {
            int prevCounts = typeList.Count;
            if (prevCounts < numOfEffects) {
                for (int i = 0; i < numOfEffects - prevCounts; ++i) {
                    var newEffectsInputFields = Instantiate(_commandOptionPrefab, transform);

                    typeList.Add(newEffectsInputFields.GetComponentInChildren<TMP_Dropdown>());
                    amountList.Add(newEffectsInputFields.GetComponentInChildren<TMP_InputField>());

                    newEffectsInputFields.transform.SetSiblingIndex(indexStarts + typeList.Count);
                }
            }
            else if (prevCounts > numOfEffects) {
                while (prevCounts > numOfEffects) {
                    int lastIndex = prevCounts - 1;
                    var content = typeList[lastIndex].transform.parent.gameObject;

                    typeList.RemoveAt(lastIndex);
                    amountList.RemoveAt(lastIndex);
                    Destroy(content);

                    --prevCounts;
                }
            }
        }

        private CommandInfo[] GetCommandsFromEditor(List<TMP_Dropdown> typeList, List<TMP_InputField> amountList) {
            CommandInfo[] commands = new CommandInfo[typeList.Count];
            for (int i = 0; i < commands.Length; ++i) {
                string name = typeList[i].options[typeList[i].value].text;
                string amount = amountList[i].text;
                commands[i] = new CommandInfo(name, amount);
            }
            return commands;
        }
    }
}
