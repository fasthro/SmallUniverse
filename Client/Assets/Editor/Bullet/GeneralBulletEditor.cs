/*
 * @Author: fasthro
 * @Date: 2018-12-27 18:03:05
 * @Description: 普通子弹编辑器工具
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SmallUniverse.GameEditor
{
	[CustomEditor(typeof(GeneralBullet))]
    public class GeneralBulletEditor : Editor
    {
		public GeneralBullet Target;
        private Color m_backgroundColor;
        
		void OnEnable()
        {
            Target = target as GeneralBullet;

            Target.bulletType = BulletType.General;
        }

        public override void OnInspectorGUI()
        {
            m_backgroundColor = GUI.backgroundColor;

            EditorGUILayout.BeginHorizontal("box");
			Target.speed = EditorGUILayout.FloatField("Flight Speed ", Target.speed);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal("box");
            Target.lifeTime = EditorGUILayout.FloatField("Life Time ", Target.lifeTime);
            EditorGUILayout.EndHorizontal();

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