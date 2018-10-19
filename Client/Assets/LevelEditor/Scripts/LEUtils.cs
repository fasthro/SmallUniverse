using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SU.Editor.LevelEditor
{
    public class LEUtils
    {

        public static T CreateScriptable<T>() where T : ScriptableObject
        {
            T newScriptable = ScriptableObject.CreateInstance<T>();
#if UNITY_EDITOR
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path.Length == 0)
            {
                path = "Assets/";
            }
            else
            {
                int lastPos = path.LastIndexOf("/");
                path = path.Substring(0, lastPos + 1);
            }

            string className = typeof(T).Name;
            path = AssetDatabase.GenerateUniqueAssetPath(path + "/" + className + ".asset");
            AssetDatabase.CreateAsset(newScriptable, path);

#endif
            return newScriptable;
        }

        /// <summary>
        ///  加载模版
        /// </summary>
        /// <param name="templateName"></param>
        /// <returns></returns>
        public static string LoadTemplate(string templateName)
        {
#if UNITY_EDITOR
            string path = Path.Combine("Assets/LevelEditor/Templates/", templateName);
            Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(TextAsset));
            TextAsset asset = obj as TextAsset;
            return asset.text;
#else
            return "";
#endif
        }


        /// <summary>
        /// 字符串首字母大写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToUpperFirstChar(string str)
        {
            string outStr = "";
            char[] chars = str.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (i == 0)
                {
                    outStr += chars[i].ToString().ToUpper();
                }
                else {
                    outStr += chars[i].ToString();
                }
            }
            return outStr;
        }

        /// <summary>
        /// 获取Assets/下面的完整路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetPathToAssets(string path)
        {
            return Path.Combine("Assets", path);
        }

        /// <summary>
        /// 获取关卡场景保存目录
        /// </summary>
        /// <param name="levelSceneName"></param>
        /// <returns></returns>
        public static string GetLevelSceneDirectory(string levelSceneName, bool absolute = true)
        {
            var directory = Path.Combine(LESetting.LevelSceneDirectory, levelSceneName);
            if (absolute)
                return Path.Combine(Application.dataPath, directory);
            return Path.Combine(LESetting.LevelSceneDirectory, levelSceneName);
        }

        /// <summary>
        /// 获取关卡场景保存路径
        /// </summary>
        /// <param name="levelSceneName"></param>
        /// <returns></returns>
        public static string GetLevelScenePath(string levelSceneName, bool absolute = true)
        {
            return Path.Combine(GetLevelSceneDirectory(levelSceneName, absolute), levelSceneName + ".unity");
        }

        /// <summary>
        /// 获取关卡数据保存路径
        /// </summary>
        /// <param name="levelSceneName"></param>
        /// <returns></returns>
        public static string GetLevelDataPath(string levelSceneName, bool absolute = true)
        {
            return Path.Combine(GetLevelSceneDirectory(levelSceneName, absolute), levelSceneName + ".xml");
        }
    }
}