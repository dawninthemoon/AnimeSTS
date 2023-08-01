using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RieslingUtils;

public class CombatEntityStatus : MonoBehaviour {
    [SerializeField] private SpriteRenderer _hpFill = null;
    [SerializeField]private SpriteRenderer _block = null;
    [SerializeField] private TMP_Text _hpText = null;
    [SerializeField]private TMP_Text _blockText = null;

    public void UpdateHPBarStatus(int curHP, int maxHP) {
        float xScale = (float)curHP / maxHP * 3f;
        _hpFill.transform.localScale = _hpFill.transform.localScale.ChangeXPos(xScale);
        _hpText.text = curHP.ToString() + "/" + maxHP.ToString();
    }

    public void UpdateBlockStatus(int blockAmount) {
        if (blockAmount > 0) {
            _block.gameObject.SetActive(true);
            _blockText.text = blockAmount.ToString();
        }
        else {
            _block.gameObject.SetActive(false);
            _blockText.text = null;
        }
    }
}
