/*
 * @Author: fasthro
 * @Date: 2018-12-27 18:03:13
 * @Description: 武器-> 枪
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class Gun : WeaponBase
    {
        #region config

        // 子弹发射点
        public Transform firePoint;
        // 子弹类型
        public BulletType bulletType;
        // 子弹资源资源路径
        public string bulletAssetPath;

        #endregion

        // 子弹
        protected BulletBase m_bullet;

        // 子弹旋转
        private Quaternion m_bulletRotation = new Quaternion();

        public override void Initialize(ActorBase actor)
        {
            base.Initialize(actor);
        }

        public override void Attack(AttackData attackData, ActorBase target)
        {
            m_bulletRotation.SetLookRotation(m_actor.actorGameObject.transform.forward);

            switch (bulletType)
            {
                case BulletType.General:
                    FireGeneralBullet(attackData, target);
                    break;
                case BulletType.Fuel:
                    FireFuelBullet(attackData, target);
                    break;
                case BulletType.Line:
                    FireLineBullet(attackData, target);
                    break;
            }
        }

        public override void StopAttack()
        {
            if (m_bullet != null)
            {
                m_bullet.Despawn();
                m_bullet = null;
            }
        }

        protected override void OnUpdate()
        {
            if (bulletType == BulletType.Fuel || bulletType == BulletType.Line)
            {
                if (m_bullet != null)
                {
                    m_bulletRotation.SetLookRotation(m_actor.actorGameObject.transform.forward);
                    m_bullet.SetRotation(m_bulletRotation);
                    m_bullet.SetPosition(firePoint.position);
                }
            }
        }

        /// <summary>
        /// 发射普通子弹
        /// </summary>
        private void FireGeneralBullet(AttackData attackData, ActorBase target)
        {
            var bullet = Game.gamePool.Spawn<GeneralBullet>(bulletAssetPath);
            bullet.Initialize();
            bullet.firePosition = firePoint.position;
            bullet.fireRotation = m_bulletRotation;
            bullet.Spawn(attackData, target);

            // 设置枪口效果
            if (!string.IsNullOrEmpty(bullet.muzzleAssetPath))
                Game.gamePool.Spawn(bullet.muzzleAssetPath, firePoint, firePoint.position, m_bulletRotation);
        }

        /// <summary>
        /// 发射燃料子弹
        /// </summary>
        private void FireFuelBullet(AttackData attackData, ActorBase target)
        {
            if (m_bullet == null)
            {
                m_bullet = Game.gamePool.Spawn<FuelBullet>(bulletAssetPath);
                m_bullet.Initialize();
                m_bullet.firePosition = firePoint.position;
                m_bullet.fireRotation = m_bulletRotation;
                m_bullet.Spawn(attackData, target);

                // 设置枪口效果
                if (!string.IsNullOrEmpty(m_bullet.muzzleAssetPath))
                    Game.gamePool.Spawn(m_bullet.muzzleAssetPath, firePoint, firePoint.position, m_bulletRotation);
            }
        }

        /// <summary>
        /// 发射线型子弹
        /// </summary>
        private void FireLineBullet(AttackData attackData, ActorBase target)
        {
            if (m_bullet == null)
            {
                m_bullet = Game.gamePool.Spawn<LineBullet>(bulletAssetPath);
                m_bullet.Initialize();
                m_bullet.firePosition = firePoint.position;
                m_bullet.fireRotation = m_bulletRotation;
                m_bullet.Spawn(attackData, target);

                // 设置枪口效果
                if (!string.IsNullOrEmpty(m_bullet.muzzleAssetPath))
                    Game.gamePool.Spawn(m_bullet.muzzleAssetPath, firePoint, firePoint.position, m_bulletRotation);
            }
        }
    }
}
