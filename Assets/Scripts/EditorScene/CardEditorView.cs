using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardEditorView : MonoBehaviour {
    #region EditorInput
    [SerializeField] private TMP_InputField _nameInput = null;
    [SerializeField] private TMP_InputField _portraitNameInput = null;
    [SerializeField] private TMP_Dropdown _rarityDropdown = null;
    [SerializeField] private TMP_Dropdown _typeDropdown = null;
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
    
    private void Start() {
        _effectsTypeList = new List<TMP_InputField>();
        _effectsAmountList = new List<TMP_InputField>();

        _currentChildCount = transform.childCount;

        _numOfEffectsInput.onValueChanged.AddListener(OnNumOfEffectsChanged);
    }

    public void OnNumOfEffectsChanged(string str) {
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
