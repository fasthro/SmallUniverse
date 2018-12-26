using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class GameLayer
    {
		// 英雄层
		public static string HERO = "Hero";
		// 怪层
		public static string MONSTER = "Monster";

		public static int NameToLayer(string name)
		{
			return LayerMask.NameToLayer(name);
		}

		public static bool Compare(int layer, string name)
		{
			return NameToLayer(name) == layer;
		}
    }
}

