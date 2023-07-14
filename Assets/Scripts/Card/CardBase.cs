using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RieslingUtils;

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

public class CardBase : ObserverSubject {
    [SerializeField] private CardInfo _cardInfo;
    private Sprite _portrait;
    public bool MouseOver { get; private set; }
    public bool MouseExit { get; private set; }
    public bool MouseDown { get; private set; }
    public static readonly Vector3 DefaultCardScale = new Vector3(0.3f, 0.3f);
    public static readonly Vector3 HighlightCardScale = new Vector3(0.5f, 0.5f);

    private void OnMouseOver() {
        MouseOver = true;
        Notify();
        MouseOver = false;
    }

    public void OnMouseExit() {
        MouseExit = true;
        Notify();
        MouseExit = false;
    }

    public void HighlightCard() {
        transform.localPosition = transform.position.ChangeYPos(0f).ChangeZPos(-20f);
        transform.rotation = Quaternion.identity;
        transform.localScale = HighlightCardScale;
    }
}
