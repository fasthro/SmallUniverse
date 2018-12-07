using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class Bullet : BulletBase
    {
        public override void Spawn()
        {
            base.Spawn();

            // 发射位置
            SetPosition(firePosition);

            // 发射方向
            SetRotation(fireDirection);
        }

        protected override void OnUpdate()
        {
            if (!m_isSpawn || m_isLive)
                return;

            if (m_lifeTime >= lifeTime)
            {
                Despawn();
            }
            else
            {
                m_lifeTime += Time.deltaTime;
                transform.position += transform.forward * speed * Time.deltaTime;
            }
        }
    }
}
