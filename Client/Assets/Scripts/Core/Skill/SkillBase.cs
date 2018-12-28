using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public enum SkillType
    {
        Bullet,
    }

    public class SkillBase : MonoBehaviour
    {
        public SkillType type;

        protected ActorBase m_actor;

        public virtual void Initialize(ActorBase actor)
        {
            m_actor = actor;
        }

        public virtual void Attack(AttackData attackData, ActorBase target)
        {
            
        }
    }
}
