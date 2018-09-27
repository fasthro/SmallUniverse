using SU.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SU.Scene
{
    public interface IScene
    {
        void OnEnterScene(SceneType level, Action action);
        void OnLeaveScenel(SceneType level, Action action);
    }
}
