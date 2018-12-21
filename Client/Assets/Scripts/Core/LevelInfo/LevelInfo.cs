using Mono.Xml;
using SmallUniverse.Behaviour;
using SmallUniverse.Manager;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.AI;

namespace SmallUniverse
{
    public enum LevelFunctionType
    {
        Ground,              // 地面
        Player,                // 玩家
        Monster,            // 怪
        Door,                 // 门
        Trap,                  // 陷阱
        Transfer,            // 传送门
        Decoration,       // 装饰品
    }

    // 动画类型
    public enum LevelAnimationType
    {
        None,               // 无
        Volatility,         // 波动
    }

    // 门的状态
    public enum LevelDoorState
    {
        Open,              // 开启
        Close,             // 关闭
    }

    // 通用状态
    public enum LevelStateMachine
    {
        Enter,
        Stay,
        Exit,
    }

    public class LevelInfo : MonoBehaviour
    {
        #region event define

        // 区域加载完成
        public delegate void LoadedAreaHandler(LevelArea area);
        public event LoadedAreaHandler OnLoadedAreaHandler;

        // 进入/保持/离开区域
        public delegate void EnterAreaHandler(LevelArea area);
        public delegate void StayAreaHandler(LevelArea area);
        public delegate void ExitAreaHandler(LevelArea area);
        public event EnterAreaHandler OnEnterAreaHandler;
        public event StayAreaHandler OnStayAreaHandler;
        public event ExitAreaHandler OnExitAreaHandler;

        #endregion

        #region base

        // 关卡名称
        private string m_levelName;
        // xml 数据
        private SecurityElement m_xml;
        // 区域数量
        private int m_areaCount;
        // 区域列表
        private LevelArea[] m_areas;

        // navMesh Surface
        private NavMeshSurface m_navMeshSurface;

        #endregion
        
        // hero transform
        private Transform m_heroTransform;

        #region public
        
        #endregion

        /// <summary>
        /// 创建关卡信息
        /// </summary>
        /// <param name="parent"></param>
        public static LevelInfo Create(string levelName)
        {
            GameObject go = new GameObject();
            go.name = "LevelInfo";
            var levelInfo = go.AddComponent<LevelInfo>();
            levelInfo.Initialize(levelName);
            return levelInfo;
        }

        private void Initialize(string levelName)
        {
            m_levelName = levelName;

            LoadLevelConfig();

            m_areaCount = int.Parse(m_xml.Attribute("area_count"));
            m_areas = new LevelArea[m_areaCount];

            // 初始化关卡区域
            foreach (SecurityElement xmlChild in m_xml.Children)
            {
                var areaIndex = int.Parse(xmlChild.Attribute("index"));
                var areaTrans = CreateRoot(transform, "Area_" + areaIndex);
                var area = areaTrans.gameObject.AddComponent<LevelArea>();
                area.Initialize(this, areaIndex, xmlChild);
                m_areas[areaIndex - 1] = area;
            }
        }

        /// <summary>
        /// 创建节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private Transform CreateRoot(Transform parent, string name)
        {
            GameObject go = new GameObject();
            go.name = name;
            go.transform.parent = parent;
            return go.transform;
        }

        /// <summary>
        ///  加载关卡配置
        /// </summary>
        /// <returns></returns>
        private void LoadLevelConfig()
        {
            // 加载配置
            var resMgr = Game.GetManager<GResManager>();
            var bundleName = "levels/scenes/" + m_levelName.ToLower();
            var bundle = resMgr.LoadAssetBundle(bundleName);

            var mapTextAsset = bundle.LoadAsset("Map.xml") as TextAsset;
            SecurityParser parser = new SecurityParser();
            parser.LoadXml(mapTextAsset.text);
            m_xml = parser.ToXml();

            // navmesh
            var navmeshPrefab = bundle.LoadAsset("Navmesh.prefab") as GameObject;
            var navGo = GameObject.Instantiate(navmeshPrefab) as GameObject;
            navGo.transform.position = Vector3.zero;
            m_navMeshSurface = navGo.GetComponent<NavMeshSurface>();
            m_navMeshSurface.layerMask = 1 << LayerMask.NameToLayer(LevelFunctionType.Ground.ToString());

            resMgr.UnLoadAssetBundle(bundleName);
        }

        void Update()
        {
            for (int i = 0; i < m_areas.Length; i++)
            {
                m_areas[i].OnUpdate();
            }
        }

        // <summary>
        /// 初始化关卡环境
        /// </summary>
        public void InitEnvironment(LevelEnvironment environment)
        {
            for (int i = 0; i < m_areas.Length; i++)
            {
                m_areas[i].InitEnvironment(environment);
            }
        }

        // <summary>
        /// 设置 hero transform
        /// </summary>
        /// <param name="heroTransform"></param>
        public void SetHeroTransform(Transform heroTransform)
        {
            m_heroTransform = heroTransform;
            for (int i = 0; i < m_areas.Length; i++)
            {
                m_areas[i].SetHeroTransform(heroTransform);
            }
        }

        #region event
        
        // <summary>
        /// 区域加载完毕
        /// </summary>
        public void OnLoadedArea(LevelArea area)
        {
            if(OnLoadedAreaHandler != null)
            {
                OnLoadedAreaHandler(area);
            }
        }

        // <summary>
        /// 进入区域
        /// </summary>
        public void OnEnterArea(LevelArea area)
        {
            if(OnEnterAreaHandler != null)
            {
                OnEnterAreaHandler(area);
            }
        }

        // <summary>
        /// 保持在此区域
        /// </summary>
        public void OnStayArea(LevelArea area)
        {
            if(OnStayAreaHandler != null)
            {
                OnStayAreaHandler(area);
            }
        }

        // <summary>
        /// 离开区域加载
        /// </summary>
        public void OnExitArea(LevelArea area)
        {
            if(OnExitAreaHandler != null)
            {
                OnExitAreaHandler(area);
            }
        }

        #endregion

        #region point

        /// <summary>
        /// 获取当前区域内Hero出生点
        /// </summary>
        public List<LevelPoint> GetHeroPoints(int areaIndex)
        {
            if (areaIndex <= m_areaCount)
                return m_areas[areaIndex - 1].playerPoints;
            return new List<LevelPoint>();
        }

        /// <summary>
        /// 获取当前区域内怪物出生点
        /// </summary>
        public List<LevelPoint> GetMonsterPoints(int areaIndex)
        {
            if (areaIndex <= m_areaCount)
                return m_areas[areaIndex - 1].monsterPoints;
            return new List<LevelPoint>();
        }

        #endregion
    }
}


