using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmallUniverse.Behaviour;

namespace SmallUniverse
{
    public class Hero : GameBehaviour {
        public HeroTransform heroTransform;

        public static Hero Create(string _heroName)
        {
            GameObject go = new GameObject();
            go.name = "Hero_" + _heroName ;
            var hero = go.AddComponent<Hero>();
            hero.Initialize(_heroName);
            return hero;
        }

        private void Initialize(string characterName)
        {
            CreateHero(characterName);
        }

        public void Born(LevelPoint point)
        {
            heroTransform.transform.position = point.position;
            heroTransform.transform.localEulerAngles = point.rotationAngle;
            heroTransform.gameObject.SetActive(true);
        }

        private void CreateHero(string heroName)
        {
            GameObject prefab = LevelAsset.GetGameObject("heros/" + heroName, heroName);
            var heroGo = GameObject.Instantiate<GameObject>(prefab);
            heroGo.transform.parent = transform;
            heroGo.SetActive(false);
            heroTransform = heroGo.GetComponent<HeroTransform>();
        }
    }
}
