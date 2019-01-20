/*
 * @Author: fasthro
 * @Date: 2018-12-27 18:03:13
 * @Description: 怪
 */
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace SmallUniverse
{
    public class Monster : ActorBase
    {
        // 数据
        private CSV_Monster m_dataCSV;

        // AI 行为
        private BehaviorTree m_behaviorTree;

        // 技能
        private SkillBase m_skill;

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
        }

        public override void Born(LevelPoint point)
        {
            base.Born(point, m_dataCSV.art);

            // 怪行为设置
            m_behaviorTree = actorGameObject.gameObject.AddComponent<BehaviorTree>();
            m_behaviorTree.StartWhenEnabled = false;
            m_behaviorTree.RestartWhenComplete = true;
            m_behaviorTree.ExternalBehavior = LevelAsset.GetExternalBehaviorTree(m_dataCSV.behavior);
            m_behaviorTree.SetVariableValue("Target", Game.hero.actorGameObject.gameObject.transform);
            m_behaviorTree.EnableBehavior();
        }
    }
}
