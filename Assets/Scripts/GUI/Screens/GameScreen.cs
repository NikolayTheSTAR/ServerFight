using UnityEngine;
using Zenject;

namespace TheSTAR.GUI
{
    public class GameScreen : GuiScreen
    {
        [SerializeField] private PointerButton restartBtn;
        [SerializeField] private UnityDictionary<SkillType, PointerButton> skillButtons;

        protected override void OnShow()
        {
            base.OnShow();

            Debug.Log("OnShow GameScreen");
        }
    }
}