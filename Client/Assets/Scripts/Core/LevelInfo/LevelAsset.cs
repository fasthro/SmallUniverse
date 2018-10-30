using SmallUniverse.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class LevelAsset
    {
        private static Dictionary<string, LevelAssetBundle> bundles = new Dictionary<string, LevelAssetBundle>();

        public static GameObject GetGameObject(string bundleName, string assetName)
        {
            LevelAssetBundle bundle = null;
            if (bundles.TryGetValue(bundleName, out bundle))
            {
                return bundle.GetAsset(assetName) as GameObject;
            }

            bundle = new LevelAssetBundle();
            bundle.Initialize(bundleName);

            bundles.Add(bundleName, bundle);

            return bundle.GetAsset(assetName) as GameObject;
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

