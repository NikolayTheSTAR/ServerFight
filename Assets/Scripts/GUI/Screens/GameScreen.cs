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
    }
}