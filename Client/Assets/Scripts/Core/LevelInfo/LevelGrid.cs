using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SmallUniverse.Behaviour;

namespace SmallUniverse
{
    public class LevelGrid : GameBehaviour
    {
        // 资源名称
        public string assetName;
        // bundle 名称
        public string bundleName;
        // 位置
        public Vector3 position;
        // 旋转
        public Vector3 rotationAngle;
        // 功能
        public LevelFunctionType function;

        // 格子实例物体
        private GameObject gridGameObject;
        private Transform gridTransform;

        protected override void OnAwake()
        {
            base.OnAwake();
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        public void Initialize()
        {
            
        }

        public void LoadAsset()
        {
            var prefab = LevelAsset.GetGameObject(bundleName, assetName);
            gridGameObject = GameObject.Instantiate<GameObject>(prefab);
            gridTransform = gridGameObject.transform;
            gridTransform.position = position;
            gridTransform.parent = transform;
            gridTransform.localEulerAngles = rotationAngle;
            gridGameObject.AddComponent<BoxCollider>();
        }
    }

}
