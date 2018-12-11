using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SmallUniverse.GameEditor
{
    [CustomEditor(typeof(BulletLine))]
    public class BulletLineEditor : Editor
    {
        public BulletLine Target;
        private Color m_backgroundColor;

        void OnEnable()
        {
            Target = target as BulletLine;
        }

        public override void OnInspectorGUI()
        {
            m_backgroundColor = GUI.backgroundColor;

            EditorGUILayout.BeginHorizontal("box");
            Target.length = EditorGUILayout.FloatField("Max Length ", Target.length);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical("box");
            Target.uvAnimate = EditorGUILayout.Toggle("UV Animate", Target.uvAnimate);
            if(Target.uvAnimate)
            {
                Target.uvAnimateSpeed = EditorGUILayout.Vector2Field("UV Animate Speed ", Target.uvAnimateSpeed);
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box");
            BulletEditorUtils.SetAssetBackgroundColor(Target.beginPointAssetPath);
            Target.beginPointAssetPath = EditorGUILayout.TextField("Begin Point Asset Path", Target.beginPointAssetPath);
            GUI.backgroundColor = m_backgroundColor;
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box");
            BulletEditorUtils.SetAssetBackgroundColor(Target.endPointAssetPath);
            Target.endPointAssetPath = EditorGUILayout.TextField("End Point Asset Path", Target.endPointAssetPath);
            GUI.backgroundColor = m_backgroundColor;
            EditorGUILayout.EndVertical();
        }
    }
}
