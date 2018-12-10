using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public static class GameObjectExtensions
    {
        /// <summary>
        /// 缓存池回收
        /// </summary>
        public static void Despawn(this GameObject gameObject)
        {
            Game.gamePool.Despawn(gameObject);
        }
    }
}
