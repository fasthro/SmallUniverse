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

            // 设置属性
            attribute.SetAttribute(ActorAttributeType.MoveSpeed, m_dataCSV.moveSpeed);
            attribute.SetAttribute(ActorAttributeType.AttackSpeed, m_dataCSV.attackSpeed);
            attribute.SetAttribute(ActorAttributeType.RotationSpeed, m_dataCSV.rotationSpeed);
            attribute.SetAttribute(ActorAttributeType.Range, m_dataCSV.range);
            attribute.SetAttribute(ActorAttributeType.Attack, m_dataCSV.attack);
            attribute.SetAttribute(ActorAttributeType.MagicAttack, m_dataCSV.magicAttack);
            attribute.SetAttribute(ActorAttributeType.Defense, m_dataCSV.defense);
            attribute.SetAttribute(ActorAttributeType.MagicDefense, m_dataCSV.magicDefense);
            attribute.SetAttribute(ActorAttributeType.Hp, m_dataCSV.hp);
            attribute.SetAttribute(ActorAttributeType.HpMax, m_dataCSV.hpMax);

            GameObject prefab = LevelAsset.GetGameObject(m_dataCSV.art);
            var heroGo = GameObject.Instantiate<GameObject>(prefab);
            heroGo.transform.parent = transform;
            heroGo.SetActive(false);
            actorGameObject = heroGo.GetComponent<ActorGameObject>();
        }
    }
}
