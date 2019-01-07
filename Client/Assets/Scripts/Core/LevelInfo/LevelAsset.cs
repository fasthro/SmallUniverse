using BehaviorDesigner.Runtime;
using SmallUniverse.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class LevelAsset
    {
        private static Dictionary<string, LevelAssetBundle> bundles = new Dictionary<string, LevelAssetBundle>();
        
        public static GameObject GetGameObject(string assetPath)
        {
            var ap =  AssetPath.Get(assetPath);
            return GetGameObject(ap.bundleName, ap.assetName);
        }

        public static GameObject GetGameObject(string bundleName, string assetName)
        {
           return Get<GameObject>(bundleName, assetName);
        }

        public static ExternalBehaviorTree GetExternalBehaviorTree(string behavior)
        {
            string bundleName = "behavior/" + behavior;
            string assetName = behavior + ".asset";
           return Get<ExternalBehaviorTree>(bundleName, assetName);
        }

        public static T Get<T>(string bundleName, string assetName) where T : class
        {
            LevelAssetBundle bundle = null;
            if (bundles.TryGetValue(bundleName, out bundle))
            {
                return bundle.GetAsset(assetName) as T;
            }

            bundle = new LevelAssetBundle();
            bundle.Initialize(bundleName);

            bundles.Add(bundleName, bundle);

            return bundle.GetAsset(assetName) as T;
        }

        public void UnloadBundle(string bundleName)
        {
            LevelAssetBundle bundle = null;
            if (bundles.TryGetValue(bundleName, out bundle))
            {
                bundle.Unload();

                bundles.Remove(bundleName);
            }
        }
    }
}

