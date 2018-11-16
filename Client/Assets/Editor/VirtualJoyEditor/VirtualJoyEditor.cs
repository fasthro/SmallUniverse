using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SmallUniverse.GameEditor
{
    [CustomEditor(typeof(VirtualJoy))]
    public class VirtualJoyEditor : Editor
    {
        public VirtualJoy Target;
        private Texture2D circleTexture;
        private Texture2D squarTexture;
        
        private void OnEnable()
        {   
            Target = target as VirtualJoy;

            circleTexture = AssetDatabase.LoadAssetAtPath("Assets/Editor/VirtualJoyEditor/GUI/circle.png", typeof(Texture2D)) as Texture2D;
            squarTexture = AssetDatabase.LoadAssetAtPath("Assets/Editor/VirtualJoyEditor/GUI/square.png", typeof(Texture2D)) as Texture2D;

            VirtualJoyDebug.OnGUIHandlers -= OnGuiHandler;
            VirtualJoyDebug.OnGUIHandlers += OnGuiHandler;

            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
        }

        private void OnDisable()
        {
            VirtualJoyDebug.OnGUIHandlers -= OnGuiHandler;

            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
        }

        private void OnGuiHandler()
        {
            // move joy
            GUI.DrawTexture(GetUIRect(Target.moveJoy), GetJoyTexture(Target.moveJoy));

            // att joy
            GUI.DrawTexture(GetUIRect(Target.attackJoy), GetJoyTexture(Target.attackJoy));

            var count = Target.joys.Length;
            for(int i = 0; i < count; i++)
            {
                GUI.DrawTexture(GetUIRect(Target.joys[i]), GetJoyTexture(Target.joys[i]));
            }
        }

        Texture2D GetJoyTexture(Joy joy)
        {
            if(joy.virtualShape == JoyVirtualShape.Circle)
            {
                return circleTexture;
            }
            else{
                return squarTexture;
            }
        }

        Rect GetUIRect(Joy joy)
        {
            var uiPoint = GetUIPoint(joy);
            return new Rect(uiPoint.x, uiPoint.y, joy.parame.radius * 2, joy.parame.radius * 2);
        }

        Vector2 GetUIPoint(Joy joy)
        {
            var virtualCenter = GetVirtualCenter(joy);
            Vector2 uiPoint = Vector2.zero;

            uiPoint.x = virtualCenter.x - joy.parame.radius;
            uiPoint.y = virtualCenter.y - joy.parame.radius;

            return uiPoint;
        }

         Vector2 GetVirtualCenter(Joy joy)
        {
            JoyParame parame = joy.parame;
            Vector2 center = Vector2.zero;
            if(joy.screenDirection == JoyScreenDirection.Left)
			{
				center.x = parame.boundary.x + parame.radius;
				center.y = Screen.height - parame.boundary.y - parame.radius;
			}
			else if(joy.screenDirection == JoyScreenDirection.Right){
				center.x = Screen.width - parame.boundary.x - parame.radius;
				center.y = Screen.height - parame.boundary.y - parame.radius;
			}
            return center;
        }
    }

}
