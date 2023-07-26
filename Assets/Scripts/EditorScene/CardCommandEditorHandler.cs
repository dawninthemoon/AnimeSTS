using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Linq;

namespace GameEditor {
    public class CardCommandEditorHandler : MonoBehaviour {
        [SerializeField] private TMP_InputField _baseNumOfEffectsInput = null;
        [SerializeField] private TMP_InputField _upgradedNumOfEffectsInput = null;

        [SerializeField] private GameObject _commandOptionPrefab = null;
        private List<CustomDropdown> _baseCommandsTypeList, _upgradedCommandsTypeList;
        private List<TMP_InputField> _baseCommandsAmountList, _upgradedCommandsAmountList;
        private UnityEvent<List<string>> _getModelEvent;

        private void Start() {
            _baseCommandsTypeList = new List<CustomDropdown>();
            _baseCommandsAmountList = new List<TMP_InputField>();
            _upgradedCommandsTypeList = new List<CustomDropdown>();
            _upgradedCommandsAmountList = new List<TMP_InputField>();

            _baseNumOfEffectsInput.onEndEdit.AddListener(OnNumOfBaseCommandsChanged);
            _upgradedNumOfEffectsInput.onEndEdit.AddListener(OnNumOfUpgradedCommandsChanged);

            _getModelEvent = new UnityEvent<List<string>>();
            _getModelEvent.AddListener(SetCommandsName);
        }

        private void SetCommandsName(List<string> commandList) {
            commandList.Clear();

            var nestedType = typeof(CardCommand).GetNestedTypes(System.Reflection.BindingFlags.Public);
            var tempList = nestedType
                    .Where(type => (type.GetInterface("ICardCommand") != null))
                    .Select(type => type.Name)
                    .ToList();

            for (int i = 0; i < tempList.Count; ++i) {
                commandList.Add(tempList[i]);
            }
        }

        public void RefreshCommandEditor() {
            _baseNumOfEffectsInput.text = "0";
            OnNumOfBaseCommandsChanged("0");
            _upgradedNumOfEffectsInput.text = "0";
            OnNumOfUpgradedCommandsChanged("0");
        }

        public void RefreshCommandEditor(CardInfo card) {
            if (card.baseCommands == null) {
                _baseNumOfEffectsInput.text = "0";
                OnNumOfBaseCommandsChanged("0");
            }
            else {
                _baseNumOfEffectsInput.text = card.baseCommands.Length.ToString();
                OnNumOfBaseCommandsChanged(_baseNumOfEffectsInput.text);
                for (int i = 0; i < _baseCommandsTypeList.Count; ++i) {
                    _baseCommandsTypeList[i].SetOptionByName(card.baseCommands[i].name);
                    _baseCommandsAmountList[i].text = card.baseCommands[i].amount;
                }
            }

            if (card.upgradeCommands == null) {
                _upgradedNumOfEffectsInput.text = "0";
                OnNumOfUpgradedCommandsChanged("0");
            }
            else {
                _upgradedNumOfEffectsInput.text = card.upgradeCommands.Length.ToString();
                OnNumOfUpgradedCommandsChanged(_upgradedNumOfEffectsInput.text);
                for (int i = 0; i < _upgradedCommandsTypeList.Count; ++i) {
                    _upgradedCommandsTypeList[i].SetOptionByName(card.upgradeCommands[i].name);
                    _upgradedCommandsAmountList[i].text = card.upgradeCommands[i].amount;
                }
            }
        }

        public CommandInfo[] GetCommandInformation(bool isUpgraded) {
            List<CustomDropdown> typeList = isUpgraded ? _baseCommandsTypeList : _upgradedCommandsTypeList;
            List<TMP_InputField> amountList = isUpgraded ? _baseCommandsAmountList : _upgradedCommandsAmountList;
            return GetCommandsFromEditor(typeList, amountList);
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

        private void OnNumOfCommandsChanged(int numOfEffects, int indexStarts, List<CustomDropdown> typeList, List<TMP_InputField> amountList) {
            int prevCounts = typeList.Count;
            if (prevCounts < numOfEffects) {
                for (int i = 0; i < numOfEffects - prevCounts; ++i) {
                    var newEffectsInputFields = Instantiate(_commandOptionPrefab, transform);

                    var dropdown = newEffectsInputFields.GetComponentInChildren<CustomDropdown>();
                    dropdown.InitializeDropdownSettings(_getModelEvent, null, null);
                    
                    typeList.Add(dropdown);
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

        private CommandInfo[] GetCommandsFromEditor(List<CustomDropdown> typeList, List<TMP_InputField> amountList) {
            CommandInfo[] commands = new CommandInfo[typeList.Count];
            for (int i = 0; i < commands.Length; ++i) {
                string name = typeList[i].CurrentOptionName;
                string amount = amountList[i].text;
                commands[i] = new CommandInfo(name, amount);
            }
            return commands;
        }
    }
}