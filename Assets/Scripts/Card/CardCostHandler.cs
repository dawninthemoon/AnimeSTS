using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCostHandler : MonoBehaviour {
    public class CardCostConfig {
        string type;
        string data;
    }
    private Dictionary<string, CardCostConfig> _cardCostConfigDictionary;

    private void Awake() {
        _cardCostConfigDictionary = new Dictionary<string, CardCostConfig>();
    }

    public bool CanUseCard(int currentCost, CardBase cardBase) {
        CardCostConfig config = GetCostConfig(cardBase);
        return false;
    }

    public int GetRequireCost(int currentCost, CardBase cardBase) {
        return 0;
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
        switch (data[0]) {
        case "d":
            break;
        case "x":
            break;
        case "c":
            break;
        }
        return new CardCostConfig();
    }
}
