using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class NormalBulletSkill : SkillBase
    {
        public Transform firePoint;

        // 子弹资源资源路径
        public string bulletAssetPath;

        public override void Attack(AttackData attackData, ActorBase target)
        {
            Quaternion rotation = new Quaternion();
            rotation.SetLookRotation(m_actor.actorGameObject.transform.forward);

            var bullet = Game.gamePool.Spawn<Bullet>(bulletAssetPath);
            bullet.Initialize();
            bullet.firePosition = firePoint.position;
            bullet.fireRotation = rotation;
            bullet.Spawn(attackData, target);
        }
    }
}
