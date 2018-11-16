using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
	 [ExecuteInEditMode]
	public class VirtualJoy : MonoBehaviour 
	{
		public Joy moveJoy;
		public Joy attackJoy;
		public Joy[] joys;

		public bool enableVirtuakJoy = true;

		void Awake()
		{
			moveJoy.Initialize();
		}

#if UNITY_EDITOR
        void OnGUI()
        {
            if (VirtualJoyDebug.OnGUIHandlers != null)
                VirtualJoyDebug.OnGUIHandlers();
        }
#endif
		
		void OnEnable()
        {
            EasyTouch.On_TouchStart += On_TouchStart;
            EasyTouch.On_TouchUp += On_TouchUp;
            EasyTouch.On_TouchDown += On_TouchMove;
        }

        void OnDisable()
        {
            EasyTouch.On_TouchStart -= On_TouchStart;
            EasyTouch.On_TouchUp -= On_TouchUp;
            EasyTouch.On_TouchDown -= On_TouchMove;
        }

		void Update()
		{
			if(!enableVirtuakJoy)
				return;

			moveJoy.On_Update();
			attackJoy.On_Update();

			for(int i = 0; i < joys.Length; i++)
			{
				joys[i].On_Update();
			}
		}

		void On_TouchStart(Gesture gesture)
		{
			if(!enableVirtuakJoy)
				return;

			moveJoy.On_TouchStart(gesture);
			attackJoy.On_TouchStart(gesture);

			for(int i = 0; i < joys.Length; i++)
			{
				joys[i].On_TouchStart(gesture);
			}
		}

		void On_TouchUp(Gesture gesture)
		{
			if(!enableVirtuakJoy)
				return;
				
			moveJoy.On_TouchUp(gesture);
			attackJoy.On_TouchUp(gesture);

			for(int i = 0; i < joys.Length; i++)
			{
				joys[i].On_TouchUp(gesture);
			}
		}

		void On_TouchMove(Gesture gesture)
		{
			if(!enableVirtuakJoy)
				return;
				
			moveJoy.On_TouchMove(gesture);
			attackJoy.On_TouchMove(gesture);

			for(int i = 0; i < joys.Length; i++)
			{
				joys[i].On_TouchMove(gesture);
			}
		}
	}
}

