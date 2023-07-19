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
        private GameObject _selectedContent;
        public int SelectedIndex { get; private set; }

        private void Start() {
            _toggleButton.onClick.AddListener(Toggle);
            _createButton.onClick.AddListener(CreateNew);

            InitalizeContent();
        }

        private void InitalizeContent() {
            List<string> nameList = new List<string>();
            _getModelCallback.Invoke(nameList);

            for (int i = 0; i < nameList.Count; ++i) {
                CreateContent(nameList[i], i);
            }
        }

        private void Toggle() {
            var triangleImage = _toggleButton.transform.GetChild(0);
            triangleImage.localScale = new Vector3(1f, -triangleImage.localScale.y, 1f);
            _additionalWindow.SetActive(!_additionalWindow.activeSelf);
        }

        private void CreateContent(string name, int index) {
            var newContent = Instantiate(_contentPrefab, _contentTransform);
            newContent.GetComponentInChildren<TMP_Text>().text = name;
            newContent.onClick.AddListener(() => OnOptionSelected(index));
        }

        public void CreateNew() {
            CreateContent("New Content", _contentTransform.childCount);
            _onOptionCreated.Invoke();
        }

        public void OnCurrentOptionChanged(string name) {
            _selectedName.text = name;
            _selectedContent.GetComponentInChildren<TMP_Text>().text = name;
        }

        private void OnOptionSelected(int index) {
            _selectedContent = _contentTransform.GetChild(index).gameObject;
            SelectedIndex = index;
            Toggle();
            _onOptionSelected.Invoke();
        }
    }
}
