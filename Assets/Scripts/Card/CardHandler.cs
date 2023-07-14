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

    private void Start() {
        _cardContainer = new CardContainer();

        CardBase defendPrefab = Resources.Load<CardBase>("Cards/Defend");
        CardBase strikePrefab = Resources.Load<CardBase>("Cards/Strike");
        for (int i = 0; i < handCount; ++i) {
            var prefab = (i < 5) ? defendPrefab : strikePrefab;
            CardBase card = Instantiate(prefab, transform);
            card.Initialize(this, _combatUIHandler);
            _cardContainer.CardsInHand.Add(card);
        }

        AlignCards();
    }

    [Inject]
    private void Initialize(CombatUIHandler uiHandler) {
        _combatUIHandler = uiHandler;
    }

    public void AlignCards(){
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
            t.localPosition = new Vector3(xPos, yPos, -cardIndex);
            t.localScale = CardBase.DefaultCardScale;
            t.rotation = Quaternion.Euler(0f, 0f, rotZ);
        }
    }

    public void Notify(ObserverSubject subject) {
        CardBase card = subject as CardBase;
        
        if (card.MouseDown) {
            //_selectedCard = card;
            //_mouseOffset = card.transform.position - ExVector.GetMouseWorldPosition();
        }
        else if (card.MouseOver) {
            card.HighlightCard();
        }
        
        if (card.MouseUp) {
            _selectedCard = null;
            AlignCards();
        }
        else if (card.MouseExit) {
            _selectedCard = null;
            AlignCards();
        }
    }
}
