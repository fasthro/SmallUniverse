//Generate By @ExportPanelCode
using FairyGUI;
using SmallUniverse.UI;

namespace SmallUniverse.UI
{
    public class MainPanelView : PanelViewBase
    {
      public GComponent joystick;
      public GImage bg_joystick;
      public GImage touch_joystick;

        public override void Get()
        {
            base.Get();

           joystick = root.GetChild("@joystick").asCom;
           bg_joystick = root.GetChild("@joystick").asCom.GetChild("@bg").asImage;
           touch_joystick = root.GetChild("@joystick").asCom.GetChild("@touch").asImage;
        }

        public override void Init()
        {
            base.Init();


        }
        
        public override void Dispose() 
        {
            base.Dispose();

           joystick = null;
           bg_joystick = null;
           touch_joystick = null;
        }
    }
}