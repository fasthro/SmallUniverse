/*
 * @Author: fasthro
 * @Date: 2018-12-27 18:03:13
 * @Description: 角色基类
 */
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
        Move,
        MoveTo,
        Attack,
        MoveAttack,
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
        
        // 目标
        protected Transform m_target;
        public Transform target
        {
            get
            {
                return m_target;
            }
        }

        // transform
        public Transform Transform
        {
            get
            {
                return actorGameObject.transform;
            }
        }

        // position
        public Vector3 Position
        {
            get
            {
                return actorGameObject.transform.position;
            }
        }

        // 出生标识
        protected bool m_isBorn;
        public bool IsBorn
        {
            get
            {
                return m_isBorn;
            }
        }

        // 死亡标识
        protected bool m_isDeath;
        public bool IsDeath
        {
            get
            {
                return m_isDeath;
            }
        }

        // 移动标识
        protected bool m_isMove;
        public bool IsMove
        {
            get
            {
                return m_isMove;
            }
        }

        // 移动到目标标识
        protected bool m_isMoveTo;
        public bool IsMoveTo
        {
            get
            {
                return m_isMoveTo;
            }
        }

        // 攻击标识
        protected bool m_isAttack;
        public bool IsAttack
        {
            get
            {
                return m_isAttack;
            }
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        public void InitActorData()
        {
            m_isBorn = false;
            m_isDeath = false;
            m_isMove = false;
            m_isMoveTo = false;
            m_isAttack = false;

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
            m_isBorn = true;
        }

        /// <summary>
        /// 出生
        /// </summary>
        /// <param name="haveHPBar"></param>
        /// <param name="haveWeapon"></param>
        protected void Born(LevelPoint point, bool haveHPBar, bool haveWeapon)
        {
            m_isBorn = true;

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
                GameObject prefab = LevelAsset.GetGameObject("Weapons/Rifle/Rifle_General");
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
            if (m_isBorn && !m_isDeath)
            {
                m_isDeath = true;
                m_animator.SetBool(ActorAnimatorParameters.Death.ToString(), true);
            }
        }

        /// <summary>
        /// 移动
        /// </summary>
        public virtual void Move(Vector3 move, float delta)
        {
            m_isMove = true;
            m_isMoveTo = false;
        }

        /// <summary>
        /// 移动到指定位置
        /// </summary>
        /// <param name="target">目标位置</param>
        /// <param name="stoppingDistance">当距离目标位置这个距离的时候停止</param>
        public virtual void MoveTo(Vector3 target, float stoppingDistance)
        {
            m_isMove = true;
            m_isMoveTo = true;
            m_navMeshAgent.updatePosition = false;
            m_navMeshAgent.updateRotation = false;
            m_navMeshAgent.isStopped = false;
            m_navMeshAgent.stoppingDistance = stoppingDistance;
            m_navMeshAgent.SetDestination(target);
        }

        /// <summary>
        /// 停止移动到指定位置
        /// </summary>
        public void StopMoveTo()
        {
            m_isMove = false;
            m_isMoveTo = false;
            m_navMeshAgent.isStopped = true;
            m_navMeshAgent.stoppingDistance = 0;
            m_navMeshAgent.updateRotation = false;
            m_navMeshAgent.updatePosition = false;
        }

        /// <summary>
        /// 旋转到指定方向
        /// </summary>
        /// <param name="direction">方向</param>
        public virtual void RotationTo(Vector3 direction)
        {
            actorGameObject.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }

        /// <summary>
        /// 转向目标
        /// </summary>
        /// <param name="target">目标 transform</param>
        public virtual void RotationTo(Transform target)
        {
            Vector3 direction = target.position - actorGameObject.gameObject.transform.position;
            direction.Normalize();
            direction.y = 0;
            RotationTo(direction);
        }

        /// <summary>
        /// 攻击
        /// </summary>
        public virtual void Attack(Transform target)
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

            if (m_HPBar != null)
            {
                m_HPBar.Dispose();
                m_HPBar = null;
            }
        }

        protected virtual void OnUpdate()
        {
            // move to update
            if (m_isMove && m_isMoveTo)
            {
                if (m_navMeshAgent.remainingDistance <= m_navMeshAgent.stoppingDistance)
                {
                    StopMoveTo();
                }
                else
                {
                    Vector3 direction = m_navMeshAgent.nextPosition - actorGameObject.gameObject.transform.position;
                    direction.Normalize();
                    direction.y = 0;
                    RotationTo(direction);

                    actorGameObject.gameObject.transform.position = m_navMeshAgent.nextPosition;
                }
            }
        }

        protected virtual void OnLateUpdate()
        {

        }

        void Update()
        {
            if (m_isBorn)
            {
                OnUpdate();
            }
        }

        void LateUpdate()
        {
            if (m_isBorn)
            {
                OnLateUpdate();
            }
        }
    }
}
