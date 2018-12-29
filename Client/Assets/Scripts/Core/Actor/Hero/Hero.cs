/*
 * @Author: fasthro
 * @Date: 2018-12-27 18:03:13
 * @Description: 英雄
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmallUniverse.Behaviour;

namespace SmallUniverse
{
    public class Hero : ActorBase
    {
        private CSV_Hero m_dataCSV;
        
        // 是否连续攻击
        private bool continuouAttack;
        
        public static Hero Create(int heroId)
        {
            GameObject go = new GameObject();
            go.name = "Hero_" + heroId;
            var hero = go.AddComponent<Hero>();
            hero.Initialize(heroId);
            return hero;
        }

        private void Initialize(int heroId)
        {
            base.InitActorData();

            m_dataCSV = Game.gameCSV.GetData<CSV_Hero>(heroId);

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

        public override void Born(LevelPoint point)
        {
            base.Born(point, true, true);
        }

        public override void Attack()
        {
            if (actorState == ActorState.Attack)
            {
                continuouAttack = true;
            }

            if (actorState == ActorState.Attack || actorState == ActorState.Death)
                return;

            actorState = ActorState.Attack;
            m_animator.SetFloat(ActorAnimatorParameters.AttackSpeed.ToString(), attribute.GetAttribute(ActorAttributeType.AttackSpeed));
            m_animator.SetBool(ActorAnimatorParameters.Attack.ToString(), true);
        }

        protected override void OnEndHandler()
        {
            if (actorState == ActorState.Attack)
            {
                actorState = ActorState.Idle;
                if (continuouAttack)
                {
                    Attack();
                }
                else
                {
                    m_animator.SetBool(ActorAnimatorParameters.Attack.ToString(), false);
                    m_weapon.StopAttack();
                }
                continuouAttack = false;
            }
        }

        protected override void OnAttackHandler()
        {
            if (actorState == ActorState.Attack)
            {
                var attackData = new AttackData();
                attackData.layer = GameLayer.NameToLayer(GameLayer.HERO);
                attackData.attack = attribute.GetAttribute(ActorAttributeType.Attack);
                attackData.magicAttack = attribute.GetAttribute(ActorAttributeType.MagicAttack);
                m_weapon.Attack(attackData, null);
            }
        }
    }
}
