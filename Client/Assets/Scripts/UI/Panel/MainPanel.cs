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

        // 普通攻击
        private VirtualAttackJoy m_attackJoy;
        private VirtualSkillJoy m_skillJoy1;
        private VirtualSkillJoy m_skillJoy2;
        private VirtualSkillJoy m_skillJoy3;

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
            m_moveJoy.ui = view.move_joy;
            m_moveJoy.slider = view.touch_move_joy;
            m_moveJoy.startTransition = view.move_joy.GetTransition("touch_start");
            m_moveJoy.endTransition = view.move_joy.GetTransition("touch_end");
            m_moveJoy.Initialize(Game.virtualJoy.moveJoy);

            m_moveJoy.moveJoyHandler -= MoveJoyHandler;
            m_moveJoy.moveJoyHandler += MoveJoyHandler;

            // 普通攻击
            m_attackJoy = new VirtualAttackJoy();
            m_attackJoy.ui = view.attack_joy;
            m_attackJoy.Initialize(Game.virtualJoy.attackJoy);
            
            // skill
            m_skillJoy1 = new VirtualSkillJoy();
            m_skillJoy1.ui = view.skill_1_joy;
            m_skillJoy1.Initialize(Game.virtualJoy.joys[0]);

            m_skillJoy2 = new VirtualSkillJoy();
            m_skillJoy2.ui = view.skill_2_joy;
            m_skillJoy2.Initialize(Game.virtualJoy.joys[1]);

            m_skillJoy3 = new VirtualSkillJoy();
            m_skillJoy3.ui = view.skill_3_joy;
            m_skillJoy3.Initialize(Game.virtualJoy.joys[2]);
        }


        private void MoveJoyHandler(Vector2 move, bool isMove)
        {
            if(Game.hero != null)
            {
                m_move.x = move.x;
                m_move.y = 0;
                m_move.z = move.y;

                Game.hero.Move(m_move, isMove);
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
