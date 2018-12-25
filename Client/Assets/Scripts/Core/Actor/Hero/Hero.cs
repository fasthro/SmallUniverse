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

            GameObject prefab = LevelAsset.GetGameObject(m_dataCSV.art);
            var heroGo = GameObject.Instantiate<GameObject>(prefab);
            heroGo.transform.parent = transform;
            heroGo.SetActive(false);
            actorGameObject = heroGo.GetComponent<ActorGameObject>();
        }

        public override void Born(LevelPoint point)
        {
            base.Born(point);

            m_animationEvent.OnEndHandler -= OnAnimationEndHandler;
            m_animationEvent.OnEndHandler += OnAnimationEndHandler;

            m_animationEvent.OnAttackHandler -= OnAnimationAttackHandler;
            m_animationEvent.OnAttackHandler += OnAnimationAttackHandler;
        }

        public override void LoadWeapon()
        {
            GameObject prefab = LevelAsset.GetGameObject("Weapons/Rifle/Rifle");
            var weaponGo = GameObject.Instantiate<GameObject>(prefab);
            weaponGo.transform.parent = actorGameObject.WeaponBone;
            weaponGo.transform.localPosition = Vector3.zero;
            weaponGo.transform.localRotation = Quaternion.Euler(90, 0, 0);

            m_weapon = weaponGo.GetComponent<WeaponBase>();
            m_weapon.Initialize(this);
        }

        public override void Attack(SkillData skillData)
        {
            if (actorState != ActorState.Attack)
            {
                base.Attack(skillData);
                actorState = ActorState.Attack;
                m_animator.SetFloat(ActorAnimatorParameters.AttackSpeed.ToString(), attribute.GetAttribute(ActorAttributeType.AttackSpeed));
                m_animator.SetBool(ActorAnimatorParameters.Attack.ToString(), true);
            }
        }

        void OnAnimationEndHandler()
        {
            if (actorState == ActorState.Attack)
            {
                actorState = ActorState.AttackEnd;
            }
        }

        void OnAnimationAttackHandler()
        {
            if (actorState == ActorState.Attack)
            {
               m_weapon.Attack();
            }
        }

        protected override void UpdateState()
        {
            if (actorState == ActorState.None)
            {
                m_animator.SetBool(ActorAnimatorParameters.Attack.ToString(), false);
            }
            else if (actorState == ActorState.AttackEnd)
            {
                if (m_attackInput)
                {
                    Attack(m_skillData);
                }
                else
                {
                    actorState = ActorState.None;
                }
            }
        }
    }
}
