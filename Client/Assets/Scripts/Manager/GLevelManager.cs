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

        public void InitLevel(string levelName, string heroName, string heroResName)
        {
            levelInfo = LevelInfo.Create(levelName);
            hero = Hero.Create(heroName, heroResName);
            heroCamera = Game.gameCamera.heroCamera;

            // 默认区域索引
            areaIndex = 1;

            // 相机初始化
            heroCamera.Initialize();

            // 环境相关
            environment = new LevelEnvironment();

            levelInfo.OnGroudLoadCompletedHandler += OnGroudLoadCompletedHandler;
            levelInfo.InitEnvironment(environment);
        }

        /// <summary>
        /// 区域地面加载完成
        /// </summary>
        private void OnGroudLoadCompletedHandler(LevelArea area)
        {
            Debug.Log("OnGroudLoadCompletedHandler -> area.index : " + area.index);
            
            if (area.index == 1)
            {
                var points = levelInfo.GetPlayerPoints(areaIndex);
                // 玩家出生
                hero.Born(points[0]);

                // 设置相机
                heroCamera.SetLookAt(hero.actorGameObject.transform);

                // 摇杆初始化
                Game.virtualJoy.Initialize();
            }
        }
    }
}

