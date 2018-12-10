using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public enum BulletType
    {
        Normal,          // 普通子弹
        Spray,           // 喷射子弹
        Line,            // 线子弹
    }

    public abstract class BulletBase : MonoBehaviour
    {
        #region config
        // 子弹类型
        public BulletType bulletType;
        // 子弹速度
        public float speed;
        // 生命时间
        public float lifeTime;
        // 枪口效果资源路径
        public string muzzleAssetPath;
        // 影响效果资源路径
        public string impactAssetPath;
        #endregion

        #region public

        // 子弹发射位置 
        [HideInInspector]
        public Vector3 firePosition;
        // 子弹发射方向                            
        [HideInInspector]
        public Quaternion fireRotation;
        // 是否已经出生
        //[HideInInspector]
        protected bool IsSpawn;
        // 是否已经回收
        //[HideInInspector]
        protected bool IsDespawn;
        #endregion

        // 生命时间
        protected float m_lifeTime;

        public virtual void Initialize()
        {
            IsSpawn = false;
            IsDespawn = false;
        }

        public virtual void Spawn()
        {
            if (!IsSpawn)
            {
                IsSpawn = true;
                IsDespawn = false;
                m_lifeTime = 0;
            }
        }

        public virtual void Despawn()
        {
            if (IsSpawn && !IsDespawn)
            {
                IsDespawn = true;
                m_lifeTime = 0;

                // 缓存池回收
                gameObject.Despawn();
            }
        }

        /// <summary>
        /// 设置子弹旋转
        /// </summary>
        public virtual void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }

        /// <summary>
        /// 设置子弹位置
        /// </summary>
        public virtual void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        protected virtual void OnAwake()
        {

        }

        protected virtual void OnUpdate()
        {

        }

        void Awake()
        {
            OnAwake();
        }

        void Update()
        {
            OnUpdate();
        }
    }
}
