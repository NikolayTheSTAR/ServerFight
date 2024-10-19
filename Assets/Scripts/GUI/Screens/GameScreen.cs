using TMPro;
using UnityEngine;
using Zenject;

namespace TheSTAR.GUI
{
    public class GameScreen : GuiScreen
    {
        [SerializeField] private TextMeshProUGUI turnText;
        [SerializeField] private PointerButton restartBtn;
        [SerializeField] private UnityDictionary<SkillType, SkillButton> skillButtons;

        public void VisualizeGameState(BattleState state)
        {
            turnText.text = state.playersTurn ? "Your\nTurn" : "Enemy's\nTurn";

            foreach (var skillButton in skillButtons.KeyValues)
            {
                if (state.playerState.skillsRecharging.ContainsKey(skillButton.Key))
                {
                    skillButton.Value.Visualize(state.playersTurn, state.playerState.skillsRecharging[skillButton.Key]);
                }
                else skillButton.Value.Visualize(state.playersTurn, 0);
            }
        }
    }
}