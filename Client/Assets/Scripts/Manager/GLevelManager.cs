using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SU.Manager
{
    public class GLevelManager : BaseManager
    {
        // 关卡
        public LevelInfo levelInfo;

        // 角色

        // 相机

        public override void Initialize()
        {
        }

        public override void OnFixedUpdate(float deltaTime)
        {
        }

        public override void OnUpdate(float deltaTime)
        {
        }

        public override void OnDispose()
        {
        }

        /// <summary>
        /// 创建关卡
        /// </summary>
        /// <param name="levelName"></param>
        public void CreateLevel(string levelName)
        {
            GameObject go = new GameObject();
            go.name = "LevelInfo";
            levelInfo = go.AddComponent<LevelInfo>();
            levelInfo.Initialize(levelName);
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        public void CreateCharacter()
        {

        }

        /// <summary>
        /// 创建相机
        /// </summary>
        public void CreateCamera()
        {
            
        }
    }
}

