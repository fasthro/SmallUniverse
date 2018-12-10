using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class BulletSpray : BulletBase
    {
		public override void Spawn()
        {
            base.Spawn();

            transform.position = firePosition;
            transform.rotation = fireRotation;
        }
    }
}

