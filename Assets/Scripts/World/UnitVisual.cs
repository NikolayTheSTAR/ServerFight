using UnityEngine;

public class UnitVisual : MonoBehaviour
{
    [SerializeField] private HpBar hpBar;

    public void VisualizeUnitState(UnitState state)
    {
        hpBar.Set(state.hp, (float)state.hp / (float)state.maxHp);
    }
}