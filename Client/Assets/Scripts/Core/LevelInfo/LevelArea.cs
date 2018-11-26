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

        public void Initialize(LevelInfo levelInfo, int index, SecurityElement xml)
        {
            this.m_xml = xml;
            this.index = index;
            this.m_levelInfo = levelInfo;

            playerPoints = new List<LevelPoint>();
            monsterPoints = new List<LevelPoint>();
            
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
        public void InitializeDoors(Transform root, SecurityElement _xml)
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

            // 门
            for (int i = 0; i < doors.Count; i++)
            {
                doors[i].LoadDoor();
            }
        }

        /// <summary>
        /// 地面加载完毕
        /// </summary>
        public void OnGroudLoadCompleted()
        {
            m_levelInfo.OnGroudLoadCompleted(this);
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
