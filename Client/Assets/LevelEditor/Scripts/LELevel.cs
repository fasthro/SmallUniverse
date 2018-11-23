using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace SmallUniverse.GameEditor.LevelEditor
{
    public class LELevel : MonoBehaviour
    {
        // 单例模式引用
        private static LELevel _inst;
        public static LELevel Inst {
            get {
                if (_inst == null)
                {
                    _inst = GameObject.Find(LEConst.EditorLevelName).transform.GetComponent <LELevel>();
                }
                return _inst;
            }
        }

        // 关卡名称
        public string levelName;
        // 区域字典
        public Dictionary<string, LEArea> areas;

        // navmesh
        public NavMeshSurface navMeshSurface;

        #region editor
        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            areas = new Dictionary<string, LEArea>();

            var trans = gameObject.transform.Find(GridFunction.Ground.ToString());

            navMeshSurface = trans.gameObject.GetComponent<NavMeshSurface>();

            // area
            var transCount = trans.childCount;
            for (int i = 0; i < transCount; i++)
            {
                var areaTrans = trans.GetChild(i);
                var area = areaTrans.GetComponent<LEArea>();
                var gridCount = areaTrans.childCount;

                //area
                area.Initialize(areaTrans.gameObject.name);
                areas.Add(area.gameObject.name, area);
            }
            
            // other
            FieldInfo[] fields = typeof(GridFunction).GetFields();
            for (int i = 0; i < fields.Length; i++)
            {
                var name = fields[i].Name;
                if (!name.Equals("value__") && !name.Equals(GridFunction.Ground.ToString()))
                {
                    trans = gameObject.transform.Find(name);
                    transCount = trans.childCount;
                    for (int k = 0; k < transCount; k++)
                    {
                        var gridRoot = trans.GetChild(k);
                        var key = gridRoot.gameObject.name;
                        var areaName = GetAreaToKey(key);
                        var grid = gridRoot.GetComponent<LEGrid>();
                        if (grid != null)
                        {
                            AddGrid(grid.position, areaName, grid);
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// 画场景功能物体
        /// </summary>
        /// <param name="prefabGo"></param>
        /// <param name="position"></param>
        /// <param name="rotationAngle"></param>
        /// <param name="function"></param>
        /// <param name="areaName"></param>
        public void Draw(LEPrefabGo prefabGo, Vector3 position, Vector3 rotationAngle, GridFunction function, string areaName)
        {
            string key = GetKey(position, areaName);
            if (GetGrid(position, areaName))
                return;

            var go = GameObject.Instantiate(prefabGo.go) as GameObject;
            go.name = key;
            go.transform.parent = GetGridRoot(function, areaName);
            go.transform.position = position;

            var grid = go.AddComponent<LEGrid>();
            grid.key = key;
            grid.prefabGo = prefabGo;
            grid.area = int.Parse(areaName);
            grid.position = position;
            grid.rotationAngle = rotationAngle;
            grid.function = function;
            
           var addSucceed = AddGrid(position, areaName, grid);
            if (!addSucceed)
            {
                DestroyImmediate(grid.gameObject);
            }
            else {
                if (function == GridFunction.Player)
                {
                    List<LEGrid> playerGrids;
                    foreach (KeyValuePair<string, LEArea> areaItem in areas)
                    {
                        var area = areaItem.Value;
                       playerGrids = new List<LEGrid>();
                        foreach (KeyValuePair<string, LEGrid> gridItem in area.players)
                        {
                            var player = gridItem.Value;
                            if (!player.key.Equals(grid.key))
                            {
                                playerGrids.Add(player);
                            }
                        }

                        for (int i = 0; i < playerGrids.Count; i++)
                        {
                            Erase(playerGrids[i].position, playerGrids[i].area.ToString());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 擦除格子
        /// </summary>
        /// <param name="key"></param>
        public void Erase(Vector3 position, string areaName)
        {
            var grid = GetGrid(position, areaName);
            if (grid != null)
            {
                RemoveGrid(position, areaName);

                GameObject.DestroyImmediate(grid.gameObject);
            }
        }
        #endregion

        /// <summary>
        /// 获取网格尺寸
        /// </summary>
        /// <returns></returns>
        public Vector2Int GetGizmoDimension()
        {
            Vector2 dim = new Vector2(20, 20);
            int index = 0;
            float x = 0;
            float y = 0;
            foreach (KeyValuePair<string, LEArea> item in areas)
            {
                var area = item.Value;
                area.CalculateCoord();

                x = Mathf.Abs(area.coordLD.x) > Mathf.Abs(area.coordRD.x) ? Mathf.Abs(area.coordLD.x) : Mathf.Abs(area.coordRD.x);
                y = Mathf.Abs(area.coordLD.z) > Mathf.Abs(area.coordLU.z) ? Mathf.Abs(area.coordLD.z) : Mathf.Abs(area.coordLU.z);
                
                if (index == 0)
                {
                    dim.x = x;
                    dim.y = y;
                }
                else {
                    if (x > dim.x)
                        dim.x = x;
                    if (y > dim.y)
                        dim.y = y;
                }
                index++;
            }
            return new Vector2Int((int)dim.x * 2 + 4, (int)dim.y * 2 + 4);
        }

        #region area and grid
        /// <summary>
        /// 区域名称
        /// </summary>
        /// <param name="areaName"></param>
        /// <returns></returns>
        public LEArea GetArea(string areaName)
        {
            LEArea area = null;
            if (areas.TryGetValue(areaName, out area))
            {
                return area;
            }
            return null;
        }

        /// <summary>
        /// 获取格子
        /// </summary>
        /// <param name="position"></param>
        /// <param name="areaName"></param>
        /// <returns></returns>
        public LEGrid GetGrid(Vector3 position, string areaName)
        {
            string key = GetKey(position, areaName);
            LEArea area = GetArea(areaName);
            if (area != null)
            {
                return area.GetGrid(key);
            }
            return null;
        }

        /// <summary>
        /// 添加格子
        /// </summary>
        /// <param name="position"></param>
        /// <param name="areaName"></param>
        /// <param name="grid"></param>
        public bool AddGrid(Vector3 position, string areaName, LEGrid grid)
        {
            string key = GetKey(position, areaName);
            LEArea area = GetArea(areaName);
            if (area != null)
            {
                return area.AddGrid(key, grid);
            }
            return false;
        }

        /// <summary>
        /// 移除格子
        /// </summary>
        /// <param name="position"></param>
        /// <param name="areaName"></param>
        public void RemoveGrid(Vector3 position, string areaName)
        {
            string key = GetKey(position, areaName);
            LEArea area = GetArea(areaName);
            if (area != null)
            {
                area.RemoveGrid(key);
            }
        }
        #endregion

        #region  level data
        /// <summary>
        /// 导出关卡数据
        /// </summary>
        public void ExportXml()
        {
            string content = string.Empty;
            int areaCount = 0;
            foreach (KeyValuePair<string, LEArea> areaItem in areas)
            {
                content += areaItem.Value.ExportXml();
                areaCount++;
            }

            string template = LEUtils.LoadTemplate("Level.txt");
            template = template.Replace("{#level_name}", levelName);
            template = template.Replace("{#area_count}", areaCount.ToString());
            template = template.Replace("{#content}", content);

            string file = LEUtils.GetLevelDataPath(levelName);
            string dir = Path.GetDirectoryName(file);
            if (File.Exists(dir))
            {
                File.Delete(dir);
            }
            File.WriteAllText(file, template);
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }
        #endregion

        /// <summary>
        /// 获取Grid节点
        /// </summary>
        /// <param name="areaName"></param>
        /// <returns></returns>
        public Transform GetGridRoot(GridFunction function, string areaName)
        {
            Transform trans = null;
            // Ground
            if (function == GridFunction.Ground)
            {
                trans = gameObject.transform.Find(GridFunction.Ground.ToString());

                var transCount = trans.childCount;
                for (int i = 0; i < transCount; i++)
                {
                    var areaTrans = trans.GetChild(i);
                    if (areaTrans.name.Equals(areaName))
                    {
                        return areaTrans;
                    }
                }

                GameObject areaGo = new GameObject();
                areaGo.name = areaName;
                areaGo.transform.parent = trans;
                var area = areaGo.AddComponent<LEArea>();
                area.Initialize(areaName);
                areas.Add(areaName, area);

                return areaGo.transform;
            }
            else
            {
                // 创建功能节点
                FieldInfo[] fields = typeof(GridFunction).GetFields();
                for (int i = 0; i < fields.Length; i++)
                {
                    var name = fields[i].Name;
                    if (!name.Equals("value__"))
                    {
                        if (function.ToString().Equals(name))
                        {
                            return gameObject.transform.Find(name);
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 获取key
        /// </summary>
        /// <param name="position"></param>
        /// <param name="areaName"></param>
        /// <returns></returns>
        public static string GetKey(Vector3 position, string areaName)
        {
            return string.Format("x:{0}/y:{1}/z:{2}/area:{3}", position.x, position.y, position.z, areaName);
        }

        /// <summary>
        /// 获取运行时key
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static string GetRunTimeKey(Vector3 position)
        {
            return string.Format("{0}|{1}|{2}", position.x, position.y, position.z);
        }

        /// <summary>
        /// 通过key获取area
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAreaToKey(string key)
        {
            string[] strs = key.Split('/');
            string areaStr = strs[3];
            return areaStr.Substring(5);
        }
    }
}
