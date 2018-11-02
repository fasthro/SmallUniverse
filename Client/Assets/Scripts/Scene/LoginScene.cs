using SmallUniverse.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse.Scene
{
    public class LoginScene : BaseScene
    {
        public override void OnEnterScene(SceneType level, Action action = null)
        {
            base.OnEnterScene(level, action);

            panelMgr.CreatePanel(UI.PanelName.LoginPanel);
        }

        public override void OnLeaveScenel(SceneType level, Action action = null)
        {
            base.OnLeaveScenel(level, action);

            panelMgr.ClosePanel(UI.PanelName.LoginPanel);
        }
    }
}
