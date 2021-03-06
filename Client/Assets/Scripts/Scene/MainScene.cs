﻿using SmallUniverse.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse.Scene
{
    public class MainScene : BaseScene
    {
        public override void OnEnterScene(SceneType level, Action action = null)
        {
            base.OnEnterScene(level, action);

            Game.mainGame.OnEnterMainScene();

            panelMgr.CreatePanel(UI.PanelName.MainPanel);

            levelMgr.InitLevel(100001, 10001);
        }

        public override void OnLeaveScenel(SceneType level, Action action = null)
        {
            base.OnLeaveScenel(level, action);

            panelMgr.ClosePanel(UI.PanelName.MainPanel);
        }
    }
}
