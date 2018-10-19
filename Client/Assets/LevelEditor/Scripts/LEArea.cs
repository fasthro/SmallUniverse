using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
        public Dictionary<string, LEGrid> decorations;

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

        // 区域坐标描述(使用区域描述之前请先调用 CalculateCoord 计算)
        public Vector3 coordLD = Vector3.zero;
        public Vector3 coordLU = Vector3.zero;
        public Vector3 coordRU = Vector3.zero;
        public Vector3 coordRD = Vector3.zero;

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
            decorations = new Dictionary<string, LEGrid>();

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

            foreach (KeyValuePair<string, LEGrid> item in decorations)
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
                else if (grid.function == GridFunction.Decoration)
                {
                    decorations.Add(key, grid);
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
                else if (grid.function == GridFunction.Decoration)
                {
                    decorations.Remove(key);
                }
            }
        }
        #endregion

        /// <summary>
        ///  计算区域描述
        /// </summary>
        public void CalculateCoord()
        {
            int index = 0;
            foreach (KeyValuePair<string, LEGrid> item in grids)
            {
                var grid = item.Value;
                if (index == 0)
                {
                    coordLD.x = grid.position.x;
                    coordLD.y = 0;
                    coordLD.z = grid.position.z;

                    coordLU.x = grid.position.x;
                    coordLU.y = 0;
                    coordLU.z = grid.position.z;

                    coordRU.x = grid.position.x;
                    coordRU.y = 0;
                    coordRU.z = grid.position.z;

                    coordRD.x = grid.position.x;
                    coordRD.y = 0;
                    coordRD.z = grid.position.z;
                }
                else {
                    // coordLD
                    if (grid.position.x < coordLD.x)
                        coordLD.x = grid.position.x;
                    if (grid.position.z < coordLD.z)
                        coordLD.z = grid.position.z;

                    // coordLU
                    if (grid.position.x < coordLU.x)
                        coordLU.x = grid.position.x;
                    if (grid.position.z > coordLU.z)
                        coordLU.z = grid.position.z;

                    // coordRU
                    if (grid.position.x > coordRU.x)
                        coordRU.x = grid.position.x;
                    if (grid.position.z > coordRU.z)
                        coordRU.z = grid.position.z;

                    // coordRD
                    if (grid.position.x > coordRD.x)
                        coordRD.x = grid.position.x;
                    if (grid.position.z < coordRD.z)
                        coordRD.z = grid.position.z;
                }
                index++;
            }
        }

        /// <summary>
        /// 导出xml
        /// </summary>
        /// <returns></returns>
        public string ExportXml()
        {
            string groundContent = string.Empty;
            string playerContent = string.Empty;
            string monsterContent = string.Empty;
            string doorContent = string.Empty;
            string trapContent = string.Empty;
            string transferContent = string.Empty;
            string decorationContent = string.Empty;

            foreach (KeyValuePair<string, LEGrid> gridItem in grids)
            {
                var grid = gridItem.Value;
                if (grid.function == GridFunction.Ground)
                {
                    groundContent += ExporGridtXml(grid);
                }
                else if (grid.function == GridFunction.Player)
                {
                    playerContent += ExporGridtXml(grid);
                }
                else if (grid.function == GridFunction.Monster)
                {
                    monsterContent += ExporGridtXml(grid);
                }
                else if (grid.function == GridFunction.Door)
                {
                    doorContent += ExporGridtXml(grid);
                }
                else if (grid.function == GridFunction.Trap)
                {
                    trapContent += ExporGridtXml(grid);
                }
                else if (grid.function == GridFunction.Transfer)
                {
                    transferContent += ExporGridtXml(grid);
                }
                else if (grid.function == GridFunction.Decoration)
                {
                    decorationContent += ExporGridtXml(grid);
                }
            }

            string template = LEUtils.LoadTemplate("Area.txt");
            template = template.Replace("{#area_index}", areaName);
            template = template.Replace("{#ground_content}", groundContent);
            template = template.Replace("{#player_content}", playerContent);
            template = template.Replace("{#monster_content}", monsterContent);
            template = template.Replace("{#door_content}", doorContent);
            template = template.Replace("{#trap_content}", trapContent);
            template = template.Replace("{#transfer_content}", transferContent);
            template = template.Replace("{#decoration_content}", decorationContent);

            return template;
        }

        /// <summary>
        /// 替换格子模版
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        private string ExporGridtXml(LEGrid grid)
        {
            string template = "<grid bundle_name='{#bundle_name}' asset_name='{#asset_name}' pos_x='{#pos_x}' pos_y='{#pos_y}' pos_z='{#pos_z}' angle_x='{#angle_x}' angle_y='{#angle_y}' angle_z='{#angle_z}'></grid>";
            template = template.Replace("{#bundle_name}", grid.prefabGo.bundleName);
            template = template.Replace("{#asset_name}", grid.prefabGo.name);
            template = template.Replace("{#pos_x}", grid.position.x.ToString());
            template = template.Replace("{#pos_y}", grid.position.y.ToString());
            template = template.Replace("{#pos_z}", grid.position.z.ToString());
            template = template.Replace("{#angle_x}", grid.rotationAngle.x.ToString());
            template = template.Replace("{#angle_y}", grid.rotationAngle.y.ToString());
            template = template.Replace("{#angle_z}", grid.rotationAngle.z.ToString());
            return template;
        }
    }
}
