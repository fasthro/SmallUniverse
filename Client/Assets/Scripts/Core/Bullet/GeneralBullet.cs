/*
 * @Author: fasthro
 * @Date: 2018-12-27 18:03:13
 * @Description: 普通子弹
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class GeneralBullet : BulletBase
    {
        public override void Spawn(AttackData attackData, ActorBase target)
        {
            base.Spawn(attackData, target);

            transform.position = firePosition;
            transform.rotation = fireRotation;
        }

        protected override void OnUpdate()
        {
            if (!m_isSpawn || m_isDespawn)
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

        protected override void OnCollisionEnter(Collision collision)
        {
            if(m_isDamage)
                return;

            if (GameLayer.Compare(m_attackData.layer, GameLayer.HERO))
            {
                if (GameLayer.Compare(collision.gameObject.layer, GameLayer.MONSTER))
                {
                    // 生成子弹效果
                    Game.gamePool.Spawn(impactAssetPath, null, collision.contacts[0].point, transform.rotation);
                    CreateDamage(collision.gameObject.GetComponent<ActorGameObject>().actor);
                    Despawn();
                }
            }
            else if (GameLayer.Compare(m_attackData.layer, GameLayer.MONSTER))
            {
                if (GameLayer.Compare(collision.gameObject.layer, GameLayer.HERO))
                {
                    // 生成子弹效果
                    Game.gamePool.Spawn(impactAssetPath, null, collision.contacts[0].point, transform.rotation);
                    CreateDamage(collision.gameObject.GetComponent<ActorGameObject>().actor);
                    Despawn();
                }
            }
        }
    }
}
