using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace GameEditor {
    public class EditorMain : MonoBehaviour {
        [SerializeField] private CardEditorHandler _cardEditorHandler = null;

        private void Start() {
            
        }

        public void ExportCards() {
            string jsonText = JsonHelper.ToJson(_cardEditorHandler.GetAllCardInformation(), true);
            File.WriteAllText(Application.dataPath + "/Resources/Cards/CardInfo.json", jsonText);
        }
    }
}
