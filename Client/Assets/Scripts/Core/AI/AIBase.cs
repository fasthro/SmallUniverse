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
