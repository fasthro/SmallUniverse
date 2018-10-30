// Generate By @ExportViewCode
using SmallUniverse.UI;
using SmallUniverse.Manager;

namespace SmallUniverse.UI
{
    public class MainPanel : PanelBase
    {
        // 视图 PanelView
        private MainPanelView view;

        public MainPanel(params string[] _parameters) : base()
        {
            pname = PanelName.MainPanel;
            view = PanelMap.GetView(pname) as MainPanelView;
            mainPackage = "main_panel";
            packages = new string[] { "common" };
            pcname = "main_panel";
            parameters = _parameters;
            layer = PanelLayer.WINDOW;
        }

        protected override void OnShown()
        {
            base.OnShown();

            view.SetRoot(contentPane);
            view.Get();
            view.Init();
        }

        protected override void OnHide()
        {
            base.OnHide();

            view.Dispose();
        }
    }
}
