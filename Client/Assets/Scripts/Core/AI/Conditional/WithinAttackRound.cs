using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using SmallUniverse.Utils;

namespace SmallUniverse.AI
{
    [TaskCategory("Custom/WithinAttack")]
    [TaskDescription("判断目标是否在攻击范围内-攻击范围以自身为中心点的圆形范围内")]
    public class WithinAttackRange : Conditional
    {
        public SharedTransform target;
        public SharedFloat radius;

        public override TaskStatus OnUpdate()
        {
            if(WithinUtils.WithinRound(transform.position, target.Value.position, radius.Value))
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }
    }
}
