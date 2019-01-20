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
    public class ActorAnimatorParameters
    {
        // 出生
        public static string Born = "Born";
        // 死亡
        public static string Death = "Death";
        // 移动
        public static string Move = "Move";
        // 1 - 普通攻击
        // 2 - 技能攻击
        // 3 - 技能攻击
        // 4 - 技能攻击
        // 5 - 技能攻击
        public static string Attack = "Attack";
        // 被击中
        public static string Hit = "Hit";
        // 眩晕
        public static string Dizzy = "Dizzy";
        // 冰冻
        public static string Freeze = "Freeze";
        // 移动速度
        public static string MoveSpeed = "MoveSpeed";
    }

    public abstract class ActorBase : MonoBehaviour
    {
        public ActorGameObject actorGameObject;
        public ActorAttribute attribute;
        protected Animator m_animator;
        protected Rigidbody m_rigidbody;
        protected NavMeshAgent m_navMeshAgent;
        protected UP_HudSceneHpBar m_HPBar;

        // 移动量
        protected Vector3 m_moveVt;
        // 方向量
        protected Vector3 m_dirVt;
        protected Quaternion m_targetRotation;

        // 是否能移动
        public bool canMove;
        // 是否能攻击
        public bool canAttack;
        // 正在攻击
        public bool attack;

        /// <summary>
        /// 初始化数据
        /// </summary>
        public void InitActorData()
        {
            attribute = ActorAttribute.Create();
        }

        /// <summary>
        /// Actor 出生
        /// </summary>
        /// <param name="point"></param>
        public virtual void Born(LevelPoint point)
        {
        
        }

        /// <summary>
        /// Actor 出生
        /// </summary>
        /// <param name="point"></param>
        /// <param name="actorArt"></param>
        protected virtual void Born(LevelPoint point, string actorArt)
        {
            // 创建角色
            GameObject prefab = LevelAsset.GetGameObject(actorArt);
            var actorGo = GameObject.Instantiate<GameObject>(prefab);
            actorGo.transform.parent = transform;
            actorGameObject = actorGo.GetComponent<ActorGameObject>();

            // 组件获取
            m_animator = actorGameObject.GetComponent<Animator>();
            m_rigidbody = actorGameObject.GetComponent<Rigidbody>();
            m_navMeshAgent = actorGameObject.GetComponent<NavMeshAgent>();

            // 基础设置
            actorGameObject.actor = this;
            actorGameObject.transform.position = point.position;
            actorGameObject.transform.localEulerAngles = point.rotationAngle;

            // 寻路设置
            m_navMeshAgent.speed = attribute.GetAttribute(ActorAttributeType.MoveSpeed);
            
            m_moveVt = Vector3.zero;
            m_dirVt = actorGameObject.transform.forward;
        }

        /// <summary>
        /// Actor 死亡
        /// </summary>
        public virtual void Death()
        {
            
        }

        /// <summary>
        /// 移动
        /// </summary>
        public virtual void Move(Vector3 move, bool stop)
        {

        }

        /// <summary>
        /// 设置是否能移动
        /// </summary>
        public virtual void SetCanMove(bool can)
        {

        }

        /// <summary>
        /// 移动到指定位置
        /// </summary>
        /// <param name="target">目标位置</param>
        /// <param name="stoppingDistance">当距离目标位置这个距离的时候停止</param>
        public virtual void MoveTo(Vector3 target, float stoppingDistance)
        {
            
        }

        /// <summary>
        /// 看向目标方向
        /// </summary>
        /// <param name="direction">方向</param>
        public virtual void LookRotation(Vector3 direction)
        {
            actorGameObject.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }

        /// <summary>
        /// Actor 攻击
        /// </summary>
        /// <param name="attackIndex">攻击索引</param>
        public virtual void Attack(int attackIndex)
        {

        }

        /// <summary>
        /// 移动
        /// </summary>
        public virtual void SetCanAttack(bool can)
        {

        }

        /// <summary>
        /// 被攻击受伤
        /// </summary>
        public virtual void Hit()
        {
            
        }

        /// <summary>
        /// 创建生命条
        /// </summary>
        public virtual void CreateHpBar()
        {
            m_HPBar = new UP_HudSceneHpBar();
            m_HPBar.SetAlign(UIPrefabAlign.CENTER);
            //m_HPBar.SetFollow(actorGameObject.headPoint);
            m_HPBar.SetLookAt(Game.gameCamera.heroCamera.virtualCamera.transform);
            m_HPBar.SetValue(attribute.GetAttribute(ActorAttributeType.Hp), attribute.GetAttribute(ActorAttributeType.HpMax));
        }

        public virtual void OnUpdate()
        {

        }

        public virtual void OnLateUpdate()
        {

        }

        public virtual void OnFixedUpdate()
        {

        }
    }
}
