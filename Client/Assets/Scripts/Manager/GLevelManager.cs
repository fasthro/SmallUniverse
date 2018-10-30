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

        public void InitLevel(string _levelName, string _heroName)
        {
            levelInfo = LevelInfo.Create(_levelName);
            hero = Hero.Create(_heroName);
            heroCamera = Game.gameCamera.heroCamera;

            areaIndex = 1;

            // 初始化环境
            levelInfo.InitEnvironment(new LevelEnvironment());

            
            var points = levelInfo.GetPlayerPoints(areaIndex);
            // 玩家出生
            hero.Born(points[0]);

            // 设置相机
            heroCamera.Initialize();
            heroCamera.SetLookAt(hero.heroTransform.head);
        }
    }
}

