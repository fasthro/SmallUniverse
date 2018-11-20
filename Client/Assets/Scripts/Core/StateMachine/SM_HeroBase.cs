using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class SM_HeroBase : StateMachineBehaviour
    {
        protected Hero m_hero;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_hero = animator.GetComponentInParent<Hero>();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            
        }

        public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            
        }

        public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }

        protected bool IsAniEnd(float normalizedTime)
        {
            if(normalizedTime > 1.0f)
            {
                int round = (int)normalizedTime;
                float nt = normalizedTime - round;
                return nt >= 0.99f;
            }
            return normalizedTime >= 0.99f;
        }
    }

}
