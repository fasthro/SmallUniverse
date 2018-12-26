using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class ActorGameObject : MonoBehaviour
    {
        public Transform WeaponBone;
        
        // 角色实例
        private ActorBase m_actor;

        // 获取角色实例
        public ActorBase GetActor()
        {
            if(m_actor == null)
            {
                m_actor = gameObject.GetComponentInParent<ActorBase>();
            }
            return m_actor;
        }
    }
}
