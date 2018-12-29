/*
 * @Author: fasthro
 * @Date: 2018-12-27 18:03:13
 * @Description: 武器基类
 */
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

        public virtual void Attack(AttackData attackData, ActorBase target)
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
