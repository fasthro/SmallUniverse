using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class AssetPath
    {
        public string assetPath;
        public string bundleName;
        public string assetName;

        public AssetPath(string assetPath)
        {
            this.assetPath = assetPath;

            string[] aps = assetPath.Split('/');
            if (aps.Length == 3)
            {
                bundleName = aps[0] + "/" + aps[1];
                assetName = aps[2];
            }
            else
            {
				Debug.LogError("AssetPath -> error! [ " + assetPath + " ]");
            }
        }

        public static AssetPath Get(string assetPath)
        {
            return new AssetPath(assetPath);
        }
    }
}

