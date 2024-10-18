using UnityEngine;
using Zenject;

namespace TheSTAR.GUI
{
    public class GameScreen : GuiScreen
    {
        private GuiController gui;

        [Inject]
        private void Construct(GuiController gui)
        {
            this.gui = gui;
        }

        protected override void OnShow()
        {
            base.OnShow();

            Debug.Log("OnShow GameScreen");
        }
    }
}