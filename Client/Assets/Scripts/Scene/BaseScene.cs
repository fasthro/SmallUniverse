using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SU.Behaviour;
using System;
using SU.Manager;

namespace SU.Scene
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

