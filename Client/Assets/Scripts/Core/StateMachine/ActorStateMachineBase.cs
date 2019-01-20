/*
 * @Author: fasthro
 * @Date: 2019-01-18 15:29:19
 * @Description: Actor StateMachineBase 基类
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public abstract class ActorStateMachineBase : StateMachineBehaviour
    {
        protected ActorBase m_actor;
        protected ActorGameObject m_actorGamObject;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_actorGamObject = animator.gameObject.GetComponent<ActorGameObject>();
            m_actor = m_actorGamObject.actor;
        }
    }
}

