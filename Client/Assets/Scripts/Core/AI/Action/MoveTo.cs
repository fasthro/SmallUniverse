using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace SmallUniverse.AI
{
    [TaskCategory("Custom")]
    [TaskDescription("移动到指定位置")]
    public class MoveTo : Action
    {

        [BehaviorDesigner.Runtime.Tasks.Tooltip("目标 Transform")]
        public SharedTransform target;

        [BehaviorDesigner.Runtime.Tasks.Tooltip("停止的最小距离")]
        public SharedFloat stopMoveDistanceMin;

        [BehaviorDesigner.Runtime.Tasks.Tooltip("停止的最大距离")]
        public SharedFloat stopMoveDistanceMax;

        [BehaviorDesigner.Runtime.Tasks.Tooltip("停止移动的最小时间")]
        public SharedFloat stopMoveTimeMin;

        [BehaviorDesigner.Runtime.Tasks.Tooltip("停止移动的最大时间")]
        public SharedFloat stopMoveTimeMax;

        private ActorBase m_ownerActor;

        private float stopMoveTime;

        public override void OnStart()
        {
            m_ownerActor = gameObject.GetComponentInParent<ActorBase>();
            stopMoveTime = Random.Range(stopMoveTimeMin.Value, stopMoveTimeMax.Value);
            m_ownerActor.MoveTo(target.Value.position, Random.Range(stopMoveDistanceMin.Value, stopMoveDistanceMax.Value));
        }

        public override TaskStatus OnUpdate()
        {
            stopMoveTime -= Time.deltaTime;
            if(!m_ownerActor.IsMove || stopMoveTime <= 0)
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }
    }
}
