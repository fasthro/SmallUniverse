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

            transform.position = firePosition;
            transform.rotation = fireRotation;
        }

        protected override void OnUpdate()
        {
            if (!IsSpawn || IsDespawn)
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
