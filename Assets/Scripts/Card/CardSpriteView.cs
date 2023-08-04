using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardSpriteView : CardView {
    [SerializeField] private SpriteRenderer _cardFrame = null;
    [SerializeField] private SpriteRenderer _portrait = null;
    [SerializeField] private SpriteRenderer _nameFrame = null;
    [SerializeField] private SpriteRenderer _typeFrame = null;
    [SerializeField] private SpriteRenderer _costFrame = null;
    [SerializeField] private TextMeshPro _cardText = null;
    [SerializeField] private TextMeshPro _nameText = null;
    [SerializeField] private TextMeshPro _typeText = null;
    [SerializeField] private TextMeshPro _costText = null;
    
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
