using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;
using SmallUniverse.Manager;

namespace SmallUniverse
{
    public class VirtualAttackJoy : VirtualFuncJoyBase
    {
        #region ui
        public GComponent ui;

        #endregion

        // ui 半径
        private float m_uiRadius;

        public override void Initialize(Joy joy)
        {
            base.Initialize(joy);

            m_uiRadius = ui.width / 2;
        }

        protected override void OnTouchInit(JoyGesture gesture)
        {
            ui.xy = CenterToUIPoint(gesture.virtualCenter, m_uiRadius);
        }

        protected override void OnTouchStart(JoyGesture gesture)
        {
            if(m_joy.joyFunc == JoyFunc.AttackJoy)
            {
                Game.hero.Attack(1);
            }
        }

        protected override void OnTouchUp(JoyGesture gesture)
        {
            
        }

        protected override void OnKeyUp()
        {
            
        }

        protected override void OnKeyDown()
        {
            if(m_joy.joyFunc == JoyFunc.AttackJoy)
            {
                Game.hero.Attack(1);
            }
        }
    }
}
