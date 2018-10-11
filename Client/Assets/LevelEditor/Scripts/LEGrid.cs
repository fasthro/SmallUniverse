using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SU.Editor.LevelEditor
{
    /// <summary>
    /// 功能类型
    /// </summary>
    public enum GridFunctions
    {
        Ground,              // 地面
        Player,                // 玩家
        Monster,            // 怪
        Door,                 // 门
        Trap,                 // 陷阱
        Transfer,           // 传送门
    }

    public class LEGrid : MonoBehaviour
    {
        // 对应的key
        public string key;
        // 资源库名称
        public string repositoryName;
        // 资源路径
        public string assetPath;
        // 资源名称
        public string assetName;
        // 所在bundle 名称
        public string bundleName;
        // 所在地块
        public int groud;
        // 位置
        public Vector3 position;
        // 旋转
        public Vector3 rotationAngle;
        // 格子功能
        public GridFunctions function;

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
