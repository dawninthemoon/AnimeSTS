using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RieslingUtils;

public class CardHandler : MonoBehaviour, IObserver {
    [SerializeField] private AnimationCurve _alignCurve = null;
    [SerializeField] private BoxCollider2D _cardCancelArea = null;
    [SerializeField] private float _offsetX = 1f;
    [SerializeField] private float _offsetY = 0.1f;
    [SerializeField] private float _offsetRotation = 5f;
    private CombatReticle _combatReticle;
    public CardContainer CardContainer { 
        get; 
        set; 
    }
    [SerializeField] private int handCount = 0;
    private Vector3 _mouseOffset;
    private CardBase _selectedCard;
    private System.Func<CardBase, bool> _cardUseCallback;
    private System.Action<CardBase> _redrawCardCallback;

    public void Initialize() {
        _combatReticle = GetComponent<CombatReticle>();
        CardContainer = new CardContainer(CreateCard);
    }

    public void InitializeBattle(GameData data) {
        CardContainer.Refresh(data.Deck);
        AlignCards();
    }

    public void SetCallback(System.Func<CardBase, bool> cardUseCallback, System.Action<CardBase> redrawCardCallback) {
        _cardUseCallback = cardUseCallback;
        _redrawCardCallback = redrawCardCallback;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            CardContainer.Draw(1);
            AlignCards();
        }
    }

    public void AlignCards() {
        int cardCount = CardContainer.CardsInHand.Count;

        for (int cardIndex = 0; cardIndex < cardCount; ++cardIndex) {
            float alignAmount = (cardCount == 1) ? 0.5f : (float)cardIndex / (cardCount - 1);
            
            float maxX = _offsetX * cardCount * 0.5f;
            float maxY = _offsetY * cardCount * 0.5f;
            float maxRotation = _offsetRotation * cardCount * 0.5f;

            float xPos = Mathf.Lerp(-maxX, maxX, alignAmount);
            float yPos = -Mathf.Abs(Mathf.Lerp(-maxY, maxY, _alignCurve.Evaluate(alignAmount)));
            float rotZ = Mathf.Lerp(maxRotation, -maxRotation, alignAmount);

            Transform t = CardContainer.CardsInHand[cardIndex].transform;
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
        if (_selectedCard == null) {
            return;
        }

        if (!MouseUtils.IsMouseOverCollider(_cardCancelArea) && (!_selectedCard.NeedTarget() || BattleRoom.SelectedEnemy)) {
            if (_cardUseCallback.Invoke(_selectedCard)) {
                CardContainer.CardsInHand.Remove(_selectedCard);
                CardContainer.CardsInDiscardPile.Add(_selectedCard);

                Destroy(_selectedCard.gameObject);
            }
        }
        _selectedCard = null;
    }

    private CardBase CreateCard(CardInfo cardInfo) {
        var cardPrefab = ResourceManager.GetInstance().GetAsset<CardBase>("Cards/CardBase");

        CardBase cardInstance = Instantiate(cardPrefab, transform);
        cardInstance.Initialize(this, _combatReticle);
        cardInstance.Info = cardInfo;

        _redrawCardCallback.Invoke(cardInstance);

        return cardInstance;
    }
}
