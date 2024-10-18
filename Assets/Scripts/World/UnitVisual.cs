using System.Collections.Generic;
using UnityEngine;
using TheSTAR.Utility;

public class UnitVisual : MonoBehaviour
{
    [SerializeField] private HpBar hpBar;
    [SerializeField] private Transform effectsParent;
    [SerializeField] private UnitEffectUI effectUiPrefab;

    private List<UnitEffectUI> createdEffectUi = new();
    private readonly ResourceHelper<BattleConfig> battleConfig = new("Configs/BattleConfig");

    public void VisualizeUnitState(UnitState state)
    {
        // hp UI
        hpBar.Set(state.hp, (float)state.hp / (float)state.maxHp);

        // effect UI
        int i = 0;
        for (; i < state.effects.Length; i++)
        {
            if (createdEffectUi.Count <= i)
            {
                var newEffectUI = Instantiate(effectUiPrefab, effectsParent);
                newEffectUI.Set(
                    battleConfig.Get.Effects[(int)state.effects[i].effectType].Icon, 
                    state.effects[i].value);
                createdEffectUi.Add(newEffectUI);
            }
            else
            {
                createdEffectUi[i].gameObject.SetActive(true);
                createdEffectUi[i].Set(
                    battleConfig.Get.Effects[(int)state.effects[i].effectType].Icon, 
                    state.effects[i].value);
            }
        }

        for (; i < createdEffectUi.Count; i++)
        {
            createdEffectUi[i].gameObject.SetActive(false);
        }
    }
}