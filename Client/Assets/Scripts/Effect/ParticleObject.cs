using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class ParticleObject : MonoBehaviour
    {
		private ParticleSystem[] particles;

		void Awake()
		{
			particles = gameObject.GetComponentsInChildren<ParticleSystem>();
		}

		public void Play()
		{
			for(int i = 0; i < particles.Length; i++)
			{
				particles[i].Play();
			}
		}
		
		public void RePlay()
		{
			for(int i = 0; i < particles.Length; i++)
			{
				particles[i].Stop();
				particles[i].Play();
			}
		}

		public void Pause()
		{
			for(int i = 0; i < particles.Length; i++)
			{
				particles[i].Pause();
			}
		}

		public void Stop()
		{
			for(int i = 0; i < particles.Length; i++)
			{
				particles[i].Stop();
			}
		}

		public void Clear()
		{
			for(int i = 0; i < particles.Length; i++)
			{
				particles[i].Clear();
			}
		}
    }
}

