using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatUIHandler : MonoBehaviour, IObserver {
    [SerializeField] private CombatEntityStatus _entityStatusPrefab = null;
    [SerializeField] private Vector2 _hpBarOffset = Vector2.zero;
    [SerializeField] private Button _drawPileButton = null, _discardPileButton = null;
    [SerializeField] private CardPileView _cardPileView = null;
    [SerializeField] private TextMeshProUGUI _costText = null;
    private TextMeshProUGUI _drawPileText, _discardPileText;
    private Dictionary<EntityBase, CombatEntityStatus> _entityStatusDictionary;
    private CardContainer _cardContainer;
    private CommandDataParser _parser;
    public bool IsInteractive { get; set; }

    public void InitializeUI(EntityBase player, List<EnemyBase> enemies, CardContainer cardContainer, CommandDataParser parser) {
        _cardContainer = cardContainer;
        _parser = parser;
        _entityStatusDictionary = new Dictionary<EntityBase, CombatEntityStatus>();

        _drawPileText = _drawPileButton.GetComponentInChildren<TextMeshProUGUI>();
        _discardPileText = _discardPileButton.GetComponentInChildren<TextMeshProUGUI>();
        _drawPileButton.onClick.AddListener(OnDrawPileButtonClicked);
        _discardPileButton.onClick.AddListener(OnDiscardPileButtonClicked);

        _cardPileView.Initialize();

        CreateHPBar(player);
        foreach (EntityBase enemy in enemies) {
            CreateHPBar(enemy);
        }
    }

    public void Notify(ObserverSubject subject) {
        EntityBase entity = subject as EntityBase;
        if (entity == null) return;
        UpdateUIStatus(entity);
    }

    private void CreateHPBar(EntityBase entity) {
        Vector2 hpBarPosition = entity.transform.position;
        hpBarPosition += _hpBarOffset;

        var combatEntityStatus = Instantiate(_entityStatusPrefab, hpBarPosition, Quaternion.identity);
        combatEntityStatus.transform.SetParent(transform);

        _entityStatusDictionary.Add(entity, combatEntityStatus);

        UpdateUIStatus(entity);
        entity.Attach(this);
    }

    private void UpdateUIStatus(EntityBase entity) {
        CombatEntityStatus entityStatus = _entityStatusDictionary[entity];
        entityStatus.UpdateHPBarStatus(entity.CurrentHealth, entity.MaxHealth);
        entityStatus.UpdateBlockStatus(entity.Block);
    }

    public void UpdateCardPileUI() {
        int drawPileAmount = _cardContainer.CardsInDrawPile.Count;
        int discardPileAmount = _cardContainer.CardsInDiscardPile.Count;
        
        _drawPileText.text = drawPileAmount.ToString();
        _discardPileText.text = discardPileAmount.ToString();
    }

    public void UpdateCostUI(int currentCost, int maxCost) {
        _costText.text = currentCost.ToString() + "/" + maxCost.ToString();
    }

    private void OnDrawPileButtonClicked() {
        if (IsInteractive)
            _cardPileView.ShowDrawPileView(_cardContainer.CardsInDrawPile);
    }

    private void OnDiscardPileButtonClicked() {
        if (IsInteractive)
            _cardPileView.ShowDiscardPileView(_cardContainer.CardsInDiscardPile);
    }
}
