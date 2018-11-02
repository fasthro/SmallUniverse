//Generate By @ExportPanelCode
using FairyGUI;
using SmallUniverse.UI;

namespace SmallUniverse.UI
{
    public class MainPanelView : PanelViewBase
    {
      public GComponent joystick;
      public GImage bg_joystick;
      public GImage point_joystick;
      public GImage dir_joystick;

        public override void Get()
        {
            base.Get();

           joystick = root.GetChild("@joystick").asCom;
           bg_joystick = root.GetChild("@joystick").asCom.GetChild("@bg").asImage;
           point_joystick = root.GetChild("@joystick").asCom.GetChild("@point").asImage;
           dir_joystick = root.GetChild("@joystick").asCom.GetChild("@dir").asImage;
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
           point_joystick = null;
           dir_joystick = null;
        }
    }
}