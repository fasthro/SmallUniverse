using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class VirtualAttackJoy : VirtualFuncJoyBase
    {
        protected override void OnTouchUp(JoyGesture gesture)
        {
            Game.hero.Attack();
        }

        protected override void OnKeyDown()
        {
            Game.hero.Attack();
        }
    }
}
