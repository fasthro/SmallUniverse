using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmallUniverse.Behaviour;
using DG.Tweening;
using FairyGUI;
using SmallUniverse.Manager;

namespace SmallUniverse
{
    public class MainGame : GameBehaviour {
        
        protected override void OnAwake()
        {
            base.OnAwake();

            InitGame();
        }

        private void InitGame()
        {
            DontDestroyOnLoad(gameObject);
            
            // 应用基本设置
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Application.targetFrameRate = 30;
            QualitySettings.vSyncCount = 2;
            // 设置屏幕分辨率
            Screen.SetResolution(1136, 640, false);

            // dotween
            DOTween.Init(true, true, LogBehaviour.Default);

            // FairyGUI 设置
            GRoot.inst.SetContentScaleFactor(1136, 640, UIContentScaler.ScreenMatchMode.MatchWidthOrHeight);
            
            // 初始化 BaseBehaviour
            BaseBehaviour.InitBaseBehaviour();

            // 设置初始场景
            Game.GetManager<GSceneManager>().InitializeScene();
        }

        /// <summary>
        /// 进入 main 场景
        /// </summary>
        public void OnEnterMainScene()
        {
            
        }

        /// <summary>
        /// 进入战斗场景
        /// </summary>
        public void OnEnterBattleScene()
        {
            
        }
    }
}

