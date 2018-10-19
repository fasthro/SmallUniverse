using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SU.Editor.LevelEditor
{
    /// <summary>
    /// 功能类型
    /// </summary>
    public enum GridFunction
    {
        Ground,              // 地面
        Player,                // 玩家
        Monster,            // 怪
        Door,                 // 门
        Trap,                  // 陷阱
        Transfer,            // 传送门
        Decoration,       // 装饰品
    }

    public class LEGrid : MonoBehaviour
    {
        // key
        public string key;
        // prefabGo
        public LEPrefabGo prefabGo;
        // 所在地块
        public int area;
        // 位置
        public Vector3 position;
        // 旋转
        public Vector3 rotationAngle;
        // 格子功能
        public GridFunction function;

        #region rotate
        public void RotateX()
        {
            rotationAngle.x += 90f;
            transform.localEulerAngles = rotationAngle;
        }

        public void RotateY()
        {
            rotationAngle.y += 90f;
            transform.localEulerAngles = rotationAngle;
        }

        public void RotateZ()
        {
            rotationAngle.z += 90f;
            transform.localEulerAngles = rotationAngle;
        }
        #endregion
    }
}
