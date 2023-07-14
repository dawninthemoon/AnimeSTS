using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RieslingUtils;

public class CombatUIHandler : MonoBehaviour, IObserver {
    private Vector3 _mouseOffset;
    private CardBase _selectedCard;
    private SpriteRenderer[] _reticleBlocks;
    private SpriteRenderer _reticleArrow;

    private void Start() {
        var blockPrefab = Resources.Load<SpriteRenderer>("Combat/reticleBlock");
        var arrowPrefab = Resources.Load<SpriteRenderer>("Combat/reticleArrow");

        _reticleBlocks = new SpriteRenderer[15];
        for (int i = 0; i < _reticleBlocks.Length; ++i) {
            _reticleBlocks[i] = Instantiate(blockPrefab, transform);
        }
        _reticleArrow = Instantiate(arrowPrefab, transform);
        SetReticleActive(false);
    }

    private void Update() {
        if (_selectedCard) {
            if (_selectedCard.Info.needTarget) {
                ChangeReticleColor();
                MoveReticle();
            }
            else {
                MoveCard();
            }
        }
    }

    public void Notify(ObserverSubject subject) {
        CardBase card = subject as CardBase;
        
        if (card.MouseDown) {
            _selectedCard = card;
            _mouseOffset = card.transform.position - ExVector.GetMouseWorldPosition();
            if (card.Info.needTarget) {
                SetReticleActive(true);
                card.transform.position = card.transform.position.ChangeXPos(0f);
            }
        }

        if (card.MouseUp) {
            _selectedCard = null;
            SetReticleActive(false);
        }
    }

    private void SetReticleActive(bool active) {
        for (int i = 0; i < _reticleBlocks.Length; ++i) {
            _reticleBlocks[i].gameObject.SetActive(active);
        }
        _reticleArrow.gameObject.SetActive(active);
    }

    private void MoveReticle() {
        Vector3 mousePosition = ExVector.GetMouseWorldPosition();
        int numOfBlocks = _reticleBlocks.Length;
        float rotationZ;

        _reticleBlocks[0].transform.position = _selectedCard.transform.position;
        for (int i = 1; i < numOfBlocks; ++i) {
            float t = (float)i / (numOfBlocks);
            Vector3 p0 = new Vector3(0f, 5f);
            Vector3 blockPosition = Bezier.GetPoint(_selectedCard.transform.position, p0, mousePosition, t);
            _reticleBlocks[i].transform.position = blockPosition;

            rotationZ = ExMath.GetDegreeBetween(_reticleBlocks[i - 1].transform.position, blockPosition) - 90f;
            _reticleBlocks[i - 1].transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        }

        rotationZ = ExMath.GetDegreeBetween(_reticleBlocks[numOfBlocks - 1].transform.position, mousePosition) - 90f;
        _reticleBlocks[numOfBlocks - 1].transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        _reticleArrow.transform.position = mousePosition;
        _reticleArrow.transform.rotation = _reticleBlocks[numOfBlocks - 1].transform.rotation;
    }

    private void ChangeReticleColor() {
        Color reticleColor = (BattleRoom.SelectedEnemy) ? Color.red : Color.white;
        int numOfBlocks = _reticleBlocks.Length;
        for (int i = 0; i < numOfBlocks; ++i) {
            _reticleBlocks[i].color = reticleColor;
        }
        _reticleArrow.color = reticleColor;
    }

    private void MoveCard() {
        Vector3 mousePoint = ExVector.GetMouseWorldPosition();
        _selectedCard.transform.position = mousePoint + _mouseOffset;
    }
}
