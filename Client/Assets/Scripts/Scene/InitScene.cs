﻿using SmallUniverse.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse.Scene
{
    public class InitScene : BaseScene
    {
        public override void OnEnterScene(SceneType level, Action action = null)
        {
            base.OnEnterScene(level, action);

            // 游戏启动，热更新就在此场景中执行

            // 执行完毕逻辑直接进入登录场景
            sceneMgr.LoadLevel(SceneType.LoginScene);
        }

        public override void OnLeaveScenel(SceneType level, Action action = null)
        {
            base.OnLeaveScenel(level, action);
        }
    }
}
