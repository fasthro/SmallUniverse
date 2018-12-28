using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SmallUniverse
{

    public enum ActorAnimatorParameters
    {
        Speed,
        Attack,
        AttackSpeed,
        Death,
    }

    public enum ActorState
    {
        None,
        Idle,
        Attack,
        Death,
    }

    public abstract class ActorBase : MonoBehaviour
    {
        public ActorGameObject actorGameObject;
        public ActorAttribute attribute;
        protected Animator m_animator;
        protected ActorAnimationEvent m_animationEvent;
        protected Rigidbody m_rigidbody;
        protected NavMeshAgent m_navMeshAgent;
        // 武器
        protected WeaponBase m_weapon;
        // 生命条
        protected UP_HudSceneHpBar m_HPBar;

        // 当前状态
        public ActorState actorState;

        // 目标方向
        protected Vector3 m_targetDir;
        // 移动方向
        protected Vector3 m_moveDir;
        // 当前朝向
        protected Vector3 m_lookDir;

        /// <summary>
        /// 初始化数据
        /// </summary>
        public void InitActorData()
        {
            actorState = ActorState.None;
            attribute = ActorAttribute.Create();
        }

        protected virtual void OnEndHandler()
        {
        
        }

        protected virtual void OnAttackHandler()
        {
        
        }

        /// <summary>
        /// 出生
        /// </summary>
        public virtual void Born(LevelPoint point)
        {
        
        }

        /// <summary>
        /// 出生
        /// </summary>
        /// <param name="haveHPBar"></param>
        /// <param name="haveWeapon"></param>
        protected void Born(LevelPoint point, bool haveHPBar, bool haveWeapon)
        {
            actorState = ActorState.Idle;

            m_animator = actorGameObject.GetComponent<Animator>();
            m_animationEvent = actorGameObject.GetComponent<ActorAnimationEvent>();
            m_rigidbody = actorGameObject.GetComponent<Rigidbody>();
            m_navMeshAgent = actorGameObject.GetComponent<NavMeshAgent>();

            // 注册事件
            m_animationEvent.OnEndHandler -= OnEndHandler;
            m_animationEvent.OnEndHandler += OnEndHandler;
            m_animationEvent.OnAttackHandler -= OnAttackHandler;
            m_animationEvent.OnAttackHandler += OnAttackHandler;

            actorGameObject.transform.position = point.position;
            actorGameObject.transform.localEulerAngles = point.rotationAngle;
            actorGameObject.gameObject.SetActive(true);

            m_moveDir = actorGameObject.transform.forward;
 
            // HP Bar
            if (haveHPBar)
            {
                m_HPBar = new UP_HudSceneHpBar();
                m_HPBar.SetAlign(UIPrefabAlign.CENTER);
                m_HPBar.SetFollow(actorGameObject.headPoint);
                m_HPBar.SetLookAt(Game.gameCamera.heroCamera.virtualCamera.transform);
                m_HPBar.SetValue(attribute.GetAttribute(ActorAttributeType.Hp), attribute.GetAttribute(ActorAttributeType.HpMax));
            }

            // weapon
            if (haveWeapon)
            {
                GameObject prefab = LevelAsset.GetGameObject("Weapons/Rifle/Rifle");
                var weaponGo = GameObject.Instantiate<GameObject>(prefab);
                weaponGo.transform.parent = actorGameObject.weaponPoint;
                weaponGo.transform.localPosition = Vector3.zero;
                weaponGo.transform.localRotation = Quaternion.Euler(90, 0, 0);

                m_weapon = weaponGo.GetComponent<WeaponBase>();
                m_weapon.Initialize(this);
            }

            // 出生动画
            m_animator.SetFloat(ActorAnimatorParameters.Speed.ToString(), 0);
        }

        /// <summary>
        /// 死亡
        /// </summary>
        public virtual void Death()
        {
            if(actorState == ActorState.Death)
                return;

            actorState = ActorState.Death;
            m_animator.SetBool(ActorAnimatorParameters.Death.ToString(), true);
        }

        /// <summary>
        /// 移动
        /// </summary>
        public virtual void Move(Vector3 move, float delta)
        {
            Vector3 vector = move * attribute.GetAttribute(ActorAttributeType.MoveSpeed);
            actorGameObject.transform.position = actorGameObject.transform.position + vector * delta;

            if (vector.magnitude > 0)
            {
                m_moveDir = vector;
                m_moveDir.y = 0;
                m_moveDir.Normalize();
            }

            m_animator.SetFloat(ActorAnimatorParameters.Speed.ToString(), vector.magnitude);
        }

        /// <summary>
        /// 攻击
        /// </summary>
        public virtual void Attack()
        {

        }

        /// <summary>
        /// 被攻击受伤
        /// </summary>
        public virtual void BeAttack(AttackData attackData)
        {
            float defense = attribute.GetAttribute(ActorAttributeType.Defense);
            float damage = ((defense * 0.06f) / (1 + 0.06f * defense)) * attackData.attack;
            float sub = attribute.GetAttribute(ActorAttributeType.Hp) - damage;
            attribute.SetAttribute(ActorAttributeType.Hp, sub > 0 ? sub : 0);

            // 死亡判断
            if (sub <= 0)
            {
                Death();
            }

            // 设置血条显示
            if (m_HPBar != null)
            {
                m_HPBar.SetValue(attribute.GetAttribute(ActorAttributeType.Hp), attribute.GetAttribute(ActorAttributeType.HpMax));
            }
        }

        /// <summary>
        /// 摧毁自己
        /// </summary>
        protected virtual void DestroySelf()
        {
            Destroy(gameObject);
        }

        protected virtual void OnUpdate()
        {
            if (actorState == ActorState.None)
                return;

            actorGameObject.transform.rotation = Quaternion.LookRotation(m_moveDir, Vector3.up);
        }

        protected virtual void OnLateUpdate()
        {
            if (actorState == ActorState.None)
                return;

        }

        void Update()
        {
            OnUpdate();
        }

        void LateUpdate()
        {
            OnLateUpdate();
        }
    }
}
