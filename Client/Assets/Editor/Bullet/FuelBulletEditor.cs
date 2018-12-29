/*
 * @Author: fasthro
 * @Date: 2018-12-27 18:03:05
 * @Description: 燃料子弹编辑器工具
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SmallUniverse.GameEditor
{
    [CustomEditor(typeof(FuelBullet))]
    public class FuelBulletEditor : Editor
    {
        public FuelBullet Target;
        private Color m_backgroundColor;

        void OnEnable()
        {
            Target = target as FuelBullet;
            Target.bulletType = BulletType.Fuel;
        }

        public override void OnInspectorGUI()
        {
            m_backgroundColor = GUI.backgroundColor;
            
            EditorGUILayout.BeginVertical("box");
            BulletEditorUtils.SetAssetBackgroundColor(Target.muzzleAssetPath);
            Target.muzzleAssetPath = EditorGUILayout.TextField("Muzzle Asset Path", Target.muzzleAssetPath);
            GUI.backgroundColor = m_backgroundColor;
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box");
            BulletEditorUtils.SetAssetBackgroundColor(Target.impactAssetPath);
            Target.impactAssetPath = EditorGUILayout.TextField("Impact Asset Path", Target.impactAssetPath);
            GUI.backgroundColor = m_backgroundColor;
            EditorGUILayout.EndVertical();
        }
    }
}

