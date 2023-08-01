using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RieslingUtils;

public class CombatUIHandler : MonoBehaviour, IObserver {
    [SerializeField] private CombatEntityStatus _entityStatusPrefab = null;
    [SerializeField] private Vector2 _hpBarOffset = Vector2.zero;
    private Dictionary<EntityBase, CombatEntityStatus> _entityStatusDictionary;

    public void InitializeUI(EntityBase player, List<EntityBase> enemies) {
        _entityStatusDictionary = new Dictionary<EntityBase, CombatEntityStatus>();

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
}
