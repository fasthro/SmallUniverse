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

        #endregion

        private LineRenderer m_lineRenderer;
        private float m_length;
        private float m_uvAnimateSpeed;

        protected override void OnAwake()
        {
            m_lineRenderer = GetComponent<LineRenderer>();
        }

        public override void Spawn()
        {
            base.Spawn();

            transform.position = firePosition;
            transform.rotation = fireRotation;

            m_length = length;
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
        }
    }
}

