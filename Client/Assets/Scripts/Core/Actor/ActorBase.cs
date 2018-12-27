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
        // 武器
        protected WeaponBase m_weapon;
        // 生命条
        protected UP_HudSceneHpBar m_hpBar;

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

            LoadWeapon();
            CreateHpBar();
        }

        /// <summary>
        /// 加载武器
        /// </summary>
        public virtual void LoadWeapon()
        {

        }

        /// <summary>
        /// 创建生命条
        /// </summary>
        protected virtual void CreateHpBar()
        {
            // 创建hp ui
            m_hpBar = new UP_HudSceneHpBar();
            m_hpBar.SetAlign(UIPrefabAlign.CENTER);
            m_hpBar.SetFollow(actorGameObject.headPoint);
            m_hpBar.SetLookAt(Game.gameCamera.heroCamera.virtualCamera.transform);
            m_hpBar.SetValue(attribute.GetAttribute(ActorAttributeType.Hp), attribute.GetAttribute(ActorAttributeType.HpMax));
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
            m_attackInput = true;
        }

        /// <summary>
        /// 停止攻击
        /// </summary>
        public virtual void StopAttack()
        {
            m_attackInput = false;
            if (actorState != ActorState.Attack)
            {
                actorState = ActorState.None;
                m_weapon.StopAttack();
            }
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
