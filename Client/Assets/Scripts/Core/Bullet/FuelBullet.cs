/*
 * @Author: fasthro
 * @Date: 2018-12-27 18:03:13
 * @Description: 燃料子弹
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class FuelBullet : BulletBase
    {
		public override void Spawn(AttackData attackData, ActorBase target)
        {
            base.Spawn(attackData, target);

            transform.position = firePosition;
            transform.rotation = fireRotation;
        }
    }
}

