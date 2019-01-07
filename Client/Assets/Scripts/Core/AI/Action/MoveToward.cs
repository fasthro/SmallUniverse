using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace SmallUniverse.AI
{
    [TaskCategory("Custom")]
    [TaskDescription("移动到指定位置")]
    public class MoveToward : Action
    {
        public override void OnAwake()
        {

        }

        public override void OnStart()
        {

        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }
    }
}
