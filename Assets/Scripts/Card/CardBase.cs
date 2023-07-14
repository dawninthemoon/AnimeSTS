using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CardEffect {
    public enum Type {
        DAMAGE,
        BLOCK,
    }
    public Type type;
    public string amount;
}

[System.Serializable]
public struct CardInfo {
    public enum Rarity {
        STARTER,
        COMMON,
        UNCOMMON,
        RARE
    }

    public enum Type {
        ATTACK,
        SKILL,
        POWER
    }

    public enum Color {
        RED,
        GREEN
    }
    
    public string cardName;
    public Rarity rarity;
    public Type type;
    public Color color;

    public bool isUpgraded;
    public bool needTarget;

    public int cost;
    public int upgradeCost;

    [TextArea] public string baseDescription;
    [TextArea] public string upgradeDescription;

    public CardEffect[] baseEffects;
    public CardEffect[] upgradeEffects;

    [TextArea]
    public string variables;
}

public class CardBase : MonoBehaviour {
    [SerializeField] private CardInfo _cardInfo;
    private Sprite _portrait;

    private void OnMouseOver() {
        
    }
}
