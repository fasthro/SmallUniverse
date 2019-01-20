/*
 * @Author: fasthro
 * @Date: 2019-01-10 11:56:55
 * @Description: 攻击 StateMachine
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class AttackStateMachine : ActorStateMachineBase
    {
        // 释放此攻击的时候是否可以移动
        public bool canMove;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            m_actor.attack = true;
            m_actor.SetCanMove(canMove);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_actor.attack = false;
            m_actor.SetCanMove(true);
        }
    }
}

