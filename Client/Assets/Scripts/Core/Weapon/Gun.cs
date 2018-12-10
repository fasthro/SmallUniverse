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

        public override void Attack()
        {
            m_bulletRotation.SetLookRotation(m_actor.actorGameObject.transform.forward);

            switch (bulletType)
            {
                case BulletType.Normal:
                    FireNormalBullet();
                    break;
                case BulletType.Spray:
                    FireBulletSpray();
                    break;
                case BulletType.Line:
                    FireBulletLine();
                    break;
            }

            SetMuzzleEffect();
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
            if (bulletType == BulletType.Spray || bulletType == BulletType.Line)
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
        private void FireNormalBullet()
        {
            m_bullet = Game.gamePool.Spawn<Bullet>(bulletAssetPath);
            m_bullet.Initialize();
            m_bullet.firePosition = firePoint.position;
            m_bullet.fireRotation = m_bulletRotation;
            m_bullet.Spawn();
        }

        /// <summary>
        /// 发射 Spray 子弹
        /// </summary>
        private void FireBulletSpray()
        {
            if (m_bullet == null)
            {
                m_bullet = Game.gamePool.Spawn<BulletSpray>(bulletAssetPath);
                m_bullet.Initialize();
                m_bullet.firePosition = firePoint.position;
                m_bullet.fireRotation = m_bulletRotation;
                m_bullet.Spawn();
            }
        }

        /// <summary>
        /// 发射 Line 子弹
        /// </summary>
        private void FireBulletLine()
        {
            if (m_bullet == null)
            {
                m_bullet = Game.gamePool.Spawn<BulletLine>(bulletAssetPath);
                m_bullet.Initialize();
                m_bullet.firePosition = firePoint.position;
                m_bullet.fireRotation = m_bulletRotation;
                m_bullet.Spawn();
            }
        }

        private void SetMuzzleEffect()
        {
            if (string.IsNullOrEmpty(m_bullet.muzzleAssetPath))
                return;

            Game.gamePool.Spawn(m_bullet.muzzleAssetPath, firePoint, firePoint.position, m_bulletRotation);
        }
    }
}
