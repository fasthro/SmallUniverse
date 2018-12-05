using SmallUniverse.Behaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmallUniverse.Manager;

namespace SmallUniverse
{
    public class Game {
        
        // 游戏管理对象
        private static GameObject m_manager = null;
        public static GameObject manager {
            get {
                if (m_manager == null)
                {
                    m_manager = GameObject.FindWithTag("MainGame");
                }
                return m_manager;
            }
        }

        // main game
        private static MainGame m_mainGame = null;
        public static MainGame mainGame {
            get {
                if (m_mainGame == null)
                {
                    m_mainGame = manager.GetComponent<MainGame>();
                }
                return m_mainGame;
            }
        }

        // game camera
        private static GameCamera m_gameCamera = null;
        public static GameCamera gameCamera {
            get {
                if (m_gameCamera == null)
                {
                    m_gameCamera = mainGame.transform.Find("GameCamera").GetComponent<GameCamera>();
                }
                return m_gameCamera;
            }
        }

        // virtual joy
        private static VirtualJoy m_virtualJoy = null;
        public static VirtualJoy virtualJoy {
            get {
                if (m_virtualJoy == null)
                {
                    m_virtualJoy = mainGame.transform.Find("VirtualJoy").GetComponent<VirtualJoy>();
                }
                return m_virtualJoy;
            }
        }

        // game curve
        private static GameCurve m_gameCurve = null;
        public static GameCurve gameCurve {
            get {
                if (m_gameCurve == null)
                {
                    m_gameCurve = mainGame.transform.Find("GameCurve").GetComponent<GameCurve>();
                }
                return m_gameCurve;
            }
        }

        // game pool
        private static GamePool m_gamePool = null;
        public static GamePool gamePool {
            get {
                if (m_gamePool == null)
                {
                    m_gamePool = mainGame.transform.Find("GamePool").GetComponent<GamePool>();
                }
                return m_gamePool;
            }
        }

        // hero
        private static Hero m_hero = null;
        public static Hero hero{
            get{
                return GetManager<GLevelManager>().hero;
            }
        }

        // 获取Scene
        public static T GetScene<T>() where T : class
        {
            return BaseBehaviour.GetScene<T>();
        }

        // 获取manager
        public static T GetManager<T>() where T : class
        {
            return BaseBehaviour.GetManager<T>();
        }
    }


}
