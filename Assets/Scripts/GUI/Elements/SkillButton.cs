using System;
using TheSTAR.GUI;
using TMPro;
using UnityEngine;

public class SkillButton : MonoBehaviour
{
    [SerializeField] private PointerButton button;
    [SerializeField] private TextMeshProUGUI stepsText;

    private SkillType skillType;
    private Action<SkillType> onClickAction;

    public void Init(SkillType skillType, Action<SkillType> onClickAction)
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
        button.SetInteractable(playersTurn && stepsToUnlock <= 0);
    }
}