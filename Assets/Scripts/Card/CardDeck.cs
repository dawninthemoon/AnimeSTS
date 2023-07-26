using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDeck {
    public List<CardInfo> CardsInDeck {
        get;
        private set;
    }

    public void InitializeDeck() {
        CardsInDeck = new List<CardInfo>();
        CardInfo[] loadedCardInfo = JsonHelper.LoadJsonFile<CardInfo>(Application.dataPath + "/Resources/Cards/CardInfo.json");
        for (int i = 0; i < loadedCardInfo.Length; ++i) {
            for (int j = 0; j < 2; ++j) {
                AddCardInDeck(loadedCardInfo[i]);
            }
        }
    }

    public void AddCardInDeck(CardInfo card) {
        CardsInDeck.Add(card);
    }
}
