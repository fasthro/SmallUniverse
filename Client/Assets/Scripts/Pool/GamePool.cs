using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class GamePool : MonoBehaviour
    {
        // 对象池字典
        private Dictionary<string, PoolContainer> pools;

        void Awake()
        {
            pools = new Dictionary<string, PoolContainer>();
        }

        /// <summary>
        /// 创建 pool container
        /// </summary>
        /// <param name="containerId">对象池容器id(资源路径)</param>
        /// <param name="prefab"></param>
        public void CreatePoolContainer(string containerId, GameObject prefab)
        {
            if (!pools.ContainsKey(containerId))
            {
                GameObject containerGo = new GameObject("Container_" + containerId);
                containerGo.transform.parent = transform;
                PoolContainer container = containerGo.AddComponent<PoolContainer>();
                container.Initialize(containerId, prefab);
                pools.Add(containerId, container);
            }
        }

        /// <summary>
        /// poolContainer 获取对象
        /// </summary>
        /// <param name="containerId">对象池容器id(资源路径)</param>
        /// <param name="parent"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        public GameObject Spawn(string containerId, Transform parent = null, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion())
        {
            if (pools.ContainsKey(containerId))
            {
                return pools[containerId].Spawn(parent, position, rotation);
            }
            Debug.LogError("Game pool spawn gameObject Out of the pools! [ " + containerId + "]");
            return null;
        }

        /// <summary>
        /// poolContainer 回收对象
        /// </summary>
        /// <param name="go">回收的对象</param>
        public void Despawn(GameObject go)
        {
            var poolObj = go.GetComponent<PoolObject>();
            if (poolObj != null)
            {
                if (pools.ContainsKey(poolObj.containerId))
                {
                    pools[poolObj.containerId].Despawn(poolObj);
                }
                else
                {
                    Debug.LogError("Game pool despawn gameObject Out of the pools! [ " + go.ToString() + "]");
                }
            }
            else
            {
                Debug.LogError("Game pool despawn gameObject no poolObject component! [ " + go.ToString() + "]");
            }
        }
    }
}
