using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmallUniverse.Behaviour;
using System;
using SmallUniverse.Manager;

namespace SmallUniverse.Scene
{
    public abstract class BaseScene : BaseBehaviour, IScene
    {
        public virtual void OnEnterScene(SceneType level, Action action = null)
        {
            if (action != null)
            {
                action();
            }
        }
        public virtual void OnLeaveScenel(SceneType level, Action action = null) {
            if (action != null)
            {
                action();
            }
        }
    }
}

