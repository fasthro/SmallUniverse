using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public enum WeaponType
    {
        Gun,
    }

    public abstract class WeaponBase : MonoBehaviour
    {
        // 武器类型
        public WeaponType weaponType;

        // actor
        protected ActorBase m_actor;

        public virtual void Initialize(ActorBase actor)
        {
            m_actor = actor;
        }

        public virtual void Attack()
        {

        }

        public virtual void StopAttack()
        {

        }

        protected virtual void OnUpdate()
        {


        }

        void Update()
        {
            OnUpdate();
        }
    }
}
