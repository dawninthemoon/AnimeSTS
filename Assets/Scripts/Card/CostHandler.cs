using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostHandler : MonoBehaviour {
    public struct CardCostConfig {
        public string type;
        public string data;
        public CardCostConfig(string type, string data) {
            this.type = type;
            this.data = data;
        }
    }
    private Dictionary<string, CardCostConfig> _cardCostConfigDictionary;
    private static readonly string DecimalTypeID = "d";
    private static readonly string XTypeID = "x";
    private static readonly string ConditionTypeID = "c";
    public int CurrentCost {
        get;
        set;
    }
    public int MaxCost {
        get;
        set;
    }

    private void Awake() {
        _cardCostConfigDictionary = new Dictionary<string, CardCostConfig>();
        MaxCost = 3;
    }

    public void ChargeCost() {
        CurrentCost = MaxCost;
    }

    public bool TryUseCard(CardBase cardBase) {
        int requireCost = GetRequireCost(cardBase);
        bool canUseCard = CurrentCost >= requireCost;
        if (canUseCard) {
            CurrentCost -= requireCost;
        }
        return canUseCard;
    }

    public int GetRequireCost(CardBase cardBase) {
        int requireCost = 0;
        CardCostConfig config = GetCostConfig(cardBase);
        if (config.type.Equals(DecimalTypeID)) {
            requireCost = int.Parse(config.data);
        }
        else if (config.type.Equals(XTypeID)) {
            requireCost = CurrentCost;
        }
        else if (config.type.Equals(ConditionTypeID)) {
            requireCost = 0;
        }
        return requireCost;
    }

    private CardCostConfig GetCostConfig(CardBase cardBase) {
        CardCostConfig value;
        if (!_cardCostConfigDictionary.TryGetValue(cardBase.Info.cardName, out value)) {
            value = Parse(cardBase);
            _cardCostConfigDictionary.Add(cardBase.Info.cardName, value);
        }
        return value;
    }

    private CardCostConfig Parse(CardBase cardBase) {
        string costData = cardBase.IsUpgraded ? cardBase.Info.cost : cardBase.Info.upgradeCost;
        string[] data = costData.Split(':');
        return new CardCostConfig(data[0], data[1]);
    }
}
