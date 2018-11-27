using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class LevelEnvironment
    {
        // 天空盒
        public string skybox = "Space_Nebula_Red";

        #region ground animation param
        // 动画等待间隔时长
        public float animationWaitTime = 0.05f;
        // 动画总时长
        public float animationTotalTime = 0.5f;
        // 动画偏移
        public Vector3 animationOffset = new Vector3(0, -1, 0);

        #endregion
    }
}

