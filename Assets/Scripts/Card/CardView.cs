using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class CardView : MonoBehaviour {
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

    public void ShowCard(CardInfo info, string costText, CommandDataParser parser, EntityBase caster) {
        ShowCardFrame(info.color, info.rarity, info.type);
        ShowCardData(info.cardName, info.color, info.type, info.portraitName, costText);

        string description = info.baseDescription;
        string variables = info.variables;

        if (description == "") return;
        
        var variableData = parser.ParseVariable(variables, caster);
        foreach (KeyValuePair<string, int> variable in variableData) {
            string formatString = '{' + variable.Key + '}';
            description = description.Replace(formatString, variable.Value.ToString());
        }

        SetCardText(description);
    }

    private void ShowCardFrame(CardInfo.Color color, CardInfo.Rarity rarity, CardInfo.Type type) {
        var resourceManager = ResourceManager.GetInstance();
        
        string cardFramePath = CardResourcesPath + CardTypeStringArr[(int)type] + "_" + ColorStringArr[(int)color];
        SetCardFrame(resourceManager.GetSpriteByCache(cardFramePath));

        string nameFramePath = CardResourcesPath + NameFrameString + RarityStringArr[(int)rarity];
        SetNameFrame(resourceManager.GetSpriteByCache(nameFramePath));

        string typeFramePath = CardResourcesPath + CardTypeStringArr[(int)type] + RarityStringArr[(int)rarity];
        SetTypeFrame(resourceManager.GetSpriteByCache(typeFramePath));

        string costFramePath = CardResourcesPath + CostFrameString + "_" + ColorStringArr[(int)color];
        SetCostFrame(resourceManager.GetSpriteByCache(costFramePath));
    }

    private void ShowCardData(string cardName, CardInfo.Color color, CardInfo.Type type, string portraitName, string cost) {
        var resourceManager = ResourceManager.GetInstance();

        SetNameText(cardName);
        SetTypeText(KoreanCardTypeStringArr[(int)type]);

        string portraitPath = PortraitPath + ColorStringArr[(int)color] + "/" + CardTypeStringArr[(int)type] + "/" + portraitName;
        SetPortrait(resourceManager.GetSpriteByCache(portraitPath));

        SetCostText(cost);
    }

    protected abstract void SetCardFrame(Sprite sprite);
    protected abstract void SetNameFrame(Sprite sprite);
    protected abstract void SetTypeFrame(Sprite sprite);
    protected abstract void SetCostFrame(Sprite sprite);
    protected abstract void SetPortrait(Sprite sprite);
    protected abstract void SetNameText(string text);
    protected abstract void SetTypeText(string text);
    protected abstract void SetCostText(string text);
    public abstract void SetCardText(string text);
}
