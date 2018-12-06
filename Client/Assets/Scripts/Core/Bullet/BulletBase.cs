using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{

    public abstract class BulletBase : MonoBehaviour
    {
        #region config
        // 子弹速度
        public float speed;
        // 生命时间
        public float lifeTime;

        #endregion

        #region public

        // 是否死亡
        [HideInInspector]
        public bool isDie;

        // 是否已经出生 
        [HideInInspector]
        public bool isSpawn;

        #endregion

        #region private
        // 生命时间
        protected float m_lifeTime;

        #endregion

        public virtual void Spawn(Vector3 startPosition, Vector3 direction)
        {
            isSpawn = true;
            isDie = false;
            m_lifeTime = 0;

            transform.position = startPosition;

            Quaternion rotation = new Quaternion();
            rotation.SetLookRotation(direction);
            transform.rotation = rotation;
        }

        protected virtual void OnDie()
        {
            isDie = true;
            
            Game.gamePool.Despawn(gameObject);
        }

        protected virtual void OnUpdate()
        {
            if (!isSpawn || isDie)
                return;

            if (m_lifeTime >= lifeTime)
            {
                OnDie();
            }
            else
            {
                m_lifeTime += Time.deltaTime;
                transform.position += transform.forward * speed * Time.deltaTime;
            }
        }

        void Update()
        {
            OnUpdate();
        }
    }
}
