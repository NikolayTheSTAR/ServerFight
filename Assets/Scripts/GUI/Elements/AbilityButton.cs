using System;
using TheSTAR.GUI;
using TMPro;
using UnityEngine;

public class AbilityButton : MonoBehaviour
{
    [SerializeField] private PointerButton button;
    [SerializeField] private TextMeshProUGUI stepsText;

    private AbilityType skillType;
    private Action<AbilityType> onClickAction;

    public void Init(AbilityType skillType, Action<AbilityType> onClickAction)
    {
        this.skillType = skillType;
        this.onClickAction = onClickAction;

        button.Init(() =>
        {
            this.onClickAction.Invoke(this.skillType);
        });
    }

    public void Visualize(bool playersTurn, int stepsToUnlock)
    {
        stepsText.gameObject.SetActive(stepsToUnlock > 0);
        stepsText.text = stepsToUnlock.ToString();
        button.SetInteractable(playersTurn && stepsToUnlock == 0);
    }
}