using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmallUniverse.Behaviour;
using FairyGUI;
using System;

namespace SmallUniverse
{
    public class Joystick : MonoBehaviour
    {
        public float radius = 100f;
        public Vector2 percent = new Vector2(0.25f, 0.4f);
		public bool enableJoystick = false;
       
        // 虚拟中心点
        private Vector2 center;
        // 开始的位置
        private Vector2 startPos;
		// 移动位置
		private Vector2 movePos;
		// 是否正在拖拽
		private bool isDragging;
		// 手势 Index
        private int fingerIndex = -1;

		private IJoystick ij;

		public void Initialize(IJoystick _ij)
		{
			ij = _ij;

            enableJoystick = true;

            center.x = Screen.width * percent.x - radius;
            center.y = Screen.height * percent.y - radius;

            ij.OnInitialize(center, radius);
		}

		public void CloseJoystick()
		{
			enableJoystick = false;
            ij = null;
            fingerIndex = -1;
		}

        void OnEnable()
        {
            EasyTouch.On_TouchStart += On_TouchStart;
            EasyTouch.On_TouchUp += On_TouchUp;
            EasyTouch.On_TouchDown += On_TouchMove;
        }

        void OnDisable()
        {
            EasyTouch.On_TouchStart -= On_TouchStart;
            EasyTouch.On_TouchUp -= On_TouchUp;
            EasyTouch.On_TouchDown -= On_TouchMove;
        }

        // 检查是否在虚拟范围内
        private bool CheckInJoystick(Vector2 position)
        {
            float distance = Vector2.Distance(center, position);
            if (distance <= radius)
                return true;
            return false;
        }

        void On_TouchStart(Gesture gesture)
        {
			if(!enableJoystick)
				return;

			isDragging = CheckInJoystick(gesture.position);

			if(!isDragging)
                return;
			
            fingerIndex = gesture.fingerIndex;
            startPos = gesture.position;

            if(ij != null)
                ij.OnStart(startPos);

        }

        void On_TouchUp(Gesture gesture)
        {
			if(!enableJoystick)
				return;
            
            if(!isDragging || gesture.fingerIndex != fingerIndex)
                return;

			isDragging = false;
			fingerIndex = -1;

            if(ij != null)
                ij.OnEnd();
        }

        void On_TouchMove(Gesture gesture)
        {
			if(!enableJoystick)
				return;
			
			if(!isDragging || gesture.fingerIndex != fingerIndex)
				return;

			movePos = gesture.position;
            
            bool isMove = movePos.x != startPos.x || movePos.y != startPos.y;

            float angle = 180f + Vector2.SignedAngle((startPos - movePos).normalized, Vector2.up);

            if(ij != null)
                ij.OnMove(isMove, movePos, angle);
        }
    }
}

