using FairyGUI;

namespace SU.UI
{
    public class PanelViewBase
    {
        public bool Initialized = false;
        protected GComponent root;

        public void SetRoot(GComponent _root)
        {
            root = _root;
        }

        public virtual void Get()
        {

        }

        public virtual void Init()
        {
            Initialized = true;
        }

        public virtual void Dispose()
        {
            Initialized = false;
        }
    }
}
