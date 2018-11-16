using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class JoyGesture
    {
        public int fingerIndex;
        public JoyTouchEventName eventName;
        public float virtualRadius;
        public Vector2 virtualCenter;
        public Vector2 touchStartPosition;
        public Vector2 toucheMovePosition;
        public Vector2 touchEndPosition;

        public bool touchMoveing;
        public Vector2 touchMoveDirection;
        public float touchMoveAngle;
    }
}
