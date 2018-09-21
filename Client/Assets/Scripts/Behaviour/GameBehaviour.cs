using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SU.Behaviour
{
    public class GameBehaviour : MonoBehaviour
    {
        void Awake()
        {
            OnAwake();
        }

        void Start()
        {
            OnStart();
        }

        void FixedUpdate()
        {
            OnFixedUpdate();
        }

        void Update()
        {
            OnUpdate();
        }

        void LateUpdate()
        {
            OnLateUpdate();
        }

        void OnDestroy()
        {
            OnDestroySelf();
        }

        protected virtual void OnAwake() { }
        protected virtual void OnStart() { }
        protected virtual void OnFixedUpdate() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnLateUpdate() { }
        protected virtual void OnDestroySelf() { }
    }
}
