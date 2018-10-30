using SmallUniverse.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse.Scene
{
    public interface IScene
    {
        void OnEnterScene(SceneType level, Action action);
        void OnLeaveScenel(SceneType level, Action action);
    }
}
