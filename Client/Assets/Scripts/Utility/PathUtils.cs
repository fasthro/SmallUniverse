using SmallUniverse.Define;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SmallUniverse.Utils
{
    public class PathUtils
    {
        /// <summary>
        /// 获取上级路径
        /// </summary>
        /// <param name="path"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public static string GetTopPath(string path, int top = 1)
        {
            path = ReplaceSeparator(path);
            char separator = Path.AltDirectorySeparatorChar;
            string[] ps = path.Split(separator);
            string outPath = "";

            if (ps.Length > top)
            {
                for (int i = 0; i < ps.Length - top; i++)
                {
                    if (string.IsNullOrEmpty(outPath))
                    {
                        outPath = ps[i];
                    }
                    else
                    {
                        outPath = outPath + separator + ps[i];
                    }
                }
                return outPath;
            }
            return path;
        }

        /// <summary>
        /// 获取路径上的第几个位置内容
        /// </summary>
        /// <param name="path"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string GetPathSection(string path, int index)
        {
            if (index == 0)
                return "";

            path = ReplaceSeparator(path);
            char separator = Path.AltDirectorySeparatorChar;
            string[] ps = path.Split(separator);

            if (index < 0)
            {
                index = ps.Length + index + 1;
            }

            if (ps.Length >= index)
            {
                return ps[index - 1];
            }

            return "";
        }

        /// <summary>
        /// 替换路径分隔符
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReplaceSeparator(string path, string separator = "")
        {
            if (string.IsNullOrEmpty(separator))
            {
                separator = Path.AltDirectorySeparatorChar.ToString();
            }
            return path.Replace("\\", separator).Replace("//", separator);
        }

        /// <summary>
        /// 获取 UIEditor 路径
        /// </summary>
        /// <returns></returns>
        public static string UIEditorPath()
        {
            var separator = Path.AltDirectorySeparatorChar.ToString();
            return GetTopPath(Application.dataPath, 2) + separator + "UIEditor" + separator;
        }

        /// <summary>
        /// 获取 UIEditor Assets 路径
        /// </summary>
        /// <returns></returns>
        public static string UIEditorAssetPath()
        {
            var separator = Path.AltDirectorySeparatorChar.ToString();
            return UIEditorPath () + "assets" + separator;
        }

        /// <summary>
        /// 获取数据目录
        /// </summary>
        /// <returns></returns>
        public static string GetDataPath()
        {
            var separator = Path.AltDirectorySeparatorChar.ToString();

            if (AppConst.DebugMode)
            {
                return Application.dataPath + separator + AppConst.AssetDirname + separator;
            }
            return "";
        }
    }
}

