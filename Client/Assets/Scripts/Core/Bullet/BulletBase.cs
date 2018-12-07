using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public enum BulletType
    {
        Bullet,
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

        #endregion

        #region public
        [HideInInspector]
        public Vector3 firePosition;   // 子弹发射位置 
                                   
        [HideInInspector]
        public Vector3 fireDirection;  // 子弹发射方向                            
        
        #endregion

        #region private
        // 生命时间
        protected float m_lifeTime;
        // 是否死亡
        public bool m_isLive;
        // 是否已经出生 
        public bool m_isSpawn;

        #endregion

        public virtual void Initialize()
        {
           
        }

        public virtual void Spawn()
        {
            m_isSpawn = true;
            m_isLive = false;
            m_lifeTime = 0;
        }

        protected virtual void Despawn()
        {
            m_isLive = true;
            m_lifeTime = 0;
            m_isSpawn = false;
            
            // 放入缓存池回收
            Game.gamePool.Despawn(gameObject);
        }

        protected virtual void OnUpdate()
        {
            
        }

        void Update()
        {
            OnUpdate();
        }

        /// <summary>
        /// 设置子弹位置
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        protected void SetPosition(Vector3 pos)
        {
            transform.position = pos;
        }

        Quaternion m_rotation = new Quaternion();

        /// <summary>
        /// 设置子弹朝向
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        protected void SetRotation(Vector3 dir)
        {
            m_rotation.SetLookRotation(dir);
            transform.rotation = m_rotation;
        }
    }
}
