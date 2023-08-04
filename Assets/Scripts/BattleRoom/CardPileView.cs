using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPileView : MonoBehaviour {
    [SerializeField] private RectTransform _scrollViewContent = null;
    private CardImageView _cardImagePrefab;

    public void Initialize() {
        _cardImagePrefab = ResourceManager.GetInstance().GetAsset<CardImageView>("Cards/CardImage");
    }

    public void ShowDrawPileView(List<CardBase> cardsInDrawPile, CommandDataParser parser) {
        gameObject.SetActive(true);
        ShowCards(cardsInDrawPile, parser);
    }

    public void ShowDiscardPileView(List<CardBase> cardsInDiscardPile) {
        //ShowCards(cardsInDiscardPile);
    }

    private void ShowCards(List<CardBase> cardsInPile, CommandDataParser parser) {
        foreach (CardBase card in cardsInPile) {
            var cardImage = Instantiate(_cardImagePrefab, _scrollViewContent);
            cardImage.ShowCard(card.Info, card.Info.cost, parser, null);
        }
    }
}
