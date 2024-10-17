using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheSTAR.GUI
{
    public class LoadScreen : GuiScreen
    {
        private Action delayAction;

        private float LoadDelay = 0.5f;

        public void Init(Action delayAction)
        {
            this.delayAction = delayAction;
            Invoke(nameof(DoDelayAction), LoadDelay);
        }

        private void DoDelayAction()
        {
            delayAction?.Invoke();
        }
    }
}