using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace SmallUniverse.AI
{
    [TaskCategory("Custom")]
    [TaskDescription("转到指定方向")]
    public class RotationTo : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("目标 Transform")]
        public SharedTransform target;


        private ActorBase m_ownerActor;
        private Vector3 m_direction;

        public override void OnStart()
        {
            m_ownerActor = gameObject.GetComponentInParent<ActorBase>();
        }

        public override TaskStatus OnUpdate()
        {
            m_direction = target.Value.position - transform.position;
            m_direction.Normalize();
            m_direction.y = 0;
            m_ownerActor.RotationTo(m_direction);
            return TaskStatus.Success;
        }
    }
}
