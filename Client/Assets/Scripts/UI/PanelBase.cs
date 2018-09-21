using FairyGUI;
using SU.Manager;
using UnityEngine;

namespace SU.UI
{
    public class PanelBase : Window
    {
        // 主资源包
        protected string mainPackage;
        // 资源包列表资源列表
        protected string[] packages;
        // 面板名称
        protected PanelName pname;
        // 面板组件名称
        protected string pcname;
        // 参数列表
        protected object[] parameters;

        // layer
        public PanelLayer layer;

        // 是否正在加载
        protected bool isLoaded = false;

        // 资源管理器
        protected ResManager resMgr = null;
        // 面板管理器
        protected PanelManager panelMgr = null;

        // 面板显示
        private bool isShow = false;
        public bool IsShow {
            get {
                return IsShow;
            }
        }

        public PanelBase() : base()
        {
            resMgr = Game.GetManager<ResManager>();
            panelMgr = Game.GetManager<PanelManager>();
        }

        /// <summary>
        /// 创建面板
        /// </summary>
        public void CreatePanel()
        {
            if (!isLoaded)
            {
                LoadPackage();

                isLoaded = true;
            }
            Init();
            Show();
        }

        /// <summary>
        /// 关闭面板
        /// </summary>
        public void ClosePanel()
        {
            isShow = false;

            Hide();
        }

        protected override void OnShown()
        {
            base.OnShown();

            isShow = true;
            MakeFullScreen();
            contentPane = UIPackage.CreateObject(mainPackage, pcname).asCom;
            contentPane.SetSize(GRoot.inst.width, GRoot.inst.height);
            gameObjectName = pname.ToString();
            sortingOrder = panelMgr.GetTopSortingOrder(layer);
        }

        protected override void OnHide()
        {
            base.OnHide();

            Dispose();
            RemovePackage();
        }

        /// <summary>
        /// 加载所需资源包
        /// </summary>
        private void LoadPackage()
        {
           panelMgr.AddPackage(GetPackagePath(mainPackage));
            for (int i = 0; i < packages.Length; i++)
            {
                panelMgr.AddPackage(GetPackagePath(packages[i]));
            }
        }

        /// <summary>
        /// 移除资源包
        /// </summary>
        private void RemovePackage()
        {
            panelMgr.RemovePackage(GetPackagePath(mainPackage));
            for (int i = 0; i < packages.Length; i++)
            {
                panelMgr.RemovePackage(GetPackagePath(packages[i]));
            }
        }

        #region tools
        /// <summary>
        /// 获取资源所在路径
        /// </summary>
        /// <param name="_source"></param>
        /// <returns></returns>
        private string GetPackagePath(string _source)
        {
            return string.Format("UI/{0}/{1}", _source, _source);
        }

        #endregion
    }
}

