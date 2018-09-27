using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SU.Level
{
    public class MapGrid
    {
        // 资源名称
        public string assetName;
        // bundle 名称
        public string buneldName;
        // 所在层
        public int layer;
        // 位置
        public Vector3 position;
        // 旋转
        public Vector3 rotationAngle;

        // 对应的物体
        public GameObject gameObject;
        public Transform transform;
        public Vector3 localPosition;

        private Tween tween;
        private Vector3 tweenPosition;

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize(GameObject _go)
        {
            gameObject = _go;
            transform = gameObject.transform;
            localPosition = transform.localPosition;

            Vector3 start = new Vector3(localPosition.x, localPosition.y + layer + 1, localPosition.z);
            
            tween = DOTween.To(() => start, x => tweenPosition = x, localPosition, 0.2f * layer);
            tween.onUpdate = OnUpdate;
            tween.PlayForward();
        }

        private void OnUpdate()
        {
            transform.localPosition = tweenPosition;
        }
    }
}
