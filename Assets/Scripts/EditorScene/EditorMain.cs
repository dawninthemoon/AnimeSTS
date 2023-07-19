using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace GameEditor {
    public class EditorMain : MonoBehaviour {
        [SerializeField] private CardBase _cardPreview = null;
        private List<CardInfo> _cardInfoList;

        private void Start() {
            _cardInfoList = new List<CardInfo>();
        }

        private void LoadCards() {
            
        }

        public void ExportCards() {
            _cardInfoList.Add(_cardPreview.Info);
            string jsonText = JsonHelper.ToJson(_cardInfoList.ToArray(), true);
            File.WriteAllText(Application.dataPath + "/Resources/Cards/CardInfo.json", jsonText);
        }
    }
}
