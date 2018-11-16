using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmallUniverse.Behaviour;
using Cinemachine;

namespace SmallUniverse
{
    public class VirtualCamera : MonoBehaviour {
        public CinemachineVirtualCamera virtualCamera;
        public float distance = 15f;
        public float degree = 45f;
        public float yDegree;
        public float moveSpeed = 4f;
        private Transform lookTrans;

        public void Initialize()
        {

        }

        public void SetLookAt(Transform _lookTrans)
        {
            lookTrans = _lookTrans;
        }

        void LateUpdate()
        {
            if(lookTrans == null)
                return;

            transform.position = Vector3.MoveTowards(transform.position, lookTrans.position, Time.deltaTime * GetMoveSpeed());
            transform.eulerAngles = new Vector3(0f, -this.yDegree, 0f);

            virtualCamera.transform.localPosition = GetCameraPostion();
            virtualCamera.transform.LookAt(transform);
        }

        private float GetMoveSpeed()
        {
            if(Game.hero != null)
            {
                return Game.hero.attribute.GetAttribute(ActorAttributeType.MoveSpeed);
            }
            return moveSpeed;
        }

        public Vector3 GetCameraPostion()
        {
            float y = this.distance * Mathf.Sin(this.degree * 0.0174532924f);
            float num = this.distance * Mathf.Cos(this.degree * 0.0174532924f);
            float x = num * Mathf.Sin(this.yDegree * 0.0174532924f);
            float z = -num * Mathf.Cos(this.yDegree * 0.0174532924f);
            Vector3 result = new Vector3(x, y, z);
            return result;
        }
    }
}

