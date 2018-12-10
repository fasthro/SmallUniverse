using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class PoolObject : MonoBehaviour
    {
        // pool container id
				[HideInInspector]
        public string containerId;
        // object id
				[HideInInspector]
        public int id;
    }
}
