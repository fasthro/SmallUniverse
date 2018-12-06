using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
	public enum WeaponType
	{
		Gun,
	}

    public abstract class WeaponBase : MonoBehaviour
    {
		// 武器类型
		public WeaponType weaponType;
    }
}
