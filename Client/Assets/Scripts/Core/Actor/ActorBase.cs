using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class ActorBase : MonoBehaviour
    {
        public ActorGameObject actorGameObject;
        protected ActorAttribute attribute;
        protected Animator animator;
        
        // 目标方向
        protected Vector3 targetDir;
        // 移动方向
        protected Vector3 moveDir;
        // 当前朝向
        protected Vector3 lookDir;

        /// <summary>
        /// 初始化数据
        /// </summary>
        public void InitActorData()
        {
            attribute = new ActorAttribute();
            attribute.SetAttribute(ActorAttributeType.MoveSpeed, 3);
        }

        /// <summary>
        /// 出生
        /// </summary>
        public virtual void Born(LevelPoint point)
        {
            actorGameObject.transform.position = point.position;
            actorGameObject.transform.localEulerAngles = point.rotationAngle;
            actorGameObject.gameObject.SetActive(true);
        }

        /// <summary>
        /// 移动
        /// </summary>
        public virtual void Move(Vector3 move, float delta)
        {
            Vector3 vector = move * attribute.GetAttribute(ActorAttributeType.MoveSpeed);
            //actorGameObject.transform.position = actorGameObject.transform.position + vector * delta;
            gameObject.GetComponent<Rigidbody>().velocity = vector;
            moveDir = vector;
            moveDir.y = 0;
            moveDir.Normalize();
        }
    }
}
