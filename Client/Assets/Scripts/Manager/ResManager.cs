using SU.Define;
using SU.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SU.Manager
{
    public class AssetBundleInfo
    {
        // bundle
        public AssetBundle assetBundle;
        // 资源引用数量
        public int referencedCount;

        public AssetBundleInfo(AssetBundle _assetBundle)
        {
            assetBundle = _assetBundle;
            referencedCount = 1;
        }
    }

    public class AssetBundleLoader
    {
        // 全局 新 load Id
        private static int loadId = 0;
        // 加载 Id,每个加载信息都有唯一的Id
        public int id;
        // 资源名称
        public string abName;
        // 加载完成回调
        public event Action<AssetBundleLoader, AssetBundle> completed;

        public static AssetBundleLoader Create(string _abName, Action<AssetBundleLoader, AssetBundle> _completed)
        {
            return new AssetBundleLoader(_abName, _completed);
        }

        public AssetBundleLoader(string _abName, Action<AssetBundleLoader, AssetBundle> _completed)
        {
            loadId++;
            id = loadId;
            abName = _abName;
            completed = _completed;
        }

        public void CallCompleted(AssetBundle assetBundle)
        {
            if (completed != null)
            {
                completed(this, assetBundle);
            }
        }
    }
    
    public class ResManager : BaseManager
    {
        // bundle 资源缓存字典 <资源名称, AssetBundleInfo>
        private Dictionary<string, AssetBundleInfo> bundleDic = new Dictionary<string, AssetBundleInfo>();
        // bundle 资源缓存字典 <资源名称, List<AssetBundleLoader>>, 正在加载的bundle
        private Dictionary<string, List<AssetBundleLoader>> loaderDic = new Dictionary<string, List<AssetBundleLoader>>();
        // manifes 
        private AssetBundleManifest mainManifest;

        public override void Initialize()
        {
            bundleDic.Clear();

            // 加载 mainManifest
            var mainBundle = AssetBundle.LoadFromFile(PathUtils.GetDataPath() + AppConst.AssetDirname);
            mainManifest = mainBundle.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
        }

        public override void OnDispose()
        {
            throw new System.NotImplementedException();
        }

        public override void OnFixedUpdate(float deltaTime)
        {
            throw new System.NotImplementedException();
        }

        public override void OnUpdate(float deltaTime)
        {
            throw new System.NotImplementedException();
        }

        #region Resources Load

        public UnityEngine.Object LoadAsset(string path)
        {
            return Resources.Load(path);
        }

        public T LoadAsset<T>(string path) where T : UnityEngine.Object
        {
            UnityEngine.Object obj = Resources.Load<T>(path);
            if (obj == null)
            {
                Debug.LogError("ResManager -> LoadAsset " + path + "is null");
                return null;
            }
            return obj as T;
        }

        public T[] LoadAssets<T>(string path) where T : UnityEngine.Object
        {
            return  Resources.LoadAll<T>(path);
        }

        public ResourceRequest LoadAssetAsync(string path)
        {
            return Resources.LoadAsync(path);
        }

        public ResourceRequest LoadAssetAsync<T>(string path) where T : UnityEngine.Object
        {
            return Resources.LoadAsync<T>(path);
        }

        public ResourceRequest LoadAssetsAsync(string path, System.Type type)
        {
            return Resources.LoadAsync(path, type);
        }
        #endregion

        #region AssetsBundle Load

        /// <summary>
        /// 加载 bundle 资源
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        public AssetBundle LoadAssetBundle(string abName)
        {
            // 从缓存中加载bundle
            AssetBundleInfo bundleInfo = GetCacheAssetBundle(abName);
            if (bundleInfo == null)
            {
                // 查询所有依赖，先加载依赖资源
                string[] dependencies = mainManifest.GetAllDependencies(abName);
                for (int i = 0; i < dependencies.Length; i++)
                {
                    var dName = dependencies[i];
                    bundleInfo = GetBundleDic(dName);

                    if (bundleInfo == null)
                    {
                        // 缓存中没有，需要重新加载bundle
                        AssetBundle assetBundle = AssetBundle.LoadFromFile(GetAssetBundlePath(dName));
                        bundleInfo = new AssetBundleInfo(assetBundle);
                        // 添加到缓存
                        AddBundleDic(abName, bundleInfo);
                    }
                    else {
                        bundleInfo.referencedCount++;
                    }
                }

                // 加载资源
                bundleInfo = GetBundleDic(abName);
                if (bundleInfo == null)
                {
                    // 缓存中没有，需要重新加载bundle
                    AssetBundle assetBundle = AssetBundle.LoadFromFile(GetAssetBundlePath(abName));
                    bundleInfo = new AssetBundleInfo(assetBundle);
                    // 添加到缓存
                    AddBundleDic(abName, bundleInfo);
                }
                else {
                    bundleInfo.referencedCount++;
                }
            }
            else { 
                bundleInfo.referencedCount++;
            }

            return bundleInfo.assetBundle;
        }

        /// <summary>
        /// 异步加载 bundle
        /// </summary>
        /// /// <param name="loader"></param>
        public void LoadAssetBundleAsync(AssetBundleLoader loader)
        {
            // 从缓存中加载bundle
            AssetBundleInfo bundleInfo = GetCacheAssetBundle(loader.abName);
            if (bundleInfo == null)
            {
                StartCoroutine(AsyncLoadAssetBundle(loader));
            }
            else {
                // 查询所有依赖,依赖引用计数加一
                string[] dependencies = mainManifest.GetAllDependencies(loader.abName);
                for (int i = 0; i < dependencies.Length; i++)
                {
                    var dName = dependencies[i];
                    var dInfo = GetBundleDic(dName);
                    dInfo.referencedCount++;
                }

                // 资源引用加一
               bundleInfo.referencedCount++;

                // 加载完成回调
                loader.CallCompleted(bundleInfo.assetBundle);
            }
        }

        IEnumerator AsyncLoadAssetBundle(AssetBundleLoader loader)
        {
            AssetBundleInfo bundleInfo = null;
            List<AssetBundleLoader> loaderList = null;

            // 查询所有依赖，先加载依赖资源
            string[] dependencies = mainManifest.GetAllDependencies(loader.abName);
            for (int i = 0; i < dependencies.Length; i++)
            {
                var dName = dependencies[i];
                bundleInfo = GetBundleDic(dName);

                if (bundleInfo == null)
                {
                    loaderList = GetLoadDic(dName);

                    if (loaderList == null)
                    {
                        // 添加到正在加.载字典
                        AddLoadDic(dName, loader);
                        
                         AssetBundleCreateRequest dRequest = AssetBundle.LoadFromFileAsync(GetAssetBundlePath(dName));
                        yield return dRequest;

                        // 加载完成添加到缓存
                        bundleInfo = new AssetBundleInfo(dRequest.assetBundle);
                        bundleInfo.referencedCount = GetLoadCount(dName);
                        AddBundleDic(dName, bundleInfo);

                        // 移出loadDic
                        RemoveLoadDic(dName);
                    }
                    else {
                        AddLoadDic(dName, loader);
                    }
                }
                else
                {
                    bundleInfo.referencedCount++;
                }
            }

            // 加载资源
            bundleInfo = GetBundleDic(loader.abName);
            if (bundleInfo == null)
            {
                loaderList = GetLoadDic(loader.abName);

                if (loaderList == null)
                {
                    // 添加到正在加载字典
                    AddLoadDic(loader.abName, loader);

                    AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(GetAssetBundlePath(loader.abName));
                    yield return request;

                    // 加载完成添加到缓存
                    bundleInfo = new AssetBundleInfo(request.assetBundle);
                    AddBundleDic(loader.abName, bundleInfo);

                    // 回调加载
                    loaderList = GetLoadDic(loader.abName);
                    for (int i = 0; i < loaderList.Count; i++)
                    {
                        loaderList[i].CallCompleted(bundleInfo.assetBundle);
                    }

                    // 移出loadDic
                    RemoveLoadDic(loader.abName);
                }
                else {
                    AddLoadDic(loader.abName, loader);
                }
            }
            else {
                bundleInfo.referencedCount++;

                // 回调加载
                loader.CallCompleted(bundleInfo.assetBundle);
            }
        }

        /// <summary>
        /// 卸载 bundle
        /// </summary>
        /// <param name="loader"></param>
        /// <param name="unloadAllObjects"></param>
        public void UnLoadAssetBundle(AssetBundleLoader loader, bool unloadAllObjects = false)
        {
            UnLoadAssetBundle(loader.abName, unloadAllObjects);
        }

        /// <summary>
        /// 卸载 bundle
        /// </summary>
        /// <param name="abName"></param>
        public void UnLoadAssetBundle(string abName, bool unloadAllObjects = false)
        {
            AssetBundleInfo bundleInfo = GetCacheAssetBundle(abName);
            if (bundleInfo != null)
            {
                // 查询所有依赖，先卸载依赖资源
                string[] dependencies = mainManifest.GetAllDependencies(abName);
                for (int i = 0; i < dependencies.Length; i++)
                {
                    var dName = dependencies[i];
                    var dInfo = GetBundleDic(dName);

                    dInfo.referencedCount--;

                    if (dInfo.referencedCount <= 0)
                    {
                        RemoveBundleDic(dName);

                        dInfo.assetBundle.Unload(unloadAllObjects);
                    }
                }

                bundleInfo.referencedCount--;

                if (bundleInfo.referencedCount <= 0)
                {
                    RemoveBundleDic(abName);

                    bundleInfo.assetBundle.Unload(unloadAllObjects);
                }
            }
        }
        
        /// <summary>
        /// 从缓存中获取bundle， 如果缓存中有并且依赖资源都已经记在成功，那么直接返回bundle资源，否则视为缓存中没有此资源
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        private AssetBundleInfo GetCacheAssetBundle(string abName)
        {
            AssetBundleInfo bundleInfo = null;

            // 在缓存中获取 bundle
            bundleDic.TryGetValue(abName, out bundleInfo);
            if (bundleInfo == null)
                return null;

            string[] dependencies = mainManifest.GetAllDependencies(abName);
            // 如果此资源不包含任何依赖直接返回资源
            if (dependencies.Length <= 0)
                return bundleInfo;

            // 检查资源所有依赖是否已经加载完成
            for (int i = 0; i < dependencies.Length; i++)
            {
                var dName = dependencies[i];
                if (!bundleDic.ContainsKey(dName))
                    return null;
            }
            return bundleInfo;
        }

        /// <summary>
        /// 添加 bundleDic
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="bundleInfo"></param>
        private void AddBundleDic(string abName, AssetBundleInfo bundleInfo)
        {
            if (!bundleDic.ContainsKey(abName))
            {
                bundleDic.Add(abName, bundleInfo);
            }
            else {
                Debug.LogError("AddCacheDic-> " + abName + " 已经存在!");
            }
        }

        /// <summary>
        /// 获取 bundleDic
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        private AssetBundleInfo GetBundleDic(string abName)
        {
            AssetBundleInfo bundleInfo = null;
            if (bundleDic.TryGetValue(abName, out bundleInfo))
            {
                return bundleInfo;
            }
            return null;
        }

        /// <summary>
        /// 移出 bundle dic
        /// </summary>
        /// <param name="abName"></param>
        private void RemoveBundleDic(string abName)
        {
            if (bundleDic.ContainsKey(abName))
            {
                bundleDic.Remove(abName);
            }
        }

        /// <summary>
        /// 添加 loadDic
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="loadInfo"></param>
        private void AddLoadDic(string abName, AssetBundleLoader loadInfo)
        {
            if (loaderDic.ContainsKey(abName))
            {
                loaderDic[abName].Add(loadInfo);
            }
            else {
                var loads = new List<AssetBundleLoader>();
                loads.Add(loadInfo);

                loaderDic.Add(abName, loads);
            }
        }

        /// <summary>
        /// 获取 loadDic
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        private List<AssetBundleLoader> GetLoadDic(string abName)
        {
            List<AssetBundleLoader> loads = null; 
            if (loaderDic.TryGetValue(abName, out loads))
            {
                return loads;
            }
            return null;
        }

        /// <summary>
        /// 获取 load 数量
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        private int GetLoadCount(string abName)
        {
            var loads = GetLoadDic(abName);
            if (loads != null)
            {
                return loads.Count;
            }
            return 0;
        }

        /// <summary>
        /// 移除 loadDic
        /// </summary>
        /// <param name="abName"></param>
        private void RemoveLoadDic(string abName)
        {
            if (loaderDic.ContainsKey(abName))
            {
                loaderDic.Remove(abName);
            }
        }

        /// <summary>
        /// 获取 bundle 资源路径
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        private string GetAssetBundlePath(string abName)
        {
            return PathUtils.GetDataPath() + abName;
        }
        #endregion

        private void LogData()
        {
            foreach (KeyValuePair<string, AssetBundleInfo> info in bundleDic)
            {
                Debug.Log("INFO > " + info.Key + " > ref : " + info.Value.referencedCount);
            }

            Debug.Log("------------------------------------------------------------------");

            foreach (KeyValuePair<string, List<AssetBundleLoader>> info in loaderDic)
            {
                Debug.Log("LOAD > " + info.Key + " > ref : " + info.Value.Count);
            }
        }
    }
}

