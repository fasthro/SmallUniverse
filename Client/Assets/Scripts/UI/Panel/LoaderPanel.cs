// Generate By @ExportViewCode
using SmallUniverse.UI;
using SmallUniverse.Manager;

namespace SmallUniverse.UI
{
    public class LoaderPanel : PanelBase
    {
        // 视图 PanelView
        private LoaderPanelView view;

        public LoaderPanel(params string[] _parameters) : base()
        {
            pname = PanelName.LoaderPanel;
            view = PanelMap.GetView(pname) as LoaderPanelView;
            mainPackage = "loader_panel";
            packages = new string[] {"common"};
            pcname = "loader_panel";
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

        public void SetLoaderProgress(float progress)
        {
            if (!view.Initialized)
                return;

            view.loader_progressbar.value = progress;
        }
    }
}
