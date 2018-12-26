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
        
        #endregion

        // 是否已经出生
        protected bool m_isSpawn;
        // 是否已经回收
        protected bool m_isDespawn;
        // 生命时间
        protected float m_lifeTime;
        // 是否已经造成伤害
        protected bool m_isDamage;

        // 攻击数据
        protected AttackData m_attackData;
        // 被攻击目标
        protected ActorBase m_target;

        public virtual void Initialize()
        {
            m_isSpawn = false;
            m_isDespawn = false;
        }

        public virtual void Spawn(AttackData attackData, ActorBase target)
        {
            if (!m_isSpawn)
            {
                m_attackData = attackData;
                m_target = target;

                m_isSpawn = true;
                m_isDespawn = false;
                m_lifeTime = 0;
                m_isDamage = false;
            }
        }

        public virtual void Despawn()
        {
            if (m_isSpawn && !m_isDespawn)
            {
                m_attackData = null;
                m_target = null;

                m_isDespawn = true;
                m_lifeTime = 0;

                // 缓存池回收
                gameObject.Despawn();
            }
        }

        /// <summary>
        /// 创建伤害
        /// </summary>
        protected virtual void CreateDamage(ActorBase actor)
        {
            m_isDamage = true;
            Debug.Log(actor.actorGameObject.name + " CreateDamage");
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

        protected virtual void OnCollisionEnter(Collision collision)
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
