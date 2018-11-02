using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

namespace SmallUniverse
{
    public class JoystickUI : IJoystick
    {
        private Vector2 initPos;
        private Vector2 startPos;
        private float radius;

        // ui 组件
        public GComponent component;
        public Transition bgStartAni;
        public Transition bgEndAni;
        public GImage bgImage;
        public GImage pointImage;
        public GImage dirImage;
        

        private Vector2 componentInitPos;
        private Vector2 pointInitPos;

        public void OnInitialize(Vector2 pos, float radius)
        {
            this.initPos = pos;
            this.radius = radius;

            component.width = radius * 2;
            component.height = radius * 2;

            this.componentInitPos = PositionToUI(pos);
            component.x = componentInitPos.x;
            component.y = componentInitPos.y;

            float cp = component.width / 2 - pointImage.width / 2;
            pointInitPos = new Vector2(cp, cp);

            pointImage.visible = false;
            dirImage.visible = false;

            bgEndAni.Play();
        }

        public void OnStart(Vector2 pos)
        {
            this.startPos = pos;

            this.componentInitPos = PositionToUI(pos);
            component.x = componentInitPos.x;
            component.y = componentInitPos.y;

            pointImage.visible = true;

            bgStartAni.Play();
        }

        public void OnEnd()
        {
            this.componentInitPos = PositionToUI(this.initPos);
            component.x = componentInitPos.x;
            component.y = componentInitPos.y;

            pointImage.visible = false;
            dirImage.visible = false;

            bgEndAni.Play();
        }

        public void OnMove(bool isMove, Vector2 pos, float angle)
        {
            dirImage.visible = isMove;
            dirImage.rotation = angle;

            Vector2 dir = PositionToUI(startPos) - PositionToUI(pos);
            float disance = dir.magnitude;
            float maxDisance = radius - pointImage.width / 2;

            if(disance > maxDisance)
            {
                disance = maxDisance;          
            }

            Vector2 np = pointInitPos - dir.normalized * disance;
            pointImage.x = np.x;
            pointImage.y = np.y;
        }

        private Vector2 PositionToUI(Vector2 pos)
        {
            pos.x = pos.x - radius;
            pos.y = Screen.height - (pos.y + radius);
            return pos;
        }
    }
}

