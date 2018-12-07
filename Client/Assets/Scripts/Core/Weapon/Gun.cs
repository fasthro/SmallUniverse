using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class Gun : WeaponBase
    {
        // 子弹发射点
        public Transform firePoint;
        // 子弹资源
        public string bulletAssetPath;

        public override void Initialize(ActorBase actor)
        {
            base.Initialize(actor);

            // 子弹添加到 pool
            Game.gamePool.CreatePoolContainer(bulletAssetPath, LevelAsset.GetGameObject(bulletAssetPath));
        }

        public override void Attack()
        {
            var bullet = Game.gamePool.Spawn<BulletBase>(bulletAssetPath);
            bullet.Initialize();
            bullet.firePosition = firePoint.position;
            bullet.fireDirection = m_actor.actorGameObject.transform.forward;
            bullet.Spawn();
        }
    }
}
