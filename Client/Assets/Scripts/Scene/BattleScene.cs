using SmallUniverse.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse.Scene
{
    public class BattleScene : BaseScene
    {
        public override void OnEnterScene(SceneType level, Action action = null)
        {
            base.OnEnterScene(level, action);

            Game.mainGame.OnEnterBattleScene();

            panelMgr.CreatePanel(UI.PanelName.HudPanel);

            levelMgr.InitLevel(100001, 10003);
        }

        public override void OnLeaveScenel(SceneType level, Action action = null)
        {
            base.OnLeaveScenel(level, action);
        }
    }
}
