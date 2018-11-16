using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

namespace SmallUniverse
{

    public class VirtualMoveJoy : VirtualFuncJoyBase
    {
        public delegate void MoveJoyHandler(Vector2 move);

        public event MoveJoyHandler moveJoyHandler;

        #region ui
        public GComponent ui;
        public Transition startTransition;
        public Transition endTransition;
        public GImage slider;

        #endregion
        
        // ui 半径
        private float m_uiRadius;

        // 开始坐标
        private Vector2 m_starPosition;
        // touch 坐标
        private Vector2 m_touchPosition;
        
        // ui 坐标
        private Vector2 m_uiPoint;
        
        // slider 坐标
        private Vector2 m_sliderPoint;
        // slider 坐在ui里的初始坐标
        private Vector2 m_sliderInitPoint;
        // slider 半径
        private float m_sliderRadius;

        public override void Initialize(Joy joy)
        { 
            base.Initialize(joy);

            m_uiRadius = ui.width / 2;

            m_sliderRadius = slider.width / 2;

            m_sliderInitPoint.x = m_uiRadius - m_sliderRadius;
            m_sliderInitPoint.y = m_uiRadius - m_sliderRadius;
        }

        protected override void OnTouchInit(JoyGesture gesture)
        {
            m_starPosition = gesture.virtualCenter;
            m_touchPosition = gesture.virtualCenter;

            m_uiPoint = CenterToUIPoint(m_starPosition, m_uiRadius);
            m_sliderPoint = m_sliderInitPoint;

            ui.xy = m_uiPoint;
            slider.xy = m_sliderPoint;

            endTransition.Play();
        }
        
        protected override void  OnTouchStart(JoyGesture gesture)
        {
            float referenceValue = gesture.virtualRadius - Vector2.Distance(gesture.virtualCenter, gesture.touchStartPosition);
            if(referenceValue > m_uiRadius)
            {
                m_starPosition = gesture.touchStartPosition;
            }
            else{
                var dir = (gesture.touchStartPosition - gesture.virtualCenter).normalized;
                var len = gesture.virtualRadius - m_uiRadius;
                m_starPosition =  gesture.virtualCenter + dir * len;
            }

            m_touchPosition = gesture.touchStartPosition;

            // ui
            m_uiPoint = CenterToUIPoint(m_starPosition, m_uiRadius);

            // slider
            m_sliderPoint = m_sliderInitPoint + (m_touchPosition - m_starPosition);

            startTransition.Play();
        }

        protected override void  OnTouchMove(JoyGesture gesture)
        {
            if(!gesture.touchMoveing)
                return;

            m_touchPosition = gesture.toucheMovePosition;
            
            float maxDis = m_uiRadius - m_sliderRadius;
            var vector = m_touchPosition - m_starPosition;
            var dis = vector.magnitude;
            if(dis > maxDis)
            {
                dis = maxDis;
            }

            // slider
            m_sliderPoint = m_sliderInitPoint + vector.normalized * dis;
        }

        protected override void  OnTouchUp(JoyGesture gesture)
        {
            m_starPosition = gesture.virtualCenter;
            m_touchPosition = gesture.virtualCenter;
            m_uiPoint = CenterToUIPoint(m_starPosition, m_uiRadius);
            m_sliderPoint = m_sliderInitPoint;

            endTransition.Play();
        }

        protected override void OnUpdate()
        {
            ui.xy = Vector2.Lerp(ui.xy, m_uiPoint, Time.deltaTime * 50);
            slider.xy = Vector2.Lerp(slider.xy, m_sliderPoint, Time.deltaTime * 50);

            if(moveJoyHandler != null)
            {
                var move = (m_starPosition - m_touchPosition).normalized;
                move.x *= -1;

                moveJoyHandler(move);
            }
        }
    }
}

