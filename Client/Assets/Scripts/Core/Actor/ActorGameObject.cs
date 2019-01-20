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
        [HideInInspector]
        public ActorBase actor;

        void Update()
        {
            if(actor != null)
                actor.OnUpdate();
        }

        void LateUpdate()
        {
            if(actor != null)
                actor.OnLateUpdate();
        }

        void FixedUpdate()
        {
            if(actor != null)
                actor.OnFixedUpdate();
        }
    }
}
