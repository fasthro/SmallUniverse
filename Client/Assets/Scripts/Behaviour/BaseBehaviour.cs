using SU.Scene;
using SU.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SU.Behaviour
{
    public abstract class BaseBehaviour
    {
        // manager 管理字典
        protected static Dictionary<string, BaseManager> Managers = new Dictionary<string, BaseManager>();
        // scene 字典
        protected static Dictionary<string, BaseScene> Scenes = new Dictionary<string, BaseScene>();

        // levelManager
        private static LevelManager _levelMgr = null;
        protected static LevelManager levelMgr {
            get {
                if (_levelMgr == null)
                {
                    _levelMgr = GetManager<LevelManager>();
                }
                return _levelMgr;
            }
        }

        // panelManager
        private static PanelManager _panelMgr = null;
        protected static PanelManager panelMgr
        {
            get
            {
                if (_panelMgr == null)
                {
                    _panelMgr = GetManager<PanelManager>();
                }
                return _panelMgr;
            }
        }

        // resManager
        private static ResManager _resMgr = null;
        protected static ResManager resMgr
        {
            get
            {
                if (_resMgr == null)
                {
                    _resMgr = GetManager<ResManager>();
                }
                return _resMgr;
            }
        }

        public BaseBehaviour()
        {
        }

        //  初始化
        public static void InitBaseBehaviour()
        {
            InitManager();
            InitScene();
        }

        //  初始化Manager
        private static void InitManager()
        {
            AddManager<PanelManager>();
            AddManager<LevelManager>();
            AddManager<ResManager>();
        }

        private static T AddManager<T>() where T : BaseManager, new()
        {
            var type = typeof(T);
            var obj = new T();
            obj.Initialize();
            Managers.Add(type.Name, obj);
            return obj;
        }

        public static T GetManager<T>() where T : class
        {
            var type = typeof(T);
            if (Managers.ContainsKey(type.Name))
            {
                return Managers[type.Name] as T;
            }
            return null;
        }

        public static BaseManager GetManager(string name)
        {
            if (Managers.ContainsKey(name))
            {
                return Managers[name];
            }
            return null;
        }

        // 初始化Ctrl
        private static void InitScene()
        {
            AddScene<InitScene>();
            AddScene<LoaderScene>();
            AddScene<LoginScene>();
            AddScene<MainScene>();
            AddScene<BattleScene>();
        }

        private static T AddScene<T>() where T : BaseScene, new()
        {
            var type = typeof(T);
            var obj = new T();
            Scenes.Add(type.Name, obj);
            return obj;
        }

        public static T GetScene<T>() where T : class
        {
            var type = typeof(T);
            if (Scenes.ContainsKey(type.Name))
            {
                return Scenes[type.Name] as T;
            }
            return null;
        }

        public static BaseScene GetScene(string name)
        {
            if (Scenes.ContainsKey(name))
            {
                return Scenes[name];
            }
            return null;
        }

        // 协同程序
        public Coroutine StartCoroutine(IEnumerator routine)
        {
            return Game.mainGame.StartCoroutine(routine);
        }
    }
}

