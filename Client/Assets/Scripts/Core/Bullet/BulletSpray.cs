using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class BulletSpray : BulletBase
    {
		public override void Spawn(AttackData attackData, ActorBase target)
        {
            base.Spawn(attackData, target);

            transform.position = firePosition;
            transform.rotation = fireRotation;
        }
    }
}

