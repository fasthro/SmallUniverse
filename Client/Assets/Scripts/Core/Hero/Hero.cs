using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmallUniverse.Behaviour;

namespace SmallUniverse
{
    public class Hero : ActorBase 
    {
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
            base.InitActorData();
            
            GameObject prefab = LevelAsset.GetGameObject("hero/" + heroName, resName);
            var heroGo = GameObject.Instantiate<GameObject>(prefab);
            heroGo.transform.parent = transform;
            heroGo.SetActive(false);
            actorGameObject = heroGo.GetComponent<ActorGameObject>();
        }

        public override void Born(LevelPoint point)
        {
            base.Born(point);
        }

        public override void Move(Vector3 move, float delta)
        {
            base.Move(move, delta);
        }
    }
}
