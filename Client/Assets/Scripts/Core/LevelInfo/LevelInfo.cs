﻿using Mono.Xml;
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

    public enum LevelAnimationType
    {
        None,               // 无
    }

    public class LevelInfo : MonoBehaviour
    {
        // 关卡名称
        private string m_levelName;
        // xml 数据
        private SecurityElement m_xml;
        // 区域数量
        private int m_areaCount;
        // 区域列表
        private LevelArea[] m_areas;
        
        // navMesh Surface
        public NavMeshSurface navMeshSurface;

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
            m_xml = LoadXml();

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

            navMeshSurface = gameObject.AddComponent<NavMeshSurface>();
            navMeshSurface.collectObjects = CollectObjects.Children;
            navMeshSurface.layerMask = 1 << LayerMask.NameToLayer(LevelFunctionType.Ground.ToString());
        }

        public void InitEnvironment(LevelEnvironment environment)
        {
            for(int i = 0; i < m_areas.Length; i++)
            {
                m_areas[i].InitEnvironment(environment);
            }

            navMeshSurface.BuildNavMesh();
        }

        /// <summary>
        /// 获取当前区域内玩家出生点
        /// </summary>
        public List<LevelPoint> GetPlayerPoints(int areaIndex)
        {
            if(areaIndex <= m_areaCount)
                return m_areas[areaIndex - 1].playerPoints;
            return new List<LevelPoint>();
        }

        /// <summary>
        /// 获取当前区域内怪物出生点
        /// </summary>
        public List<LevelPoint> GetMonsterPoints(int areaIndex)
        {
            if(areaIndex <= m_areaCount)
                return m_areas[areaIndex - 1].monsterPoints;
            return new List<LevelPoint>();
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
        ///  加载场景配置
        /// </summary>
        /// <returns></returns>
        private SecurityElement LoadXml()
        {
            // 加载配置
            var resMgr = Game.GetManager<GResManager>();
            var bundleName = "levels/scenes/" + m_levelName.ToLower();
            var bundle = resMgr.LoadAssetBundle(bundleName);
            var textAsset = bundle.LoadAsset(m_levelName + ".xml") as TextAsset;

            SecurityParser parser = new SecurityParser();
            parser.LoadXml(textAsset.text);

            resMgr.UnLoadAssetBundle(bundleName);

            return parser.ToXml();
        }
    }
}


