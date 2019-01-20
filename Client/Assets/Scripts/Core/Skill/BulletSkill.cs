/*
 * @Author: fasthro
 * @Date: 2018-12-29 14:34:57
 * @Description: 子弹技能
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class BulletSkill : SkillBase
    {
        public BulletGroup bulletGroup;

        public override void Attack(AttackData attackData, ActorBase target)
        {
            //bulletGroup.Attack(m_actor.actorGameObject.weaponPoint.position, m_actor.actorGameObject.transform.rotation, attackData, null);
        }
    }
}
