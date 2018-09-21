using SU.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SU.Scene
{
    public class InitScene : BaseScene
    {
        public override void OnEnterScene(LevelType level, Action action = null)
        {
            base.OnEnterScene(level, action);

            // 游戏启动，热更新就在此场景中执行

            // 执行完毕逻辑直接进入登录场景
            levelMgr.LoadLevel(LevelType.LoginScene);
        }

        public override void OnLeaveScenel(LevelType level, Action action = null)
        {
            base.OnLeaveScenel(level, action);
        }
    }
}
