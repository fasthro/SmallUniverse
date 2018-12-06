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
        Attack,
        AttackEnd,
    }

    public abstract class ActorBase : MonoBehaviour
    {
        public ActorGameObject actorGameObject;
        public ActorAttribute attribute;
        protected Animator m_animator;
        protected ActorAnimationEvent m_animationEvent;
        protected Rigidbody m_rigidbody;
        protected NavMeshAgent m_navMeshAgent;
        protected WeaponBase m_weapon;

        // 是否已经出生
        protected bool m_isBorn;
        public bool IsBorn
        {
            get
            {
                return m_isBorn;
            }
        }

        // 当前状态
        public ActorState actorState;

        // 技能数据
        protected SkillData m_skillData;

        // 目标方向
        protected Vector3 m_targetDir;
        // 移动方向
        protected Vector3 m_moveDir;
        // 当前朝向
        protected Vector3 m_lookDir;

        // 是否输入攻击
        protected bool m_attackInput;


        void Update()
        {
            if (actorGameObject == null)
                return;

            OnUpdate();
        }

        void LateUpdate()
        {
            if (actorGameObject == null)
                return;

            OnLateUpdate();
        }


        /// <summary>
        /// 初始化数据
        /// </summary>
        public void InitActorData()
        {
            attribute = ActorAttribute.Create();
            attribute.SetAttribute(ActorAttributeType.MoveSpeed, 3);
            attribute.SetAttribute(ActorAttributeType.AttackSpeed, 2);
        }

        /// <summary>
        /// 出生
        /// </summary>
        public virtual void Born(LevelPoint point)
        {
            m_isBorn = true;
            m_animator = actorGameObject.GetComponent<Animator>();
            m_animationEvent = actorGameObject.GetComponent<ActorAnimationEvent>();
            m_rigidbody = actorGameObject.GetComponent<Rigidbody>();
            m_navMeshAgent = actorGameObject.GetComponent<NavMeshAgent>();

            actorGameObject.transform.position = point.position;
            actorGameObject.transform.localEulerAngles = point.rotationAngle;
            actorGameObject.gameObject.SetActive(true);

            m_moveDir = actorGameObject.transform.forward;

            // 加载武器
            LoadWeapon();
        }

        // 加载武器
        public virtual void LoadWeapon()
        {

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
        public virtual void Attack(SkillData skillData)
        {
            m_skillData = skillData;
            m_attackInput = true;
        }

        /// <summary>
        /// 停止攻击
        /// </summary>
        public virtual void StopAttack()
        {
            m_attackInput = false;
        }

        /// <summary>
        /// 被攻击
        /// </summary>
        public virtual void BeAttack(AttackData attackData)
        {

        }

        /// <summary>
        /// 角色方向
        /// </summary>
        public virtual void UpdateDirection()
        {
            actorGameObject.transform.rotation = Quaternion.LookRotation(m_moveDir, Vector3.up);
        }

        /// <summary>
        /// 状态刷新
        /// </summary>
        protected virtual void UpdateState()
        {

        }

        protected virtual void OnUpdate()
        {
            if (m_isBorn)
            {
                UpdateDirection();
                UpdateState();
            }
        }

        protected virtual void OnLateUpdate()
        {

        }

    }
}
