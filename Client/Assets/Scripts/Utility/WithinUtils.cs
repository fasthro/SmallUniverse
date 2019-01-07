/*
 * @Author: fasthro
 * @Date: 2019-01-07 18:20:45
 * @Description: 判断点是否在图形的范围内
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse.Utils
{
    public class WithinUtils
    {
        /// <summary>
        /// 判断目标点是否在圆内
        /// </summary>
        /// <param name="center">圆心</param>
        /// <param name="position">目标点</param>
        /// <param name="radius">半径</param>
        /// <returns></returns>
        public static bool WithinRound(Vector3 center, Vector3 position, float radius)
        {
            return Vector3.Distance(center, position) <= radius;
        }
    }
}
