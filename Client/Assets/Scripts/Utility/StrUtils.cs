using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SU.Utils
{
    public class StrUtils
    {
        /// <summary>
        /// 字符串首字母转成大写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToFirstUpper(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                if (str.Length > 1)
                {
                    return char.ToUpper(str[0]) + str.Substring(1);
                }
                return str.ToUpper();
            }
            return str;
        }

        /// <summary>
        /// 字符串首字母转成小写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToFirstLower(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                if (str.Length > 1)
                {
                    return char.ToLower(str[0]) + str.Substring(1);
                }
                return str.ToLower();
            }
            return str;
        }
    }
}
