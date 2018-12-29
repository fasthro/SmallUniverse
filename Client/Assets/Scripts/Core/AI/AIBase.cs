/*
 * @Author: fasthro
 * @Date: 2018-12-28 12:18:24
 * @Description: AI 基类
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class AIBase : MonoBehaviour
    {
        protected ActorBase m_actor;

        void Start()
        {
            m_actor = gameObject.GetComponentInParent<ActorBase>();
        }
    }
}
