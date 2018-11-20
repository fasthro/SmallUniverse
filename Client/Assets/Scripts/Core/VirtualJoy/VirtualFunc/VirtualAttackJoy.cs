using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class VirtualAttackJoy : VirtualFuncJoyBase
    {
        protected override void OnKeyUp()
        {
            Game.hero.StopAttack();
        }

        protected override void OnKeyDown()
        {
            Game.hero.Attack(new SkillData());
        }
    }
}
