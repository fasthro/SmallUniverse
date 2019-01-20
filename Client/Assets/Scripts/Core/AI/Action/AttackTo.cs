using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace SmallUniverse.AI
{
    [TaskCategory("Custom")]
    [TaskDescription("移动到指定位置")]
    public class AttackTo : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("目标 Transform")]
        public SharedTransform target;


        private ActorBase m_ownerActor;

        private bool m_attacked;

        public override void OnStart()
        {
            m_ownerActor = gameObject.GetComponentInParent<ActorBase>();
            m_attacked = false;
        }

        public override TaskStatus OnUpdate()
        {
            // if (!m_ownerActor.IsAttack)
            // {
            //     if (!m_attacked)
            //     {
            //         m_attacked = true;
            //         m_ownerActor.Attack(Game.hero.Transform);
            //     }
            // }
            // if (m_attacked)
            // {
            //     if (!m_ownerActor.IsAttack)
            //     {
            //         return TaskStatus.Success;
            //     }
            // }
            return TaskStatus.Running;
        }
    }
}
