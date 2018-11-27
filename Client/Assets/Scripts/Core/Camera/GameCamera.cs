using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmallUniverse.Manager;

namespace SmallUniverse
{
    public class GameCamera : MonoBehaviour
    {   
        public Skybox skybox;
        public VirtualCamera heroCamera;

        /// <summary>
        /// 设置天空盒
        /// </summary>
        /// <param name="skyName"></param>
        public void SetSkybox(string skyName)
        {
            var resMgr = Game.GetManager<GResManager>();
            var bundleName = "skybox/" + skyName.ToLower();
            var bundle = resMgr.LoadAssetBundle(bundleName);
            var material = bundle.LoadAsset(skyName) as Material;
            skybox.material = material;

            resMgr.UnLoadAssetBundle(bundleName);
        }
    }
}
