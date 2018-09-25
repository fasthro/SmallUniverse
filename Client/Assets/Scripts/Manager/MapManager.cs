using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SU.Level;

namespace SU.Manager
{
    public class MapManager : BaseManager
    {
        private Map map;
        private Transform mapTransform;

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

        /// <summary>
        /// 创建关卡地图
        /// </summary>
        /// <param name="mapName"></param>
        public void CreattLevelMap(string mapName)
        {
            GameObject go = new GameObject();
            go.name = "MapRoot";
            mapTransform = go.transform;
            map = go.AddComponent<Map>();
            map.Initialize(mapName);
        }
    }
}

