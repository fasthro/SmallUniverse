using SU.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SU.Scene
{
    public class BattleScene : BaseScene
    {
        public override void OnEnterScene(LevelType level, Action action = null)
        {
            base.OnEnterScene(level, action);

            Game.mainGame.OnEnterBattleScene();

        }

        public override void OnLeaveScenel(LevelType level, Action action = null)
        {
            base.OnLeaveScenel(level, action);
        }
    }
}
