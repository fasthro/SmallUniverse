using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SmallUniverse.GameEditor
{
    [CustomEditor(typeof(ParticleScale))]
    public class ParticleScaleEditor : Editor
    {
        public ParticleScale Target;

        private float m_scale;
        private float m_prevScale;

        private bool m_scaleGameObject;
        private bool m_prevScaleGameObject;

        void OnEnable()
        {
            Target = target as ParticleScale;
            m_scale = Target.scale;
            m_prevScale = m_scale;
            m_scaleGameObject = Target.scaleGameobject;
            m_prevScaleGameObject = m_scaleGameObject;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginHorizontal("box");
            m_scaleGameObject = GUILayout.Toggle(m_scaleGameObject, " Particle GameObject Scale");
            if(m_scaleGameObject != m_prevScaleGameObject)
            {
                Target.scaleGameobject = m_scaleGameObject;
                m_prevScaleGameObject = m_scaleGameObject;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical("box");
            GUILayout.Label("Particle Scale");
            m_scale = EditorGUILayout.Slider(m_scale, 0.0001f, 20f);
            if(m_prevScale != m_scale)
            {
                Target.scale = m_scale;
                m_prevScale = m_scale;
            }
            EditorGUILayout.EndVertical();
        }

        [MenuItem("SmallUniverse/Effect/Add Particle Scale")]
        public static void AddComponent()
        {
            var activeGameObject = Selection.activeGameObject;
            if(activeGameObject != null)
            {
               ParticleScale ps = activeGameObject.GetComponent<ParticleScale>();
               if(ps == null)
               {
                   activeGameObject.AddComponent<ParticleScale>();
               }
            }
        }
    }
}

