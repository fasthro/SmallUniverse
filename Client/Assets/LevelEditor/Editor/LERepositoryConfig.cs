using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SU.Editor.LevelEditor
{
    [System.Serializable]
    public class LERepositoryAsset
    {
        // 资源所在仓库名称
        public string repositoryName;
        // prefab name
        public string assetName;
        // prefab path
        public string assetPath;
        // prefab 资源
        public GameObject asset;
        // bundle name
        public string bundleName;
    }

    [System.Serializable]
    public class LERepository
    {
        // 资源库名称
        public string name;
        // 资源库路径
        public string path;
        // 资源库中 prefab 资源
        public LERepositoryAsset[] assets;

        /// <summary>
        /// 初始化资源库
        /// </summary>
        public void Initialize()
        {
            string[] fps = Directory.GetFiles(path, "*.prefab", SearchOption.TopDirectoryOnly);
            assets = new LERepositoryAsset[fps.Length];

            for (int i = 0; i < fps.Length; i++)
            {
                string fp = fps[i];
                LERepositoryAsset rp = new LERepositoryAsset();
                string ns = fp.Substring(path.Length + 1, fp.Length - path.Length - 1);
                rp.repositoryName = name;
                rp.assetName = ns.Substring(0, ns.Length - ".prefab".Length);
                rp.assetPath = fp;
                rp.asset = AssetDatabase.LoadAssetAtPath(fp, typeof(GameObject)) as GameObject;
                rp.bundleName = "level/repository/" + name.ToLower();

                assets[i] = rp;
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
        /// 获取资源库
        /// </summary>
        /// <param name="repositoryName"> 资源库名称 </param>
        /// <returns></returns>
        public LERepository GetRepository(string repositoryName)
        {
            for (int i = 0; i < repositorys.Count; i++)
            {
                if (repositoryName == repositorys[i].name)
                {
                    return repositorys[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 获取资源库内的资源
        /// </summary>
        /// <param name="repositoryName"></param>
        /// <param name="prefabPath"></param>
        /// <returns></returns>
        public LERepositoryAsset GetRepositoryPrefab(string repositoryName, string prefabPath)
        {
            for (int i = 0; i < repositorys.Count; i++)
            {
                if (repositorys[i].name == repositoryName)
                {
                    for (int k = 0; k < repositorys[i].assets.Length; k++)
                    {
                        if (repositorys[i].assets[k].assetPath == prefabPath)
                        {
                            return repositorys[i].assets[k];
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
