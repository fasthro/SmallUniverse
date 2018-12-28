using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class Monster : ActorBase
    {
        private CSV_Monster m_dataCSV;

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

            GameObject prefab = LevelAsset.GetGameObject(m_dataCSV.art);
            var heroGo = GameObject.Instantiate<GameObject>(prefab);
            heroGo.transform.parent = transform;
            heroGo.SetActive(false);
            actorGameObject = heroGo.GetComponent<ActorGameObject>();
        }

        public override void Born(LevelPoint point)
        {
            base.Born(point, true, false);

            m_skill = actorGameObject.GetComponent<SkillBase>();
            m_skill.Initialize(this);
        }

        public override void Attack()
        {
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
                m_animator.SetBool(ActorAnimatorParameters.Attack.ToString(), false);

                if (m_weapon != null)
                {
                    m_weapon.StopAttack();
                }

            }
            else if (actorState == ActorState.Death)
            {
                DestroySelf();
            }
        }

        protected override void OnAttackHandler()
        {
            if (actorState == ActorState.Attack)
            {
                var attackData = new AttackData();
                attackData.layer = GameLayer.NameToLayer(GameLayer.MONSTER);
                attackData.attack = attribute.GetAttribute(ActorAttributeType.Attack);
                attackData.magicAttack = attribute.GetAttribute(ActorAttributeType.MagicAttack);

                if (m_weapon != null)
                {
                    m_weapon.Attack(attackData, null);
                }
                else
                {
                    m_skill.Attack(attackData, null);
                }
            }
        }
    }
}
