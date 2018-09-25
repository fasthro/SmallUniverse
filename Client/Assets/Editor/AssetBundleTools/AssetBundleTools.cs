using SU.Utils;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SU.Editor
{
    public class AssetBundleTools
    {
        /// <summary>
        /// 设置资源asset bundle name
        /// </summary>
        [MenuItem("SU/AssetsBundleTools/Set AssetBundle Name")]
        public static void SetAssetBundleName()
        {
            // 清理无用asset bundle name
            AssetDatabase.RemoveUnusedAssetBundleNames();
            
            // level
            SetAssetBundleNameToLevelRepository();
            SetAssetBundleNameToLevelScene();

            Debug.Log("设置资源 assetBundleName 完成!");
        }

        /// <summary>
        /// 设置关卡资源库 bundle name
        /// </summary>
        private static void SetAssetBundleNameToLevelRepository()
        {
            string rootDir = Path.Combine(Application.dataPath, "Levels/Repository");
            if (!Directory.Exists(rootDir))
                return;

            string[] repositoryDirs = Directory.GetDirectories(rootDir, "*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < repositoryDirs.Length; i++)
            {
                var repositoryDir = repositoryDirs[i];
                var repositoryName = PathUtils.GetPathSection(repositoryDir, -1);

                string[] filePaths = Directory.GetFiles(repositoryDir, "*.prefab", SearchOption.TopDirectoryOnly);
                for (int k = 0; k < filePaths.Length; k++)
                {
                    SetAssetBundleName(filePaths[k], "level/repository/" + repositoryName);
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
                var levelPath = Path.Combine(levelDir, levelName + ".xml");

                SetAssetBundleName(levelPath, "level/scene/" + levelName);
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
