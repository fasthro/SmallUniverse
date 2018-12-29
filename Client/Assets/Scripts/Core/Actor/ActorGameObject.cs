/*
 * @Author: fasthro
 * @Date: 2018-12-27 18:03:13
 * @Description: 角色物体对象
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class ActorGameObject : MonoBehaviour
    {
        // 武器挂点
        public Transform weaponPoint;
        // 头上血条挂点
        public Transform headPoint;
        
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
