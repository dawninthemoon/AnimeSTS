using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardContainer {
    public List<CardBase> CardsInDeck {
        get; 
        private set; 
    }
    public List<CardBase> CardsInDrawPile {
        get; 
        private set; 
    }
    public List<CardBase> CardsInHand {
        get; 
        private set; 
    }
    public List<CardBase> CardsInDiscardPile {
        get; 
        private set; 
    }
    private static readonly int MaxHandAmount = 10;

    public CardContainer() {
        CardsInDeck = new List<CardBase>();
        CardsInDrawPile = new List<CardBase>();
        CardsInHand = new List<CardBase>();
        CardsInDiscardPile = new List<CardBase>();
    }

    public void Draw() {

    }
}
