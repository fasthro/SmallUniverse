//Generate By @ExportPanelCode
using FairyGUI;
using SmallUniverse.UI;

namespace SmallUniverse.UI
{
    public class LoginPanelView : PanelViewBase
    {
        public GButton begin_game_btn;

        public override void Get()
        {
            base.Get();

            begin_game_btn = root.GetChild("@begin_game_btn").asButton;
        }

        public override void Init()
        {
            base.Init();


        }

        public override void Dispose()
        {
            base.Dispose();

            begin_game_btn = null;
        }
    }
}
