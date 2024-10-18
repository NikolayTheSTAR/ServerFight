using UnityEngine;

public class UnitVisual : MonoBehaviour
{
    [SerializeField] private HpBar hpBar;

    public void VisualizeUnitState(UnitState state)
    {
        hpBar.Set(state.HP, (float)state.HP / (float)state.MaxHP);
    }
}