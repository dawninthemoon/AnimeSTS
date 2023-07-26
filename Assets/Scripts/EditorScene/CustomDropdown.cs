using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using RieslingUtils;

namespace GameEditor {
    public class CustomDropdown : MonoBehaviour {
        [SerializeField] private UnityEvent<List<string>> _getModelCallback = null;
        [SerializeField] private UnityEvent _onOptionSelected = null;
        [SerializeField] private UnityEvent _onOptionCreated = null;
        [SerializeField] private Button _contentPrefab = null;
        [SerializeField] private GameObject _additionalWindow = null;
        [SerializeField] private Button _toggleButton = null;
        [SerializeField] private Button _createButton = null;
        [SerializeField] private Transform _contentTransform = null;
        [SerializeField] private TMP_InputField _cardSearchInputField = null;
        [SerializeField] private TMP_Text _selectedName;
        private float _defaultToggleYScale;
        private List<string> _optionList;
        private GameObject _selectedContent;
        public int SelectedIndex { get; private set; }
        public string CurrentOptionName {
            get { return _optionList[SelectedIndex]; }
        }

        private void Awake() {
            _defaultToggleYScale = _toggleButton.transform.GetChild(0).localScale.y;
        }

        private void Start() {
            _toggleButton.onClick.AddListener(Toggle);
            _createButton?.onClick.AddListener(CreateNew);
            InitalizeContent();
        }

        public void InitializeDropdownSettings(UnityEvent<List<string>> getModelCallback, UnityEvent onOptionSelected, UnityEvent onOptionCreated) {
            _getModelCallback = getModelCallback;
            _onOptionSelected = onOptionSelected;
            _onOptionCreated = onOptionCreated;
            InitalizeContent();
        }

        public void InitalizeContent() {
            if (_optionList != null || _getModelCallback == null) return;

            _optionList = new List<string>();
            _getModelCallback.Invoke(_optionList);

            for (int i = 0; i < _optionList.Count; ++i) {
                CreateContent(_optionList[i], i);
            }
        }

        private void Toggle() {
            var triangleImage = _toggleButton.transform.GetChild(0);
            triangleImage.localScale = new Vector3(1f, -triangleImage.localScale.y, 1f);
            _additionalWindow.SetActive(!_additionalWindow.activeSelf);
        }

        private void CloseAdditionalWindow() {
            var triangleImage = _toggleButton.transform.GetChild(0);
            triangleImage.localScale = new Vector3(1f, _defaultToggleYScale, 1f);
            _additionalWindow.SetActive(false);
        }

        private void CreateContent(string name, int index) {
            var newContent = Instantiate(_contentPrefab, _contentTransform);
            newContent.GetComponentInChildren<TMP_Text>().text = name;
            newContent.onClick.AddListener(() => OnOptionSelected(index));
        }

        public void SetOptionByName(string name) {
            for (int i = 0; i < _optionList.Count; ++i) {
                if (_optionList[i].Equals(name)) {
                    OnOptionSelected(i);
                    break;
                }
            }
        }

        public void CreateNew() {
            CreateContent("New Content", _contentTransform.childCount);
            _onOptionCreated?.Invoke();
        }

        public void OnCurrentOptionChanged(string str) {
            _optionList[SelectedIndex] = str;
            _selectedName.text = str;
            _selectedContent.GetComponentInChildren<TMP_Text>().text = str;
        }

        private void OnOptionSelected(int index) {
            _selectedName.text = _optionList[index];
            _selectedContent = _contentTransform.GetChild(index).gameObject;
            SelectedIndex = index;
            CloseAdditionalWindow();
            _onOptionSelected?.Invoke();
        }

        public void OnSearchEndEdit(TMP_InputField input) {
            string str = input.text;
            FilterWithRegex(str, (str == ""));
        }

        public void FilterWithRegex(string pattern, bool isNull) {
            for (int i = 0; i < _optionList.Count; ++i) {
                GameObject content = _contentTransform.GetChild(i).gameObject;
                bool active = isNull || StringUtils.Contains(_optionList[i], pattern);
                content.transform.gameObject.SetActive(active);
            }
        }
    }
}
