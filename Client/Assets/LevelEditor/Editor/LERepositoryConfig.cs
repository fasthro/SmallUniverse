using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SU.Editor.LevelEditor
{
    [System.Serializable]
    public class LERepositoryPrefab
    {
        // 资源库名称
        public string repositoryName;
        // prefab
        public string name;
        // prefab path
        public string path;
        // prefab
        public GameObject prefab;
    }

    [System.Serializable]
    public class LERepository
    {
        // 资源库名称
        public string name;
        // 资源库路径
        public string path;
        // 资源库中prefab资源
        public LERepositoryPrefab[] prefabs;

        /// <summary>
        /// 初始化资源库
        /// </summary>
        public void Initialize()
        {
            string[] fps = Directory.GetFiles(path, "*.prefab", SearchOption.TopDirectoryOnly);
            prefabs = new LERepositoryPrefab[fps.Length];

            for (int i = 0; i < fps.Length; i++)
            {
                string fp = fps[i];
                LERepositoryPrefab rp = new LERepositoryPrefab();
                string ns = fp.Substring(path.Length + 1, fp.Length - path.Length - 1);
                rp.repositoryName = name;
                rp.name = ns.Substring(0, ns.Length - ".prefab".Length);
                rp.path = fp;
                rp.prefab = AssetDatabase.LoadAssetAtPath(fp, typeof(GameObject)) as GameObject;

                prefabs[i] = rp;
            }
        }

        /// <summary>
        /// 重新加载资源库
        /// </summary>
        public void Reload()
        {
            Initialize();
        }
    }

    [System.Serializable]
    public class LERepositoryConfig : ScriptableObject
    {
        // 资源库配置列表
        public List<LERepository> repositorys= new List<LERepository>();

        /// <summary>
        /// 初始化仓库
        /// </summary>
        public void Initialize()
        {
            repositorys.Clear();

            var repositoryDirectory = LEUtils.GetPathToAssets(LESetting.RepositoryDirectory);

            string[] dirs = Directory.GetDirectories(repositoryDirectory, "*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < dirs.Length; i++)
            {
                string dp = dirs[i];

                LERepository repository = new LERepository();
                repository.name = dp.Substring(repositoryDirectory.Length, dp.Length - repositoryDirectory.Length);
                repository.path = dp;
                repository.Initialize();

                repositorys.Add(repository);
            }
        }

        /// <summary>
        /// 获取所有资源库名字
        /// </summary>
        /// <returns></returns>
        public string[] GetNames()
        {
            string[] names = new string[repositorys.Count];
            for (int i = 0; i < repositorys.Count; i++)
            {
                names[i] = repositorys[i].name;
            }
            return names;
        }

        /// <summary>
        /// 获取资源库
        /// </summary>
        /// <param name="repositoryIndex"> 资源库索引 </param>
        /// <returns></returns>
        public LERepository GetRepository(int repositoryIndex)
        {
            if (repositoryIndex >= 0 && repositoryIndex < repositorys.Count)
            {
                return repositorys[repositoryIndex];
            }
            return null;
        }

        /// <summary>
        /// 获取资源库内的资源
        /// </summary>
        /// <param name="repositoryName"></param>
        /// <param name="prefabPath"></param>
        /// <returns></returns>
        public LERepositoryPrefab GetRepositoryPrefab(string repositoryName, string prefabPath)
        {
            for (int i = 0; i < repositorys.Count; i++)
            {
                if (repositorys[i].name == repositoryName)
                {
                    for (int k = 0; k < repositorys[i].prefabs.Length; k++)
                    {
                        if (repositorys[i].prefabs[k].path == prefabPath)
                        {
                            return repositorys[i].prefabs[k];
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 重新载入资源库
        /// </summary>
        /// <param name="repositoryIndex"></param>
        public void ReloadRepository(int repositoryIndex)
        {
            if (repositoryIndex >= 0 && repositoryIndex < repositorys.Count)
            {
                repositorys[repositoryIndex].Reload();
            }
        }
    }
}
