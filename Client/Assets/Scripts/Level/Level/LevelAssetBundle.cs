using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SU.Manager;

public class LevelAssetBundle {

    private Dictionary<string, Object> objs = new Dictionary<string, Object>();
    private string name;
    private AssetBundle bundle;

    public void Initialize(string _name)
    {
        name = _name;
    }

    public Object GetAsset(string assetName)
    {
        Object obj = null;
        if (objs.TryGetValue(assetName, out obj))
        {
            return obj;
        }
        if (bundle == null)
        {
            var resMgr = Game.GetManager<GResManager>();
            bundle = resMgr.LoadAssetBundle(name);
        }
        return bundle.LoadAsset(assetName);
    }

    public void Unload()
    {
        if (bundle != null)
        {
            var resMgr = Game.GetManager<GResManager>();
            resMgr.UnLoadAssetBundle(name);
            bundle = null;
        }
    }
}
