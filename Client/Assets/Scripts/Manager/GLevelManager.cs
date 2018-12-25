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
         // 怪
        public Monster monster;
        // 英雄相机
        public VirtualCamera heroCamera;
        // 关卡环境
        public LevelEnvironment environment;

        // 区域索引
        private int areaIndex;

        private CSV_Level m_levelCSV;

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

        public void InitLevel(int levelId, int heroId)
        {
            // 关卡数据配置
            m_levelCSV = Game.gameCSV.GetData<CSV_Level>(levelId);
            
            levelInfo = LevelInfo.Create(m_levelCSV.scene);
            hero = Hero.Create(heroId);
            heroCamera = Game.gameCamera.heroCamera;

            // 默认区域索引
            areaIndex = 1;

            // 相机初始化
            heroCamera.Initialize();

            // 环境相关
            environment = new LevelEnvironment();
            // 天空盒
            Game.gameCamera.SetSkybox(m_levelCSV.skybox);
            
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

            CreateMonster(100001);
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

        #region moster
        private void CreateMonster(int monsterId)
        {
            monster = Monster.Create(monsterId);
            var points = levelInfo.GetMonsterPoints(areaIndex);
            monster.Born(points[0]);
        }
        #endregion
    }
}

