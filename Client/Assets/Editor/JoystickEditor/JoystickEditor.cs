using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SmallUniverse.GameEditor
{
    [CustomEditor(typeof(Joystick))]
    public class JoystickEditor : Editor
    {
        public Joystick Target;
        private Texture2D circleTexture;

        private Vector2 joyCenter;
        private Vector2 joyUICenter;
        
        private void OnEnable()
        {   
            Target = target as Joystick;

            circleTexture = AssetDatabase.LoadAssetAtPath("Assets/Editor/JoystickEditor/GUI/circle.png", typeof(Texture2D)) as Texture2D;

            JoystickDebug.OnGUIHandlers -= OnGuiHandler;
            JoystickDebug.OnGUIHandlers += OnGuiHandler;

            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
        }

        private void OnDisable()
        {
            JoystickDebug.OnGUIHandlers -= OnGuiHandler;

            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
        }

        private void OnGuiHandler()
        {
            if(circleTexture == null)
                return;
            
            joyCenter.x = Screen.width * Target.percent.x - Target.radius;
            joyCenter.y = Screen.height * Target.percent.y - Target.radius;

            joyUICenter.x = joyCenter.x - Target.radius;
            joyUICenter.y = Screen.height - (joyCenter.y + Target.radius);

            GUI.DrawTexture(new Rect(joyUICenter.x, joyUICenter.y, Target.radius * 2, Target.radius * 2), circleTexture);
        }
    }

}
