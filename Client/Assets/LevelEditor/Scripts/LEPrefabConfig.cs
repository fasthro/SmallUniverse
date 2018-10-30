using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SmallUniverse.Editor.LevelEditor
{
    [System.Serializable]
    public class LEPrefabGo
    {
        public string prefabName;
        public string name;
        public string path;
        public GameObject go;
        public string bundleName;
    }

    [System.Serializable]
    public class LEPrefab
    {
        public string name;
        public string path;
        public Dictionary<string, LEPrefabGo> gos = new Dictionary<string, LEPrefabGo>();

        /// <summary>
        /// 初始化资源库
        /// </summary>
        public void Initialize()
        {
            gos.Clear();

            string[] paths = Directory.GetFiles(path, "*.prefab", SearchOption.TopDirectoryOnly);
            
            for (int i = 0; i < paths.Length; i++)
            {
                string p = paths[i];
                LEPrefabGo pgo = new LEPrefabGo();
                string ns = p.Substring(path.Length + 1, p.Length - path.Length - 1);
                pgo.prefabName = name;
                pgo.name = ns.Substring(0, ns.Length - ".prefab".Length);
                pgo.path = p;
#if UNITY_EDITOR
                pgo.go = AssetDatabase.LoadAssetAtPath(p, typeof(GameObject)) as GameObject;
#endif
                pgo.bundleName = "levels/prefabs/" + name.ToLower();

                gos.Add(pgo.name, pgo);
            }
        }

        /// <summary>
        /// 获取 prefab go
        /// </summary>
        /// <param name="gname"></param>
        /// <returns></returns>
        public LEPrefabGo GetPrefabGo(string gname)
        {
            LEPrefabGo prefabGo = null;
            if (gos.TryGetValue(gname, out prefabGo))
            {
                return prefabGo;
            }
            return null;
        }
    }

    [System.Serializable]
    public class LEPrefabConfig : ScriptableObject
    {
        // prefab map
        public Dictionary<string, LEPrefab> prefabs = new Dictionary<string, LEPrefab>();

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            prefabs.Clear();

            var prefabDirectory = LEUtils.GetPathToAssets(LESetting.PrefabDirectory);

            string[] dirs = Directory.GetDirectories(prefabDirectory, "*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < dirs.Length; i++)
            {
                string dp = dirs[i];

                LEPrefab prefab = new LEPrefab();
                prefab.name = dp.Substring(prefabDirectory.Length, dp.Length - prefabDirectory.Length);
                prefab.path = dp;
                prefab.Initialize();

                prefabs.Add(prefab.name, prefab);
            }
        }

        /// <summary>
        /// 获取Prefab
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public LEPrefab GetPrefab(string name)
        {
            LEPrefab prefab = null;
            if (prefabs.TryGetValue(name, out prefab))
            {
                return prefab;
            }
            return null;
        }

        /// <summary>
        /// 获取 Prefab 内的资源
        /// </summary>
        /// <param name="pname">prefab name</param>
        /// <param name="gname">prefab go name </param>
        /// <returns></returns>
        public LEPrefabGo GetPrefabGo(string pname, string gname)
        {
            LEPrefab prefab = GetPrefab(pname);
            if (prefab != null)
            {
                return prefab.GetPrefabGo(gname);
            }
            return null;
        }
    }
}
