using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmallUniverse.Behaviour;

namespace SmallUniverse.Manager
{
    public abstract class BaseManager : BaseBehaviour, IManager
    {
        public abstract void Initialize();
        public abstract void OnFixedUpdate(float deltaTime);
        public abstract void OnUpdate(float deltaTime);
        public abstract void OnDispose();
    }
}
