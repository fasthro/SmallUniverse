using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SU.Manager
{
    public interface IManager  {
        void Initialize();
        void OnDispose();
    }
}
