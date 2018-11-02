//Generate By @ExportPanelCode
using FairyGUI;
using SmallUniverse.UI;

namespace SmallUniverse.UI
{
    public class LoaderPanelView : PanelViewBase
    {
      public GProgressBar loader_progressbar;

        public override void Get()
        {
            base.Get();

           loader_progressbar = root.GetChild("@loader_progressbar").asProgress;
        }

        public override void Init()
        {
            base.Init();


        }
        
        public override void Dispose() 
        {
            base.Dispose();

           loader_progressbar = null;
        }
    }
}