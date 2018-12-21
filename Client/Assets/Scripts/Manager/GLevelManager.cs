using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmallUniverse;

namespace SmallUniverse.Manager
{
    public class GLevelManager : BaseManager
    {
        // 关卡
        public LevelInfo levelInfo;
        // 英雄
        public Hero hero;
        // 英雄相机
        public VirtualCamera heroCamera;
        // 关卡环境
        public LevelEnvironment environment;

        // 区域索引
        private int areaIndex;

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

        public void InitLevel(string levelName, string heroAssetPath)
        {
            levelInfo = LevelInfo.Create(levelName);
            hero = Hero.Create(heroAssetPath);
            heroCamera = Game.gameCamera.heroCamera;

            // 默认区域索引
            areaIndex = 1;

            // 相机初始化
            heroCamera.Initialize();

            // 环境相关
            environment = new LevelEnvironment();
            // 天空盒
            Game.gameCamera.SetSkybox(environment.skybox);
            
            // event
            levelInfo.OnLoadedAreaHandler += OnLoadedAreaHandler;
            levelInfo.OnEnterAreaHandler += OnEnterAreaHandler;
            levelInfo.OnStayAreaHandler += OnStayAreaHandler;
            levelInfo.OnExitAreaHandler += OnExitAreaHandler;

            // init
            levelInfo.InitEnvironment(environment);
        }

        #region  event
        /// <summary>
        /// 区域加载完成
        /// </summary>
        private void OnLoadedAreaHandler(LevelArea area)
        {
            Debug.Log("OnLoadedAreaHandler - > " + area.index);

            if (area.index == 1)
            {
                var points = levelInfo.GetHeroPoints(areaIndex);
                // 玩家出生
                hero.Born(points[0]);
                
                // 关卡设置hero transform
                levelInfo.SetHeroTransform(hero.actorGameObject.transform);

                // 设置相机
                heroCamera.SetLookAt(hero.actorGameObject.transform);

                // 摇杆初始化
                Game.virtualJoy.Initialize();
            }
        }

        /// <summary>
        /// 进入区域
        /// </summary>
        private void OnEnterAreaHandler(LevelArea area)
        {
            Debug.Log("OnEnterAreaHandler - > " + area.index);
        }

        /// <summary>
        /// 保持在区域
        /// </summary>
        private void OnStayAreaHandler(LevelArea area)
        {
            Debug.Log("OnStayAreaHandler - > " + area.index);
        }

        /// <summary>
        /// 离开区域
        /// </summary>
        private void OnExitAreaHandler(LevelArea area)
        {
            Debug.Log("OnExitAreaHandler - > " + area.index);
        }

        #endregion
    }
}

