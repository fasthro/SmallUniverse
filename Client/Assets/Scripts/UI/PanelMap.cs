//Generate By @ExportPanelCode
namespace SmallUniverse.UI
{
    public class PanelMap
    {
        public static PanelBase GetPanel(PanelName pname)
        {
            switch (pname)
            {
                case PanelName.LoaderPanel:
                    return new LoaderPanel();
                case PanelName.LoginPanel:
                    return new LoginPanel();
                case PanelName.MainPanel:
                    return new MainPanel();
                default:
                    return null;
            }
        }

        public static PanelViewBase GetView(PanelName pname)
        {
            switch (pname)
            {
                case PanelName.LoaderPanel:
                    return new LoaderPanelView();
                case PanelName.LoginPanel:
                    return new LoginPanelView();
                case PanelName.MainPanel:
                    return new MainPanelView();
                default:
                    return null;
            }
        }
    }
}