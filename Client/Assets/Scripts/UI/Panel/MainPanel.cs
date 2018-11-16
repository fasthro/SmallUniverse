// Generate By @ExportViewCode
using SmallUniverse.UI;
using SmallUniverse.Manager;
using UnityEngine;

namespace SmallUniverse.UI
{
    public class MainPanel : PanelBase
    {
        // 视图 PanelView
        private MainPanelView view;
        
        // 移动摇杆
        private VirtualMoveJoy m_moveJoy;
        private Vector3 m_move;

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
            
            // 移动摇杆
            m_moveJoy = new VirtualMoveJoy();
            m_moveJoy.ui = view.joystick;
            m_moveJoy.slider = view.touch_joystick;
            m_moveJoy.startTransition = view.joystick.GetTransition("touch_start");
            m_moveJoy.endTransition = view.joystick.GetTransition("touch_end");
            m_moveJoy.Initialize(Game.virtualJoy.moveJoy);

            m_moveJoy.moveJoyHandler -= MoveJoyHandler;
            m_moveJoy.moveJoyHandler += MoveJoyHandler;
        }


        private void MoveJoyHandler(Vector2 move)
        {
            if(Game.hero != null)
            {
                m_move.x = move.x;
                m_move.y = 0;
                m_move.z = move.y;

                Game.hero.Move(m_move, Time.deltaTime);
            }
        }

        protected override void OnHide()
        {
            base.OnHide();

            view.Dispose();

            if(m_moveJoy != null)
            {
                m_moveJoy.moveJoyHandler -= MoveJoyHandler;
                m_moveJoy = null;
            }
        }
        
    }
}
