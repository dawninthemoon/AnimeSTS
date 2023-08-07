using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardPileView : MonoBehaviour {
    [SerializeField] private RectTransform _scrollViewContent = null;
    [SerializeField] private Button _undoButton = null;
    [SerializeField] private GameObject _drawPileBanner = null, _discardPileBanner = null;
    private CardImageView _cardImagePrefab;
    private CommandDataParser _parser;
    private List<CardImageView> _currentCardList;

    public void Initialize() {
        _parser = new CommandDataParser();
        _currentCardList = new List<CardImageView>();
        _cardImagePrefab = ResourceManager.GetInstance().GetAsset<CardImageView>("Cards/CardImage");
        _undoButton.onClick.AddListener(CloseCardPileView);
    }

    public void ShowDrawPileView(List<CardBase> cardsInDrawPile) {
        _drawPileBanner.SetActive(true);
        ShowCards(cardsInDrawPile);
    }

    public void ShowDiscardPileView(List<CardBase> cardsInDiscardPile) {
        _discardPileBanner.SetActive(true);
        ShowCards(cardsInDiscardPile);
    }

    private void ShowCards(List<CardBase> cardsInPile) {
        gameObject.SetActive(true);

        foreach (CardBase card in cardsInPile) {
            var cardImage = Instantiate(_cardImagePrefab, _scrollViewContent);
            cardImage.ShowCard(card.Info, card.Info.cost, _parser, null);
            _currentCardList.Add(cardImage);
        }
    }

    private void CloseCardPileView() {
        _drawPileBanner.SetActive(false);
        _discardPileBanner.SetActive(false);
        gameObject.SetActive(false);

        for (int i = 0; i < _currentCardList.Count; ++i) {
            Destroy(_currentCardList[i].gameObject);
            _currentCardList.RemoveAt(i--);
        }
    }
}
