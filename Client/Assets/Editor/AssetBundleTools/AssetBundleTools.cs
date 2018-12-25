using SmallUniverse.Utils;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SmallUniverse.GameEditor
{
    public class AssetBundleTools
    {
        public class BundleName
        {
            public string assetPath;
            public string bundlePath;

            public BundleName(string assetPath, string bundlePath)
            {
                this.assetPath = assetPath;
                this.bundlePath = bundlePath + "/";
            }
        }

        /// <summary>
        /// 设置资源asset bundle name
        /// </summary>
        [MenuItem("SmallUniverse/AssetsBundleTools/Set AssetBundle Name")]
        public static void SetAssetBundleName()
        {
            // 清理无用asset bundle name
            AssetDatabase.RemoveUnusedAssetBundleNames();

            // 标准资源
            BundleName[] standards = new BundleName[]
            {
                new BundleName("Art/Heros", "heros"),
                new BundleName("Art/Monsters", "monsters"),
                new BundleName("Art/Weapons", "weapons"),
                new BundleName("Art/Bullets", "bullets"),
                new BundleName("Art/Effects", "effects"),
            };
            SetAssetBundleNameToStandard(standards);

            // level
            SetAssetBundleNameToLevelPrefab();
            SetAssetBundleNameToLevelScene();

            // skybox
            SetAssetBundleNameToSkybox();

            // csv
            SetAssetBundleNameToCSV();

            Debug.Log("设置资源 assetBundleName 完成!");
        }

        /// <summary>
        /// 设置关卡资源库 bundle name
        /// </summary>
        private static void SetAssetBundleNameToLevelPrefab()
        {
            string rootDir = Path.Combine(Application.dataPath, "Levels/Prefabs");
            if (!Directory.Exists(rootDir))
                return;

            string[] prefabDirs = Directory.GetDirectories(rootDir, "*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < prefabDirs.Length; i++)
            {
                var prefabDir = prefabDirs[i];
                var prefabName = PathUtils.GetPathSection(prefabDir, -1);

                string[] filePaths = Directory.GetFiles(prefabDir, "*.prefab", SearchOption.TopDirectoryOnly);
                for (int k = 0; k < filePaths.Length; k++)
                {
                    SetAssetBundleName(filePaths[k], "levels/prefabs/" + prefabName);
                }
            }
        }

        /// <summary>
        /// 设置关卡场景配置 bundle name
        /// </summary>
        private static void SetAssetBundleNameToLevelScene()
        {
            string rootDir = Path.Combine(Application.dataPath, "Levels/Scenes");
            if (!Directory.Exists(rootDir))
                return;

            string[] levelDirs = Directory.GetDirectories(rootDir, "*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < levelDirs.Length; i++)
            {
                var levelDir = levelDirs[i];
                var levelName = PathUtils.GetPathSection(levelDir, -1);

                var mapPath = Path.Combine(levelDir, "Map.xml");
                var navmeshPath = Path.Combine(levelDir, "Navmesh.prefab");

                SetAssetBundleName(mapPath, "levels/scenes/" + levelName);
                SetAssetBundleName(navmeshPath, "levels/scenes/" + levelName);
            }
        }

        /// <summary>
        /// 设置标准资源 bundle name
        /// </summary>
        private static void SetAssetBundleNameToStandard(BundleName[] standards)
        {
            for (int i = 0; i < standards.Length; i++)
            {
                string rootDir = Path.Combine(Application.dataPath, standards[i].assetPath);
                if (Directory.Exists(rootDir))
                {
                    string[] heroDirs = Directory.GetDirectories(rootDir, "*", SearchOption.TopDirectoryOnly);
                    for (int k = 0; k < heroDirs.Length; k++)
                    {
                        var dir = heroDirs[k];
                        var prefabDir = Path.Combine(dir, "Prefabs");
                        if (Directory.Exists(prefabDir))
                        {
                            var name = PathUtils.GetPathSection(dir, -1);
                            string[] filePaths = Directory.GetFiles(prefabDir, "*.prefab", SearchOption.TopDirectoryOnly);
                            for (int m = 0; m < filePaths.Length; m++)
                            {
                                SetAssetBundleName(filePaths[m], standards[i].bundlePath + name);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 设置 天空盒 bundle name
        /// </summary>
        private static void SetAssetBundleNameToSkybox()
        {
            string rootDir = Path.Combine(Application.dataPath, "Art/Skyboxs");
            if (!Directory.Exists(rootDir))
                return;

            string[] skyboxDirs = Directory.GetDirectories(rootDir, "*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < skyboxDirs.Length; i++)
            {
                var skyboxDir = skyboxDirs[i];
                var skyboxName = PathUtils.GetPathSection(skyboxDir, -1);
                string[] filePaths = Directory.GetFiles(skyboxDir, "*.mat", SearchOption.TopDirectoryOnly);
                for (int k = 0; k < filePaths.Length; k++)
                {
                    SetAssetBundleName(filePaths[k], "skyboxs/" + skyboxName);
                }
            }
        }

        /// <summary>
        /// 设置 csv bundle name
        /// </summary>
        private static void SetAssetBundleNameToCSV()
        {
            string rootDir = Path.Combine(Application.dataPath, "Data/CSV");
            if (!Directory.Exists(rootDir))
                return;

            string[] filePaths = Directory.GetFiles(rootDir, "*.csv", SearchOption.TopDirectoryOnly);
            for (int k = 0; k < filePaths.Length; k++)
            {   
                SetAssetBundleName(filePaths[k], "csv/" + Path.GetFileNameWithoutExtension(filePaths[k]));
            }
        }
        

        /// <summary>
        /// 设置资源文件 bundle name
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="bundleName"></param>
        private static void SetAssetBundleName(string filePath, string bundleName)
        {
            if (!File.Exists(filePath))
            {
                Debug.LogError("设置资源 assetBundleName 时，资源路径不存在!" + filePath);
                return;
            }

            string pPath = PathUtils.GetTopPath(Application.dataPath);
            string assetPath = filePath.Substring(pPath.Length + 1, filePath.Length - pPath.Length - 1);

            AssetImporter import = AssetImporter.GetAtPath(assetPath);
            if (import != null)
            {
                import.assetBundleName = bundleName;
            }
        }

    }
}
