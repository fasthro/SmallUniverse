using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SU.Editor.LevelEditor
{
    public enum GridFunction
    {
       None,
       CharacterPoint, // 角色出生点
       MonsterPoint, // 怪物出生点
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
        // 所在层
        public int layer;
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
