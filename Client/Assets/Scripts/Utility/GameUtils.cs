using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class GameUtils
    {
		/// <summary>
        /// 设置 GameObject layer
        /// </summary>
        /// <param name="go"></param>
		/// <param name="layer"></param>
		public static void SetGameObjectLayer(GameObject go, int layer)
		{
			go.layer = layer;

			for(int i = 0; i < go.transform.childCount; i++)
			{
				SetGameObjectLayer(go.transform.GetChild(i).gameObject, layer);
			}
		}
    }
}
