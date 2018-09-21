// Generate By @ExportViewCode
using SU.UI;
using SU.Manager;

public class LoginPanel : PanelBase
{
    // 视图 PanelView
    private LoginPanelView view;

    public LoginPanel(params string[] _parameters) : base()
    {
        pname = PanelName.LoginPanel;
        view = PanelMap.GetView(pname) as LoginPanelView;
        mainPackage = "login_panel";
        packages = new string[] {"common"};
        pcname = "login_panel";
        parameters = _parameters;
        layer = PanelLayer.WINDOW;
    }

    protected override void OnShown()
    {
        base.OnShown();

        view.SetRoot(contentPane);
        view.Get();
        view.Init();

        view.begin_game_btn.onClick.Set(this.OckBeginGame);
    }

    protected override void OnHide()
    {
        base.OnHide();

        view.Dispose();
    }

    private void OckBeginGame()
    {
        Game.GetManager<LevelManager>().LoadLevel(LevelType.MainScene);
    }
}