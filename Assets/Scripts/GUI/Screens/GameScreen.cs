using TMPro;
using UnityEngine;
using Zenject;

namespace TheSTAR.GUI
{
    public class GameScreen : GuiScreen
    {
        [SerializeField] private TextMeshProUGUI turnText;
        [SerializeField] private PointerButton restartBtn;
        [SerializeField] private UnityDictionary<AbilityType, AbilityButton> skillButtons;

        private IGameClient client;

        [Inject]
        private void Construct(IGameClient client)
        {
            this.client = client;
        }

        public override void Init()
        {
            base.Init();

            foreach (var skillButton in skillButtons.KeyValues)
            {
                skillButton.Value.Init(skillButton.Key, OnSkillButtonClick);
            }

            restartBtn.Init(OnRestartButtonClick);
        }

        public void VisualizeGameState(BattleState state)
        {
            turnText.text = state.playersTurn ? "Your\nTurn" : "Enemy's\nTurn";

            foreach (var skillButton in skillButtons.KeyValues)
            {
                if (state.playerState.abilitiesRecharging.ContainsKey(skillButton.Key))
                {
                    skillButton.Value.Visualize(state.playersTurn, state.playerState.abilitiesRecharging[skillButton.Key]);
                }
                else skillButton.Value.Visualize(state.playersTurn, 0);
            }
        }

        private void OnSkillButtonClick(AbilityType skillType)
        {
            client.SendPlayerActionToServer($"ability_{skillType}");
        }

        private void OnRestartButtonClick()
        {
            client.SendPlayerActionToServer("restart");
        }
    }
}