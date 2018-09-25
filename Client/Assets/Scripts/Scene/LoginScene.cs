using SU.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SU.Scene
{
    public class LoginScene : BaseScene
    {
        public override void OnEnterScene(LevelType level, Action action = null)
        {
            base.OnEnterScene(level, action);

            //panelMgr.CreatePanel(UI.PanelName.LoginPanel);

            mapMgr.CreattLevelMap("Test");
        }

        public override void OnLeaveScenel(LevelType level, Action action = null)
        {
            base.OnLeaveScenel(level, action);

            //panelMgr.ClosePanel(UI.PanelName.LoginPanel);
        }
    }
}
