using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardContainer {
    public List<CardInfo> CardsInDeck {
        get; 
        private set; 
    }
    public List<CardInfo> CardsInDrawPile {
        get; 
        private set; 
    }
    public List<CardInfo> CardsInHand {
        get; 
        private set; 
    }
    public List<CardInfo> CardsInDiscardPile {
        get; 
        private set; 
    }
    private static readonly int MaxHandAmount = 10;

    public CardContainer() {
        CardsInDeck = new List<CardInfo>();
        CardsInDrawPile = new List<CardInfo>();
        CardsInHand = new List<CardInfo>();
        CardsInDiscardPile = new List<CardInfo>();
    }

    public void Draw() {

    }
}
