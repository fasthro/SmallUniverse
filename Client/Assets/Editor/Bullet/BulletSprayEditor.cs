using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SmallUniverse.GameEditor
{
    [CustomEditor(typeof(BulletSpray))]
    public class BulletSprayEditor : Editor
    {
        public BulletSpray Target;
        private Color m_backgroundColor;

        void OnEnable()
        {
            Target = target as BulletSpray;
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

