using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SU.Behaviour;
using SU.Scene;
using UnityEngine.SceneManagement;

namespace SU.Manager
{
    public enum LevelType {
        InitScene = 0,
        LoginScene = 1,
        LoaderScene = 2,
        MainScene = 3,
        BattleScene = 4,
    }

    public class LevelManager : BaseManager
    {
        private LevelType levelType;
        private LoaderScene loaderScene;
        // loader 界面
        private LoaderPanel loaderPanel;

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

        // 初始化第一个场景
        public void InitializeScene()
        {
            levelType = LevelType.InitScene;
            GetScene(levelType.ToString()).OnEnterScene(levelType);
        }

        /// <summary>
        /// 加载关卡场景
        /// </summary>
        /// <param name="level"></param>
        public void LoadLevel(LevelType level)
        {
            levelType = level;

            if (loaderScene == null)
            {
                loaderScene = Game.GetScene<LoaderScene>();
            }

            loaderPanel = panelMgr.CreatePanel(UI.PanelName.LoaderPanel) as LoaderPanel;
            loaderPanel.SetLoaderProgress(0);

            var scene = SceneManager.GetActiveScene();
            LevelType currentLevel = (LevelType)scene.buildIndex;
            StartCoroutine(OnLoadLoaderLevel(currentLevel));
        }
        
        IEnumerator OnLoadLoaderLevel(LevelType level)
        {
            int levelIndex = (int)LevelType.LoaderScene;
            AsyncOperation op = SceneManager.LoadSceneAsync(levelIndex);
            yield return op;
            yield return new WaitForSeconds(0.1f);
            
            GetScene(level.ToString()).OnLeaveScenel(level, delegate ()
            {
                loaderScene.OnEnterScene(level, delegate ()
                {
                    StartCoroutine(OnLoadLevel(levelType));
                });
            });
        }

        IEnumerator OnLoadLevel(LevelType level)
        {
            int displayProgress = 0;
            int toProgress = 0;

            int levelIndex = (int)level;
            AsyncOperation op = SceneManager.LoadSceneAsync(levelIndex);
            op.allowSceneActivation = false;

            while (op.progress < 0.9f)
            {
                toProgress = (int)op.progress * 100;
                while (displayProgress < toProgress)
                {
                    ++displayProgress;
                    loaderPanel.SetLoaderProgress(displayProgress);
                    yield return new WaitForEndOfFrame();
                }
            }

            toProgress = 100;
            while (displayProgress < toProgress)
            {
                ++displayProgress;
                loaderPanel.SetLoaderProgress(displayProgress);
                yield return new WaitForEndOfFrame();
            }
            op.allowSceneActivation = true;

            yield return new WaitForEndOfFrame();

            loaderScene.OnLeaveScenel(levelType, delegate ()
            {
                panelMgr.ClosePanel(UI.PanelName.LoaderPanel);

                GetScene(levelType.ToString()).OnEnterScene(levelType);
            });
        }
    }
}

