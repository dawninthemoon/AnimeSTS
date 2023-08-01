using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatUIHandler : MonoBehaviour {
    private struct HpBarContainer {
        public SpriteRenderer fill;
        public TMP_Text text;
        public HpBarContainer(SpriteRenderer fill, TMP_Text text) {
            this.fill = fill;
            this.text = text;
        }
    }

    [SerializeField] private GameObject _hpBarPrefab = null;
    [SerializeField] private Vector2 _hpBarOffset = Vector2.zero;
    private Dictionary<EntityBase, HpBarContainer> _hpBarDictionary;

    public void InitializeUI(EntityBase player, List<EntityBase> enemies) {
        _hpBarDictionary = new Dictionary<EntityBase, HpBarContainer>();

        CreateHPBar(player);
        foreach (EntityBase enemy in enemies) {
            CreateHPBar(enemy);
        }
    }

    private void CreateHPBar(EntityBase entity) {
        Vector2 hpBarPosition = entity.transform.position;
        hpBarPosition += _hpBarOffset;

        var hpBarObject = Instantiate(_hpBarPrefab, hpBarPosition, Quaternion.identity);
        hpBarObject.transform.SetParent(transform);

        SpriteRenderer fill = hpBarObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        TMP_Text text = hpBarObject.GetComponentInChildren<TMP_Text>();
        HpBarContainer hpBarContainer = new HpBarContainer(fill, text);

        _hpBarDictionary.Add(entity, hpBarContainer);
    }
}
