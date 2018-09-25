using SU.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SU.Level
{
    public class MapAsset
    {
        // bundle name
        public string bundleName;
        // bundle
        private AssetBundle bundle;
        // 资源管理
        private ResManager resMgr;
        // 资源字典
        private Dictionary<string, Object> assets;

        public MapAsset(ResManager _resMgr)
        {
            resMgr = _resMgr;
            assets = new Dictionary<string, Object>();
        }

        public void Load()
        {
            if (bundle == null)
            {
                bundle = resMgr.LoadAssetBundle(bundleName);
            }
        }

        public Object GetAsset(string assetName)
        {
            Object obj = null;
            if (assets.TryGetValue(assetName, out obj))
            {
                return obj;
            }
            if (bundle != null)
            {
                obj = bundle.LoadAsset(assetName);
                assets.Add(assetName, obj);
            }
            return obj;
        }
    }
}

