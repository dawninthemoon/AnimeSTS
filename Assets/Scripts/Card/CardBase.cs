using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RieslingUtils;

[System.Serializable]
public struct CommandInfo {
    public string name;
    public string value;
    public CommandInfo(string name, string amount) {
        this.name = name;
        this.value = amount;
    }
}

[System.Serializable]
public struct CardInfo {
    public enum TargetType {
        NONE,
        TARGET
    }

    public enum Rarity {
        BASIC,
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
    
    public string cardID;
    public string cardName;
    public string portraitName;
    public Rarity rarity;
    public Type type;
    public Color color;

    public bool isUpgraded;
    public TargetType targetType;

    public string cost;
    public string upgradeCost;

    public string baseDescription;
    public string upgradeDescription;

    public CommandInfo[] baseCommands;
    public CommandInfo[] upgradeCommands;

    public string variables;
}

public class CardBase : ObserverSubject {
    public CardInfo Info {
        get;
        set;
    }
    private CardSpriteView _cardView;
    public bool IsUpgraded { get; set; }
    public bool MouseOver { get; private set; }
    public bool MouseExit { get; private set; }
    public bool MouseDown { get; private set; }
    public bool MouseUp { get; private set; }
    public static readonly Vector3 DefaultCardScale = new Vector3(0.3f, 0.3f, 1f);
    public static readonly Vector3 HighlightCardScale = new Vector3(0.5f, 0.5f, 1f);

    public void ShowCard(string costText, CommandDataParser parser, EntityBase caster) {
        _cardView.ShowCard(Info, costText, parser, caster);
    }

    public bool NeedTarget() {
        return (Info.targetType == CardInfo.TargetType.TARGET);
    }

    public void Initialize(CardHandler cardHandler, CombatReticle combatReticle) {
        _cardView = GetComponent<CardSpriteView>();
        Attach(cardHandler);
        Attach(combatReticle);
    }

    private void OnMouseOver() {
        MouseOver = true;
        Notify();
        MouseOver = false;
    }

    private void OnMouseDown() {
        MouseDown = true;
        Notify();
        MouseDown = false;
    }

    public void OnMouseExit() {
        MouseExit = true;
        Notify();
        MouseExit = false;
    }

    public void OnMouseUp() {
        MouseUp = true;
        Notify();
        MouseUp = false;
    }

    public void HighlightCard() {
        transform.localPosition = transform.position.ChangeYPos(0f).ChangeZPos(-110f);
        transform.rotation = Quaternion.identity;
        transform.localScale = HighlightCardScale;
    }
}
