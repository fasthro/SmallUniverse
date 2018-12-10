using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SmallUniverse.Behaviour;

namespace SmallUniverse
{
    public class LevelGrid : MonoBehaviour
    {
        // id
        public string id;
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
        // 相邻 x 正方向格子id
        public string adjacentPx;
         // 相邻 x 负方向格子id
        public string adjacentNx;
         // 相邻 z 正方向格子id
        public string adjacentPz;
         // 相邻 z 负方向格子id
        public string adjacentNz;

        // 所在层
        public int layer;

        // 格子实例物体
        private GameObject m_gridGameObject;
        private Transform m_gridTransform;

        // 动画
        public LevelGridAnimation animation;

        public void Initialize()
        {
            this.id = GetId(position);
            gameObject.layer = layer;
        }

        public void LoadAsset(bool isShow = true)
        {
            var prefab = LevelAsset.GetGameObject(bundleName, assetName);
            m_gridGameObject = GameObject.Instantiate<GameObject>(prefab);
            m_gridTransform = m_gridGameObject.transform;
            m_gridTransform.position = position;
            m_gridTransform.parent = transform;
            m_gridTransform.localEulerAngles = rotationAngle;
            m_gridGameObject.AddComponent<BoxCollider>();
            gameObject.GetComponentInChildren<MeshRenderer>().enabled = isShow;
            animation = m_gridGameObject.AddComponent<LevelGridAnimation>();

            GameUtils.SetGameObjectLayer(m_gridGameObject, layer);
        }

        /// <summary>
        /// 获取Id
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static string GetId(Vector3 pos)
        {
            return string.Format("{0}|{1}|{2}", pos.x, pos.y, pos.z);
        }
    }

}
