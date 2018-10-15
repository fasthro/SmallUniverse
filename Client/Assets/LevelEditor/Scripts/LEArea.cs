using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SU.Editor.LevelEditor
{
    public class LEArea : MonoBehaviour
    {
        public string areaName;
        // 格子字典
        private Dictionary<string, LEGrid> grids;

        public Dictionary<string, LEGrid> players;
        public Dictionary<string, LEGrid> monsters;
        public Dictionary<string, LEGrid> doors;
        public Dictionary<string, LEGrid> traps;
        public Dictionary<string, LEGrid> transfers;

        // 当前区域是否显示
        private bool _showing = true;
        public bool showing {
            get {
                return _showing;
            }
            set {
                _showing = value;

                SetShowing();
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize(string ame)
        {
            areaName = name;
            grids = new Dictionary<string, LEGrid>();
            players = new Dictionary<string, LEGrid>();
            monsters = new Dictionary<string, LEGrid>();
            doors = new Dictionary<string, LEGrid>();
            traps = new Dictionary<string, LEGrid>();
            transfers = new Dictionary<string, LEGrid>();

            int count = transform.childCount;
            for (int k = 0; k < count; k++)
            {
                var trans = transform.GetChild(k);

                var key = trans.gameObject.name;
                var grid = trans.GetComponent<LEGrid>();

                if (grid != null)
                    grids.Add(key, grid);
            }

            showing = true;
        }

        /// <summary>
        /// 设置区域是否显示
        /// </summary>
        private void SetShowing()
        {
            gameObject.SetActive(showing);

            foreach (KeyValuePair<string, LEGrid> item in players)
            {
                item.Value.gameObject.SetActive(showing);
            }

            foreach (KeyValuePair<string, LEGrid> item in monsters)
            {
                item.Value.gameObject.SetActive(showing);
            }

            foreach (KeyValuePair<string, LEGrid> item in doors)
            {
                item.Value.gameObject.SetActive(showing);
            }

            foreach (KeyValuePair<string, LEGrid> item in traps)
            {
                item.Value.gameObject.SetActive(showing);
            }

            foreach (KeyValuePair<string, LEGrid> item in transfers)
            {
                item.Value.gameObject.SetActive(showing);
            }
        }

        #region grid
        /// <summary>
        /// 获取格子
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public LEGrid GetGrid(string key)
        {
            LEGrid grid = null;
            if (grids.TryGetValue(key, out grid))
            {
                return grid;
            }
            return null;
        }

        /// <summary>
        /// 添加格子
        /// </summary>
        /// <param name="key"></param>
        /// <param name="grid"></param>
        public bool AddGrid(string key, LEGrid grid)
        {
            if (!grids.ContainsKey(key) && grid != null)
            {
                grids.Add(key, grid);

                if (grid.function == GridFunction.Player)
                {
                    players.Add(key, grid);
                }
                else if (grid.function == GridFunction.Monster)
                {
                    monsters.Add(key, grid);
                }
                else if (grid.function == GridFunction.Door)
                {
                    traps.Add(key, grid);
                }
                else if (grid.function == GridFunction.Transfer)
                {
                    transfers.Add(key, grid);
                }

                return true;
            }
            return false;
        }

        /// <summary>
        /// 移除格子
        /// </summary>
        /// <param name="key"></param>
        public void RemoveGrid(string key)
        {
            var grid = GetGrid(key);
            if (grid != null)
            {
                grids.Remove(key);

                if (grid.function == GridFunction.Player)
                {
                    players.Remove(key);
                }
                else if (grid.function == GridFunction.Monster)
                {
                    monsters.Remove(key);
                }
                else if (grid.function == GridFunction.Door)
                {
                    traps.Remove(key);
                }
                else if (grid.function == GridFunction.Transfer)
                {
                    transfers.Remove(key);
                }
            }
        }
        #endregion
    }
}
