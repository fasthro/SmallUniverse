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
        /// <summary>
        /// 设置资源asset bundle name
        /// </summary>
        [MenuItem("SmallUniverse/AssetsBundleTools/Set AssetBundle Name")]
        public static void SetAssetBundleName()
        {
            // 清理无用asset bundle name
            AssetDatabase.RemoveUnusedAssetBundleNames();
            
            // level
            SetAssetBundleNameToLevelPrefab();
            SetAssetBundleNameToLevelScene();

            // skybox
            SetAssetBundleNameToSkybox();

            // hero
            SetAssetBundleNameToHero();

            // weapon
            SetAssetBundleNameToWeapon();

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
        /// 设置 hero bundle name
        /// </summary>
        private static void SetAssetBundleNameToHero()
        {
            string rootDir = Path.Combine(Application.dataPath, "Art/Hero");
            if (!Directory.Exists(rootDir))
                return;

            string[] heroDirs = Directory.GetDirectories(rootDir, "*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < heroDirs.Length; i++)
            {
                var herolDir = heroDirs[i];
                var prefabDir = Path.Combine(herolDir, "Prefab");
                var heroName = PathUtils.GetPathSection(herolDir, -1);
                string[] filePaths = Directory.GetFiles(prefabDir, "*.prefab", SearchOption.TopDirectoryOnly);
                for (int k = 0; k < filePaths.Length; k++)
                {
                    SetAssetBundleName(filePaths[k], "hero/" + heroName);
                }
            }
        }

        /// <summary>
        /// 设置 weapon bundle name
        /// </summary>
        private static void SetAssetBundleNameToWeapon()
        {
            string rootDir = Path.Combine(Application.dataPath, "Art/Weapon");
            if (!Directory.Exists(rootDir))
                return;

            string[] weaponDirs = Directory.GetDirectories(rootDir, "*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < weaponDirs.Length; i++)
            {
                var weaponlDir = weaponDirs[i];
                var prefabDir = Path.Combine(weaponlDir, "Prefab");
                var weaponName = PathUtils.GetPathSection(weaponlDir, -1);
                string[] filePaths = Directory.GetFiles(prefabDir, "*.prefab", SearchOption.TopDirectoryOnly);
                for (int k = 0; k < filePaths.Length; k++)
                {
                    SetAssetBundleName(filePaths[k], "weapon/" + weaponName);
                }
            }
        }

        /// <summary>
        /// 设置 天空盒 bundle name
        /// </summary>
        private static void SetAssetBundleNameToSkybox()
        {
            string rootDir = Path.Combine(Application.dataPath, "Art/Skybox");
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
                    SetAssetBundleName(filePaths[k], "skybox/" + skyboxName);
                }
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
