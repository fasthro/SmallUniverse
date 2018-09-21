using SU.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SU.Scene
{
    public class MainScene : BaseScene
    {
        public override void OnEnterScene(LevelType level, Action action = null)
        {
            base.OnEnterScene(level, action);

            Game.mainGame.OnEnterMainScene();

            panelMgr.CreatePanel(UI.PanelName.MainPanel);
        }

        public override void OnLeaveScenel(LevelType level, Action action = null)
        {
            base.OnLeaveScenel(level, action);

            panelMgr.ClosePanel(UI.PanelName.MainPanel);
        }
    }
}
