using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class PoolExample : MonoBehaviour
    {
        
        private bool die = false;
		private float delay = 5f;

        // Use this for initialization
        void OnEnable()
        {
            die = false;
			delay = 5f;
        }

        // Update is called once per frame
        void Update()
        {
			delay -= Time.deltaTime;
			if(delay <= 0 && !die)
			{
                die = true;
				Game.gamePool.Despawn(gameObject);
			}
        }
    }
}

