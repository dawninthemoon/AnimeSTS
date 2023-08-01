using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardContainer {
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
    public List<CardBase> CardsInExhaustPile {
        get; 
        private set; 
    }
    System.Func<CardInfo, CardBase> _cardCreateCallback;
    private static readonly int MaxHandAmount = 10;
    private int _drawAmount;

    public CardContainer(System.Func<CardInfo, CardBase> cardCreateCallback) {
        CardsInDrawPile = new List<CardBase>();
        CardsInHand = new List<CardBase>();
        CardsInDiscardPile = new List<CardBase>();
        CardsInExhaustPile = new List<CardBase>();

        _cardCreateCallback = cardCreateCallback;
        _drawAmount = 5;
    }

    public void Refresh(CardDeck deck) {
        for (int i = 0; i < deck.CardsInDeck.Count; ++i) {
            CardBase card = _cardCreateCallback.Invoke(deck.CardsInDeck[i]);
            card.gameObject.SetActive(false);
            CardsInDrawPile.Add(card);
        }

        Draw(_drawAmount);
    }

    public void Draw(int amount) {
        amount = Mathf.Min(amount, CardsInDrawPile.Count);
        for (int i = 0; i < amount; ++i) {
            var card = CardsInDrawPile[i];
            CardsInHand.Add(card);
            card.gameObject.SetActive(true);
        }

        for (int i = 0; i < amount; ++i) { 
            CardsInDrawPile.RemoveAt(0);
        }
    }
}
