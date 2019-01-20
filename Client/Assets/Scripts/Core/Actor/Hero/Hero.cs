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
        }

        public override void Born(LevelPoint point)
        {
            base.Born(point, m_dataCSV.art);
            canMove = true;
            canAttack = true;
            attack = false;
        }

        #region  move
        public override void Move(Vector3 move, bool isMove)
        {
            if (isMove)
            {
                if (canMove)
                {
                    m_moveVt = move;

                    m_animator.SetFloat(ActorAnimatorParameters.MoveSpeed, m_moveVt.magnitude);
                    m_animator.SetBool(ActorAnimatorParameters.Move, true);

                    if (move.magnitude > 0)
                    {
                        m_dirVt = move.normalized;
                        m_dirVt.y = 0;
                    }
                }
                else
                {
                    SetCanMove(canMove);
                }
            }
            else
            {
                SetCanMove(canMove);
            }
        }

        public override void SetCanMove(bool can)
        {
            canMove = can;
            m_moveVt = Vector3.zero;
            m_animator.SetBool(ActorAnimatorParameters.Move, false);
        }
        #endregion

        #region  attack
        public override void Attack(int attackIndex)
        {
            if(canAttack && !attack)
            {
                m_animator.SetTrigger(ActorAnimatorParameters.Attack + attackIndex.ToString());
            }
        }

        public override void SetCanAttack(bool can)
        {
            canAttack = can;
        }
        #endregion

        public override void OnUpdate()
        {
            // position
            actorGameObject.transform.position = actorGameObject.transform.position + m_moveVt * attribute.GetAttribute(ActorAttributeType.MoveSpeed) * Time.deltaTime;

            // rotation
            m_targetRotation = Quaternion.LookRotation(m_dirVt, Vector3.up);
            float rotationSpeed = attribute.GetAttribute(ActorAttributeType.RotationSpeed);
            actorGameObject.transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(actorGameObject.transform.eulerAngles.y, m_targetRotation.eulerAngles.y, (rotationSpeed * Time.deltaTime) * rotationSpeed);
        }

        public override void OnLateUpdate()
        {

        }

        public override void OnFixedUpdate()
        {

        }
    }
}
