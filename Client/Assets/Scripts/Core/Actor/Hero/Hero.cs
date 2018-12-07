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
            go.name = "Hero_" + heroName;
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

            m_animationEvent.OnEndHandler -= OnAnimationEndHandler;
            m_animationEvent.OnEndHandler += OnAnimationEndHandler;
        }

        public override void LoadWeapon()
        {
            GameObject prefab = LevelAsset.GetGameObject("weapon/scifirifle", "SciFiRifle");
            var weaponGo = GameObject.Instantiate<GameObject>(prefab);
            weaponGo.transform.parent = actorGameObject.WeaponBone;
            weaponGo.transform.localPosition = Vector3.zero;
            weaponGo.transform.localRotation = Quaternion.Euler(90, 0, 0);

            m_weapon = weaponGo.GetComponent<WeaponBase>();
            m_weapon.Initialize(this);
        }

        public override void Move(Vector3 move, float delta)
        {
            base.Move(move, delta);
        }

        public override void Attack(SkillData skillData)
        {
            if (actorState != ActorState.Attack)
            {
                base.Attack(skillData);
                actorState = ActorState.Attack;
                m_animator.SetFloat(ActorAnimatorParameters.AttackSpeed.ToString(), attribute.GetAttribute(ActorAttributeType.AttackSpeed));
                m_animator.SetBool(ActorAnimatorParameters.Attack.ToString(), true);

                m_weapon.Attack();
            }
        }

        public override void StopAttack()
        {
            base.StopAttack();
        }

        public override void BeAttack(AttackData attackData)
        {
            base.BeAttack(attackData);
        }

        void OnAnimationEndHandler()
        {
            if (actorState == ActorState.Attack)
            {
                actorState = ActorState.AttackEnd;
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
