using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmallUniverse.Manager;

namespace SmallUniverse
{
    public class JoystickInput
    {
        public static void SetMoveValue(Vector2 joy)
        {
            Vector3 move = new Vector3(-joy.x, 0, -joy.y);
            var hero = Game.hero;
            if(hero != null)
            {
                hero.Move(move, Time.deltaTime);
            }
        }
    }
}
