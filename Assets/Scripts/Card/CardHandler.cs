using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CardHandler : MonoBehaviour, IObserver {
    [SerializeField] private AnimationCurve _alignCurve = null;
    [SerializeField] private float _offsetX = 1f;
    [SerializeField] private float _offsetY = 0.1f;
    [SerializeField] private float _offsetRotation = 5f;
    private CombatUIHandler _combatUIHandler;
    private CardContainer _cardContainer;
    [SerializeField] private int handCount = 0;
    private Vector3 _mouseOffset;
    private CardBase _selectedCard;
    private System.Action<CardInfo> _cardUseCallback;
    private System.Action<CardBase> _redrawCardCallback;

    private void Start() {
        _cardContainer = new CardContainer(CreateCard);
    }

    [Inject]
    private void Initialize(CombatUIHandler uiHandler) {
        _combatUIHandler = uiHandler;
    }

    public void InitializeBattle(GameData data) {
        _cardContainer.Refresh(data.Deck);
        AlignCards();
    }

    public void SetCallback(System.Action<CardInfo> cardUseCallback, System.Action<CardBase> redrawCardCallback) {
        _cardUseCallback = cardUseCallback;
        _redrawCardCallback = redrawCardCallback;
    }

    public void AlignCards() {
        int cardCount = _cardContainer.CardsInHand.Count;

        for (int cardIndex = 0; cardIndex < cardCount; ++cardIndex) {
            float alignAmount = (float)cardIndex / (cardCount - 1);
            
            float maxX = _offsetX * cardCount * 0.5f;
            float maxY = _offsetY * cardCount * 0.5f;
            float maxRotation = _offsetRotation * cardCount * 0.5f;

            float xPos = Mathf.Lerp(-maxX, maxX, alignAmount);
            float yPos = -Mathf.Abs(Mathf.Lerp(-maxY, maxY, _alignCurve.Evaluate(alignAmount)));
            float rotZ = Mathf.Lerp(maxRotation, -maxRotation, alignAmount);

            Transform t = _cardContainer.CardsInHand[cardIndex].transform;
            t.localPosition = new Vector3(xPos, yPos, -cardIndex * 10f);
            t.localScale = CardBase.DefaultCardScale;
            t.rotation = Quaternion.Euler(0f, 0f, rotZ);
        }
    }

    public void Notify(ObserverSubject subject) {
        CardBase card = subject as CardBase;
        
        if (card.MouseDown) {
            _selectedCard = card;
        }
        else if (!_selectedCard && card.MouseOver) {
            card.HighlightCard();
        }
        
        if (card.MouseUp) {
            UseCard();
            AlignCards();
        }
        else if (!_selectedCard && card.MouseExit) {
            AlignCards();
        }
    }

    private void UseCard() {
        if (_selectedCard == null)
            return;
        if (_selectedCard.NeedTarget() && !BattleRoom.SelectedEnemy)
            return;
        
        _cardContainer.CardsInHand.Remove(_selectedCard);
        _cardContainer.CardsInDiscardPile.Add(_selectedCard);

        _cardUseCallback.Invoke(_selectedCard.Info);
        Destroy(_selectedCard.gameObject);
        _selectedCard = null;
    }

    private CardBase CreateCard(CardInfo cardInfo) {
        var cardPrefab = ResourceManager.GetInstance().GetAsset<CardBase>("Cards/CardBase");

        CardBase cardInstance = Instantiate(cardPrefab, transform);
        cardInstance.Initialize(this, _combatUIHandler);
        cardInstance.Info = cardInfo;

        _redrawCardCallback.Invoke(cardInstance);

        return cardInstance;
    }
}
