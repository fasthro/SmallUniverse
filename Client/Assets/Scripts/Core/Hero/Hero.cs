using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmallUniverse.Behaviour;

namespace SmallUniverse
{
    public class Hero : MonoBehaviour {
        public HeroTransform heroTransform;

        public static Hero Create(string heroName, string resName)
        {
            GameObject go = new GameObject();
            go.name = "Hero_" + heroName ;
            var hero = go.AddComponent<Hero>();
            hero.Initialize(heroName, resName);
            return hero;
        }

        private void Initialize(string heroName, string resName)
        {
            CreateHero(heroName, resName);
        }

        public void Born(LevelPoint point)
        {
            heroTransform.transform.position = point.position;
            heroTransform.transform.localEulerAngles = point.rotationAngle;
            heroTransform.gameObject.SetActive(true);
        }

        private void CreateHero(string heroName, string resName)
        {
            GameObject prefab = LevelAsset.GetGameObject("hero/" + heroName, resName);
            var heroGo = GameObject.Instantiate<GameObject>(prefab);
            heroGo.transform.parent = transform;
            heroGo.SetActive(false);
            heroTransform = heroGo.GetComponent<HeroTransform>();
        }
    }
}
