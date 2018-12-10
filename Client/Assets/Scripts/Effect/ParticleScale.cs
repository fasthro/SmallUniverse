using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SmallUniverse
{
	[ExecuteInEditMode]
    public class ParticleScale : MonoBehaviour
    {
#if UNITY_EDITOR

        // 缩放大小
        public float scale = 1.0f;

        // 是否缩物体 
        public bool scaleGameobject = true;

        private float m_prevScale;

        void Start()
        {
            m_prevScale = scale;
        }

        void ScaleShurikenSystems(float scaleFactor)
        {
            ParticleSystem[] systems = GetComponentsInChildren<ParticleSystem>();

            foreach (ParticleSystem system in systems)
            {
                system.startSpeed *= scaleFactor;
                system.startSize *= scaleFactor;
                system.gravityModifier *= scaleFactor;

                SerializedObject so = new SerializedObject(system);

                so.FindProperty("VelocityModule.x.scalar").floatValue *= scaleFactor;
                so.FindProperty("VelocityModule.y.scalar").floatValue *= scaleFactor;
                so.FindProperty("VelocityModule.z.scalar").floatValue *= scaleFactor;
                so.FindProperty("ClampVelocityModule.magnitude.scalar").floatValue *= scaleFactor;
                so.FindProperty("ClampVelocityModule.x.scalar").floatValue *= scaleFactor;
                so.FindProperty("ClampVelocityModule.y.scalar").floatValue *= scaleFactor;
                so.FindProperty("ClampVelocityModule.z.scalar").floatValue *= scaleFactor;
                so.FindProperty("ForceModule.x.scalar").floatValue *= scaleFactor;
                so.FindProperty("ForceModule.y.scalar").floatValue *= scaleFactor;
                so.FindProperty("ForceModule.z.scalar").floatValue *= scaleFactor;
                so.FindProperty("ColorBySpeedModule.range").vector2Value *= scaleFactor;
                so.FindProperty("SizeBySpeedModule.range").vector2Value *= scaleFactor;
                so.FindProperty("RotationBySpeedModule.range").vector2Value *= scaleFactor;

                so.ApplyModifiedProperties();
            }
        }

        void ScaleTrailRenderers(float scaleFactor)
        {
            TrailRenderer[] trails = GetComponentsInChildren<TrailRenderer>();

            foreach (TrailRenderer trail in trails)
            {
                trail.startWidth *= scaleFactor;
                trail.endWidth *= scaleFactor;
            }
        }

        void Update()
        {

            if (m_prevScale != scale && scale > 0)
            {
                if (scaleGameobject)
                    transform.localScale = new Vector3(scale, scale, scale);

                float scaleFactor = scale / m_prevScale;

                ScaleShurikenSystems(scaleFactor);
                ScaleTrailRenderers(scaleFactor);

                m_prevScale = scale;
            }
        }
#endif
    }
}

