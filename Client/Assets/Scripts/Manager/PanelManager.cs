using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using SU.UI;
using System;
using System.Reflection;

namespace SU.Manager
{
    public enum PanelLayer
    {
        LOADER = 500000000,
        NET = 400000000,
        MESSAGE = 300000000,
        WINDOW = 200000000,
        SCENE = 100000000,
    }

    public class PanelManager : BaseManager
    {
        // panel 面板缓存
        private Dictionary<PanelName, PanelBase> panels;
        // panel 资源包
        private Dictionary<string, int> packages;

        public override void Initialize()
        {
            panels = new Dictionary<PanelName, PanelBase>();
            packages= new Dictionary<string, int>();
        }
        
        public override void OnFixedUpdate(float deltaTime)
        {

        }

        public override void OnUpdate(float deltaTime)
        {

        }

        public override void OnDispose()
        {

        }

        /// <summary>
        /// 创建 Panel
        /// </summary>
        /// <param name="name"></param>
        public PanelBase CreatePanel(PanelName name)
        {
            PanelBase panel = null;
            if (!panels.TryGetValue(name, out panel))
            {
                panel = PanelMap.GetPanel(name);
                panels.Add(name, panel);
            }
            panel.CreatePanel();

            return panel;
        } 

        /// <summary>
        /// 关闭UI
        /// </summary>
        /// <param name="name"></param>
        public void ClosePanel(PanelName name)
        {
            PanelBase panel = null;
            if (panels.TryGetValue(name, out panel))
            {
                panel.ClosePanel();
                panels.Remove(name);
            }
        }

        /// <summary>
        /// 获取当前层最高 sorting order
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        public int GetTopSortingOrder(PanelLayer layer)
        {
            List<PanelBase> _panels = new List<PanelBase>();
            foreach (KeyValuePair<PanelName, PanelBase> item in panels)
            {
                if (item.Value.layer == layer)
                {
                    _panels.Add(item.Value);
                }
            }
            
            Comparison<PanelBase> comparison = new Comparison<PanelBase>((PanelBase x, PanelBase y) =>
             {
                 if (x.sortingOrder < y.sortingOrder)
                     return -1;
                 else if (x.sortingOrder == y.sortingOrder)
                     return 0;
                 else
                     return 1;
             });
            _panels.Sort(comparison);

            // 重新排序当前layer 层的 sortingOrder
            int order = (int)layer;
            for (int i = 0; i < _panels.Count; i++)
            {
                _panels[i].sortingOrder = order + i;
            }

            return order + _panels.Count;
        }

        #region package

        public void AddPackage(string pname)
        {
            if (!packages.ContainsKey(pname))
            {
                UIPackage.AddPackage(pname);

                packages.Add(pname, 1);
            }
            else {
                packages[pname]++;
            }
        }

        public void RemovePackage(string pname)
        {
            if (packages.ContainsKey(pname))
            {
                packages[pname]--;

                if (packages[pname] <= 0)
                {
                    packages.Remove(pname);

                    UIPackage.RemovePackage(pname, true);
                }
            }
        }

        #endregion
    }
}

