using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public interface IJoystick
    {
			void OnInitialize(Vector2 pos, float radius);
			void OnStart(Vector2 pos);
			void OnMove(bool isMove, Vector2 pos, float angle);
			void OnEnd();
    }
}
