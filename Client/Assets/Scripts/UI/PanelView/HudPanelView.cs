//Generate By @ExportPanelCode
using FairyGUI;
using SmallUniverse.UI;

namespace SmallUniverse.UI
{
    public class HudPanelView : PanelViewBase
    {
      public GComponent move_joy;
      public GImage bg_move_joy;
      public GImage touch_move_joy;
      public GComponent attack_joy;
      public GComponent skill_3_joy;
      public GComponent skill_2_joy;
      public GComponent skill_1_joy;

        public override void Get()
        {
            base.Get();

           move_joy = root.GetChild("@move_joy").asCom;
           bg_move_joy = root.GetChild("@move_joy").asCom.GetChild("@bg").asImage;
           touch_move_joy = root.GetChild("@move_joy").asCom.GetChild("@touch").asImage;
           attack_joy = root.GetChild("@attack_joy").asCom;
           skill_3_joy = root.GetChild("@skill_3_joy").asCom;
           skill_2_joy = root.GetChild("@skill_2_joy").asCom;
           skill_1_joy = root.GetChild("@skill_1_joy").asCom;
        }

        public override void Init()
        {
            base.Init();


        }
        
        public override void Dispose() 
        {
            base.Dispose();

           move_joy = null;
           bg_move_joy = null;
           touch_move_joy = null;
           attack_joy = null;
           skill_3_joy = null;
           skill_2_joy = null;
           skill_1_joy = null;
        }
    }
}