using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class PoolContainer : MonoBehaviour
    {
        private GameObject m_prefab;
        private string m_containerId;
        
        // 已经回收的列表
        private List<PoolObject> m_readyObjects;
        // 已经占用的列表
        private List<PoolObject> m_occupiedObjects;

        // 对象id
        private int m_objectId = 0;
        private int m_nextObjectId{
            get{
                return m_objectId++;
            }
        }

        /// <summary>
        /// 初始化容器
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="prefab"></param>
        public void Initialize(string containerId, GameObject prefab)
        {
            m_containerId = containerId;
            m_prefab = prefab;
            m_readyObjects = new List<PoolObject>();
            m_occupiedObjects = new List<PoolObject>();
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        public GameObject Spawn(Transform parent = null, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion())
        {
            PoolObject newObject = null;
            if(m_readyObjects.Count > 0)
            {
                newObject = m_readyObjects[0];
                newObject.gameObject.transform.SetParent(parent);
                newObject.gameObject.transform.position = position;
                newObject.gameObject.transform.rotation = rotation;

                m_readyObjects.RemoveAt(0);
                m_occupiedObjects.Add(newObject);
            }
            else{
                GameObject newGo = GameObject.Instantiate(m_prefab) as GameObject;
                newGo.transform.SetParent(parent);
                newGo.transform.position = position;
                newGo.transform.rotation = rotation;

                newObject = newGo.AddComponent<PoolObject>();
                newObject.containerId = m_containerId;
                newObject.id = m_objectId;

                m_occupiedObjects.Add(newObject);
            }
            newObject.gameObject.SetActive(true);
            
            return newObject.gameObject;
        }
        
        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="poolObject"></param>
        public void Despawn(PoolObject poolObject)
        {
            for(int i = 0; i < m_occupiedObjects.Count; i++)
            {
                if(poolObject.id == m_occupiedObjects[i].id)
                {
                    m_occupiedObjects.RemoveAt(i);
                    break;
                }
            }

            poolObject.gameObject.SetActive(false);
            poolObject.gameObject.transform.SetParent(transform);

            m_readyObjects.Add(poolObject);
        }
    }
}

