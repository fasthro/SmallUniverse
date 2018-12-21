using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.AI;

namespace SmallUniverse
{
    public class LevelArea : MonoBehaviour
    {
        // 关卡信息
        private LevelInfo m_levelInfo;
        // xml 数据
        private SecurityElement m_xml;
        // 区域索引
        public int index;
        // 地面
        public LevelGround ground;
        // 玩家出生点
        public List<LevelPoint> playerPoints;
        // 怪出生点
        public List<LevelPoint> monsterPoints;
        // 门
        public List<LevelDoor> doors;
        // 门的状态
        private LevelDoorState m_doorState;
        public LevelDoorState doorState
        {
            get
            {
                return m_doorState;
            }
        }

        #region hero
        // hero transform
        private Transform m_heroTransform;

        // hero position
        private Vector3 m_heroPosition;

        // hero 所在的格子
        private LevelGrid m_heroInGrid;

        // hero in area state
        private LevelStateMachine m_heroInAreaState;
        #endregion

        public void Initialize(LevelInfo levelInfo, int index, SecurityElement xml)
        {
            this.m_xml = xml;
            this.index = index;
            this.m_levelInfo = levelInfo;

            playerPoints = new List<LevelPoint>();
            monsterPoints = new List<LevelPoint>();

            m_heroInAreaState = LevelStateMachine.Exit;

            Transform root = null;

            foreach (SecurityElement xmlChild in m_xml.Children)
            {
                if (xmlChild.Tag == LevelFunctionType.Ground.ToString().ToLower())
                {
                    root = CreateRoot(transform, LevelFunctionType.Ground.ToString());
                    ground = root.gameObject.AddComponent<LevelGround>();
                    ground.Initialize(this, xmlChild);
                }
                else if (xmlChild.Tag == LevelFunctionType.Decoration.ToString().ToLower())
                {

                }
                else if (xmlChild.Tag == LevelFunctionType.Door.ToString().ToLower())
                {
                    root = CreateRoot(transform, LevelFunctionType.Door.ToString());
                    InitializeDoors(root, xmlChild);
                }
                else if (xmlChild.Tag == LevelFunctionType.Trap.ToString().ToLower())
                {

                }
                else if (xmlChild.Tag == LevelFunctionType.Transfer.ToString().ToLower())
                {

                }
                else if (xmlChild.Tag == LevelFunctionType.Player.ToString().ToLower())
                {
                    playerPoints = GetPoints(xmlChild);
                }
                else if (xmlChild.Tag == LevelFunctionType.Monster.ToString().ToLower())
                {
                    monsterPoints = GetPoints(xmlChild);
                }
            }
        }

        // 初始化门
        private void InitializeDoors(Transform root, SecurityElement _xml)
        {
            doors = new List<LevelDoor>();
            if (_xml.Children != null)
            {
                foreach (SecurityElement xmlChild in _xml.Children)
                {
                    var door = root.gameObject.AddComponent<LevelDoor>();
                    door.Initialize(this, xmlChild);
                    doors.Add(door);
                }
            }
        }

        /// <summary>
        /// 初始化区域环境
        /// </summary>
        /// <param name="environment"></param>
        public void InitEnvironment(LevelEnvironment environment)
        {
            // 地面
            ground.InitEnvironment(environment);

            // 加载门
            for (int i = 0; i < doors.Count; i++)
            {
                doors[i].LoadDoor();
            }

            // 门初始化的时候为关闭
            SetDoorState(LevelDoorState.Close);
        }

        #region  hero

        // <summary>
        /// 设置 hero transform
        /// </summary>
        /// <param name="heroTransform"></param>
        public void SetHeroTransform(Transform heroTransform)
        {
            m_heroTransform = heroTransform;
        }

        // <summary>
        /// 检查 hero 是否在此区域内
        /// </summary>
        public bool CheckHeroInArea()
        {
            if (m_heroTransform != null)
            {
                var hp = m_heroTransform.position;
                m_heroPosition.x = (int)hp.x;
                m_heroPosition.y = (int)hp.y;
                m_heroPosition.z = (int)hp.z;

                m_heroInGrid = ground.GetGrid(m_heroPosition);
            }
            else
            {
                m_heroInGrid = null;
            }
            return m_heroInGrid != null;
        }

        // <summary>
        /// 检查 hero 在此区域状态
        /// </summary>
        public void CheckHeroInAreaState()
        {
            bool inArea = CheckHeroInArea();
            if (inArea)
            {
                if(m_heroInAreaState == LevelStateMachine.Exit)
                {
                    OnEnterArea();
                }
                
                if(m_heroInAreaState == LevelStateMachine.Enter)
                {
                    OnStayArea();
                }
            }
            else
            {
                if(m_heroInAreaState == LevelStateMachine.Stay)
                {
                   OnExitArea();
                }
            }
        }

        #endregion

        public void OnUpdate()
        {
            CheckHeroInAreaState();
        }

        #region event

        // <summary>
        /// 区域加载完毕
        /// </summary>
        public void OnLoadedArea()
        {
            m_levelInfo.OnLoadedArea(this);
        }

        // <summary>
        /// 进入区域
        /// </summary>
        public void OnEnterArea()
        {
            m_heroInAreaState = LevelStateMachine.Enter;
            m_levelInfo.OnEnterArea(this);
        }

        // <summary>
        /// 保持在区域
        /// </summary>
        public void OnStayArea()
        {
            m_heroInAreaState = LevelStateMachine.Stay;
            m_levelInfo.OnStayArea(this);
        }

        // <summary>
        /// 离开区域
        /// </summary>
        public void OnExitArea()
        {
            m_heroInAreaState = LevelStateMachine.Exit;
            m_levelInfo.OnExitArea(this);
        }

        #endregion

        /// <summary>
        /// 设置门的状态
        /// </summary>
        /// <param name="state"></param>
        public void SetDoorState(LevelDoorState state)
        {
            m_doorState = state;
            for (int i = 0; i < doors.Count; i++)
            {
                doors[i].SetState(state);
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
            var root = parent.Find(name);
            if (root == null)
            {
                GameObject go = new GameObject();
                go.name = name;
                go.transform.parent = parent;
                root = go.transform;
            }
            return root;
        }

        /// <summary>
        /// 获取 points
        /// </summary>
        /// <param name="_xml"></param>
        private List<LevelPoint> GetPoints(SecurityElement _xml)
        {
            List<LevelPoint> points = new List<LevelPoint>();
            if (_xml.Children != null)
            {
                foreach (SecurityElement xmlChild in _xml.Children)
                {
                    LevelPoint point = new LevelPoint(xmlChild);
                    points.Add(point);
                }
            }
            return points;
        }
    }
}
