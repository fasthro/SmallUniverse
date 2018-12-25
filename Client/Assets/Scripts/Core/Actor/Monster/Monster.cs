using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class Monster : ActorBase
    {
        private CSV_Monster m_dataCSV;

		public static Monster Create(int monsterId)
        {
            GameObject go = new GameObject();
            go.name = "Monster_" + monsterId;
            var monster = go.AddComponent<Monster>();
            monster.Initialize(monsterId);
            return monster;
        }

		private void Initialize(int monsterId)
        {
            base.InitActorData();

            m_dataCSV = Game.gameCSV.GetData<CSV_Monster>(monsterId);

            GameObject prefab = LevelAsset.GetGameObject(m_dataCSV.art);
            var heroGo = GameObject.Instantiate<GameObject>(prefab);
            heroGo.transform.parent = transform;
            heroGo.SetActive(false);
            actorGameObject = heroGo.GetComponent<ActorGameObject>();
        }
    }
}
