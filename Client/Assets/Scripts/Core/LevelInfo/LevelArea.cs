using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using SmallUniverse.Behaviour;

namespace SmallUniverse
{
    public class LevelArea : MonoBehaviour
    {
        // 关卡信息
        private LevelInfo levelInfo;
        // xml 数据
        private SecurityElement xml;
        // 区域索引
        public int index;
        // 地面
        public LevelGround ground;
        // 玩家出生点
        public List<LevelPoint> playerPoints;
        // 怪出生点
        public List<LevelPoint> monsterPoints;

        public void Initialize(LevelInfo _levelInfo, int _index, SecurityElement _xml)
        {
            xml = _xml;
            index = _index;
            levelInfo = _levelInfo;
            
            playerPoints = new List<LevelPoint>();
            monsterPoints = new List<LevelPoint>();

            Transform root = null;

            foreach (SecurityElement xmlChild in xml.Children)
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

        /// <summary>
        /// 初始化区域环境
        /// </summary>
        /// <param name="_environment"></param>
        public void InitEnvironment(LevelEnvironment _environment)
        {
            // 地面
            ground.LoadGrid(LevelAnimationType.None);
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
        /// 获取 points
        /// </summary>
        /// <param name="_xml"></param>
        private List<LevelPoint> GetPoints(SecurityElement _xml)
        {
            List<LevelPoint> points = new List<LevelPoint>();
            if(_xml.Children != null)
            {
                foreach (SecurityElement xmlChild in _xml.Children)
                {
                    LevelPoint point= new LevelPoint(xmlChild);
                    points.Add(point);
                }
            }
            return points;
        }
    }
}
