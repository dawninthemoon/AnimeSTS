using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardView : MonoBehaviour {
    [SerializeField] private SpriteRenderer _cardFrame = null;
    [SerializeField] private SpriteRenderer _portrait = null;
    [SerializeField] private SpriteRenderer _nameFrame = null;
    [SerializeField] private SpriteRenderer _typeFrame = null;
    [SerializeField] private SpriteRenderer _costFrame = null;
    [SerializeField] private TextMeshPro _cardText = null;
    private TextMeshPro _nameText = null;
    private TextMeshPro _typeText = null;
    private TextMeshPro _costText = null;
    private static readonly string CardResourcesPath = "Cards/CardUI/";
    private static readonly string PortraitPath = "Cards/Portraits/";
    private static readonly string NameFrameString = "blanket";
    private static readonly string CostFrameString = "cost";
    private static readonly string[] CardTypeStringArr = {
        "attack",
        "skill",
        "power",
    };
    private static readonly string[] KoreanCardTypeStringArr = {
        "공격",
        "스킬",
        "파워"
    };
    private static readonly string[] ColorStringArr = {
        "red",
        "green"
    };
    private static readonly string[] RarityStringArr = {
        "_common",
        "_common",
        "_uncommon",
        "_rare"
    };

    private void Awake() {
        _nameText = _nameFrame.GetComponentInChildren<TextMeshPro>();
        _typeText = _typeFrame.GetComponentInChildren<TextMeshPro>();
        _costText = _costFrame.GetComponentInChildren<TextMeshPro>();
    }

    public void ShowCardFrame(CardInfo.Color color, CardInfo.Rarity rarity, CardInfo.Type type) {
        var resourceManager = ResourceManager.GetInstance();
        
        string cardFramePath = CardResourcesPath + CardTypeStringArr[(int)type] + "_" + ColorStringArr[(int)color];
        _cardFrame.sprite = resourceManager.GetSpriteByCache(cardFramePath);

        string nameFramePath = NameFrameString + RarityStringArr[(int)rarity];
        _nameFrame.sprite = resourceManager.GetSpriteByCache(nameFramePath);

        string typeFramePath = CardTypeStringArr[(int)type] + RarityStringArr[(int)rarity];
        _typeFrame.sprite = resourceManager.GetSpriteByCache(typeFramePath);

        string costFramePath = CostFrameString + "_" + ColorStringArr[(int)color];
        _costFrame.sprite = resourceManager.GetSpriteByCache(costFramePath);
    }

    public void ShowCardData(string cardName, CardInfo.Color color, CardInfo.Type type, string portraitName) {
        var resourceManager = ResourceManager.GetInstance();

        _nameText.text = cardName;
        _typeText.text = KoreanCardTypeStringArr[(int)type];

        string portraitPath = PortraitPath + ColorStringArr[(int)color] + CardTypeStringArr[(int)type] + portraitName;
        _portrait.sprite = resourceManager.GetSpriteByCache(portraitPath);
    }

    public void ShowCardText(string text) {
        _cardText.text = text;
    }
}
