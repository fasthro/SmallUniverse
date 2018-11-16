using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SmallUniverse
{
    public class ActorBase : MonoBehaviour
    {
        public ActorGameObject actorGameObject;
        public ActorAttribute attribute;
        protected Animator animator;
        protected Rigidbody rigidbody;
        protected NavMeshAgent navMeshAgent;
        
        // 目标方向
        protected Vector3 targetDir;
        // 移动方向
        protected Vector3 moveDir;
        // 当前朝向
        protected Vector3 lookDir;

        void Update()
        {
            if(actorGameObject == null)
                return;

            OnUpdate();
        }

        void LateUpdate()
        {
            if(actorGameObject == null)
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
            attribute.SetAttribute(ActorAttributeType.RotationSpeed, 10);
        }

        /// <summary>
        /// 出生
        /// </summary>
        public virtual void Born(LevelPoint point)
        {
            animator = actorGameObject.GetComponent<Animator>();
            rigidbody = actorGameObject.GetComponent<Rigidbody>();
            navMeshAgent = actorGameObject.GetComponent<NavMeshAgent>();

            actorGameObject.transform.position = point.position;
            actorGameObject.transform.localEulerAngles = point.rotationAngle;
            actorGameObject.gameObject.SetActive(true);

            moveDir = actorGameObject.transform.forward;
        }

        /// <summary>
        /// 移动
        /// </summary>
        public virtual void Move(Vector3 move, float delta)
        {
            Vector3 vector = move * attribute.GetAttribute(ActorAttributeType.MoveSpeed);
            if(move.magnitude > 0)
            {
                actorGameObject.transform.position = actorGameObject.transform.position + vector * delta;
            
                moveDir = vector;
                moveDir.y = 0;
                moveDir.Normalize();

                animator.SetBool("run", true);
            }
            else{
                animator.SetBool("run", false);
            }
        }

        /// <summary>
        /// 角色方向
        /// </summary>
        public virtual void UpdateDirection()
        {
            Quaternion tq = Quaternion.LookRotation(moveDir, Vector3.up);
            actorGameObject.transform.rotation = Quaternion.Lerp(actorGameObject.transform.rotation, tq, Time.fixedDeltaTime * attribute.GetAttribute(ActorAttributeType.RotationSpeed));
        }

        protected virtual void OnUpdate()
        {
            UpdateDirection();
        }

        protected virtual void OnLateUpdate()
        {

        }
    }
}
