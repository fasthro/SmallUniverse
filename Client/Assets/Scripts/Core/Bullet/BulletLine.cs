using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    [RequireComponent(typeof(LineRenderer))]
    public class BulletLine : BulletBase
    {
        #region config
        // 子弹长度
        public float length;
        
        // 是否有UV动画
        public bool uvAnimate;
        // UV 动画速度
        public Vector2 uvAnimateSpeed;
        // 开始点资源路径
        public string beginPointAssetPath;
        // 结束点资源路径
        public string endPointAssetPath;

        #endregion

        private LineRenderer m_lineRenderer;
        private float m_length;
        private float m_uvAnimateSpeed;
        private Transform m_begin;
        private Transform m_end;

        protected override void OnAwake()
        {
            m_lineRenderer = GetComponent<LineRenderer>();
        }

        public override void Spawn(AttackData attackData, ActorBase target)
        {
            base.Spawn(attackData, target);

            transform.position = firePosition;
            transform.rotation = fireRotation;

            m_length = length;

            // begin effect
            if(m_begin == null && !string.IsNullOrEmpty(beginPointAssetPath))
            {
                m_begin = Game.gamePool.Spawn(beginPointAssetPath, gameObject.transform).transform;
                m_begin.localPosition = Vector3.zero;
            }

            // end effect
            if(m_end == null && !string.IsNullOrEmpty(endPointAssetPath))
            {
                m_end = Game.gamePool.Spawn(endPointAssetPath, gameObject.transform).transform;
                m_end.localPosition = Vector3.zero;
            }
        }

        
        private void UpdateUVAnimate()
        {
            m_uvAnimateSpeed += Time.deltaTime;
            
            if(m_uvAnimateSpeed > 1f)
            {
                m_uvAnimateSpeed = 0f;
            }

            m_lineRenderer.material.SetTextureOffset("_MainTex", m_uvAnimateSpeed * uvAnimateSpeed);
        }

        protected override void OnUpdate()
        {
            if(uvAnimate)
            {
                UpdateUVAnimate();
            }

            m_lineRenderer.SetPosition(1, new Vector3(0f, 0f, m_length));

            if(m_end != null)
            {
               m_end.position = transform.position + transform.forward * m_length;
            }
        }
    }
}

