using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

namespace SmallUniverse
{
    public class LevelPoint
    {
        // 位置
        public Vector3 position;
        // 角度
        public Vector3 rotationAngle;

        public LevelPoint(SecurityElement xml)
        {
            position = new Vector3(int.Parse(xml.Attribute("pos_x")), int.Parse(xml.Attribute("pos_y")), int.Parse(xml.Attribute("pos_z")));
            rotationAngle = new Vector3(int.Parse(xml.Attribute("angle_x")), int.Parse(xml.Attribute("angle_y")), int.Parse(xml.Attribute("angle_z")));
        }
    }
}

