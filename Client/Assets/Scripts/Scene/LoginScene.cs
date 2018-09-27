using SU.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SU.Scene
{
    public class LoginScene : BaseScene
    {
        public override void OnEnterScene(SceneType level, Action action = null)
        {
            base.OnEnterScene(level, action);

            //panelMgr.CreatePanel(UI.PanelName.LoginPanel);

            levelMgr.CreateMap("Test");
        }

        public override void OnLeaveScenel(SceneType level, Action action = null)
        {
            base.OnLeaveScenel(level, action);

            //panelMgr.ClosePanel(UI.PanelName.LoginPanel);
        }
    }
}
