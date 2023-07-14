using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHandler : MonoBehaviour {
    [SerializeField] private AnimationCurve _alignCurve = null;
    [SerializeField] private float _offsetX = 1f;
    [SerializeField] private float _offsetY = 0.1f;
    [SerializeField] private float _offsetRotation = 5f;
    private CardContainer _cardContainer;
    [SerializeField] private int handCount = 0;

    void Start() {
        _cardContainer = new CardContainer();

        CardBase p = Resources.Load<CardBase>("Cards/Defend");
        for (int i = 0; i < handCount; ++i) {
            CardBase a = Instantiate(p, transform);
            _cardContainer.CardsInHand.Add(a);
        }

        AlignCards();
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
            t.localPosition = new Vector3(xPos, yPos);
            t.rotation = Quaternion.Euler(0f, 0f, rotZ);
        }
    }
}
