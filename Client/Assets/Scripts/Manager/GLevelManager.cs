using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SU.Level;

namespace SU.Manager
{
    public class GLevelManager : BaseManager
    {
        // 关卡地图
        public Map map;

        // 角色

        // 相机
        public CameraControler cameraController;

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
        /// 创建地图
        /// </summary>
        /// <param name="mapName"></param>
        public void CreateMap(string mapName)
        {
            GameObject go = new GameObject();
            go.name = "MapRoot";
            map = go.AddComponent<Map>();
            map.Initialize(mapName);
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        public void CreateCharacter()
        {

        }

        /// <summary>
        /// 创建相机
        /// </summary>
        public void CreateCamera()
        {
            cameraController = new CameraControler();
            cameraController.Initialize();
        }
    }
}

