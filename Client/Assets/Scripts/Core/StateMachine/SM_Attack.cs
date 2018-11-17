using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class SM_Attack : SM_HeroBase
    {
		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(stateInfo.normalizedTime >= 0.98f)
            {
                animator.SetBool(ActorAnimatorAttribute.Attack.ToString(), false);
            }
        }
    }
}

