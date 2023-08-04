using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardImageView : CardView {
    [SerializeField] private Image _cardFrame = null;
    [SerializeField] private Image _portrait = null;
    [SerializeField] private Image _nameFrame = null;
    [SerializeField] private Image _typeFrame = null;
    [SerializeField] private Image _costFrame = null;
    [SerializeField] private TextMeshProUGUI _cardText = null;
    [SerializeField] private TextMeshProUGUI _nameText = null;
    [SerializeField] private TextMeshProUGUI _typeText = null;
    [SerializeField] private TextMeshProUGUI _costText = null;

    protected override void SetCardFrame(Sprite sprite) {
        _cardFrame.sprite = sprite;
    }
    protected override void SetNameFrame(Sprite sprite) {
        _nameFrame.sprite = sprite;
    }
    protected override void SetTypeFrame(Sprite sprite) {
        _typeFrame.sprite = sprite;
    }
    protected override void SetCostFrame(Sprite sprite) {
        _costFrame.sprite = sprite;
    }
    protected override void SetPortrait(Sprite sprite) {
        _portrait.sprite = sprite;
    }
    protected override void SetNameText(string text) {
        _nameText.text = text;
    }
    protected override void SetTypeText(string text) {
        _typeText.text = text;
    }
    protected override void SetCostText(string text) {
        _costText.text = text;
    }
    public override void SetCardText(string text) {
        _cardText.text = text;
    }
}
