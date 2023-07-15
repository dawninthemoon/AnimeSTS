using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RieslingUtils;

public class CardEditorView : MonoBehaviour {
    #region EditorInput
    [SerializeField] private TMP_InputField _nameInput = null;
    [SerializeField] private TMP_InputField _portraitNameInput = null;
    [SerializeField] private TMP_Dropdown _rarityDropdown = null;
    [SerializeField] private TMP_Dropdown _typeDropdown = null;
    [SerializeField] private TMP_Dropdown _targetTypeDropdown = null;
    [SerializeField] private TMP_Dropdown _colorDropdown = null;
    [SerializeField] private TMP_InputField _costDropdown = null;
    [SerializeField] private TMP_InputField _upgradeCostDropdown = null;
    [SerializeField] private TMP_InputField _baseDescription = null;
    [SerializeField] private TMP_InputField _upgradeDescription = null;
    [SerializeField] private TMP_InputField _variables = null;
    [SerializeField] private TMP_InputField _numOfEffectsInput = null;
    #endregion

    [SerializeField] private GameObject _effectsPrefab = null;
    private List<TMP_InputField> _effectsTypeList;
    private List<TMP_InputField> _effectsAmountList;
    private int _currentChildCount;

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
    public int Cost {
        get { return int.Parse(_costDropdown.text); }
    }
    public int UpgradeCost {
        get { return int.Parse(_upgradeCostDropdown.text); }
    }
    public string Description {
        get { return _baseDescription.text; }
    }
    public string UpgradeDescription {
        get { return _upgradeCostDropdown.text; }
    }
    public string variables {
        get { return _variables.text; }
    }
    #endregion
    
    private void Start() {
        _effectsTypeList = new List<TMP_InputField>();
        _effectsAmountList = new List<TMP_InputField>();

        _currentChildCount = transform.childCount;

        _numOfEffectsInput.onValueChanged.AddListener(OnNumOfEffectsChanged);
    }

    private void OnNumOfEffectsChanged(string str) {
        int numofEffects = int.Parse(str);
        numofEffects = Mathf.Clamp(numofEffects, 0, 10);

        int prevCounts = _effectsTypeList.Count;
        if (prevCounts < numofEffects) {
            for (int i = 0; i < numofEffects - prevCounts; ++i) {
                var newEffectsInputFields = Instantiate(_effectsPrefab, transform);
                newEffectsInputFields.transform.SetSiblingIndex(_currentChildCount);

                _effectsTypeList.Add(newEffectsInputFields.transform.GetChild(0).GetComponent<TMP_InputField>());
                _effectsAmountList.Add(newEffectsInputFields.transform.GetChild(1).GetComponent<TMP_InputField>());

                ++_currentChildCount;
            }
        }
        else if (prevCounts > numofEffects) {
            while (prevCounts == numofEffects) {
                int lastIndex = prevCounts - 1;

                Destroy(_effectsTypeList[lastIndex].gameObject);
                Destroy(_effectsAmountList[lastIndex].gameObject);

                _effectsTypeList.RemoveAt(lastIndex);
                _effectsAmountList.RemoveAt(lastIndex);

                --prevCounts;
                --_currentChildCount;
            }
        }
    }
}
