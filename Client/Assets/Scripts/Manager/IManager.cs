using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse.Manager
{
    public interface IManager  {
        void Initialize();
        void OnDispose();
    }
}
