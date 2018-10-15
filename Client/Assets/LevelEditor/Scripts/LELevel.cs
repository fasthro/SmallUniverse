using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace SU.Editor.LevelEditor
{
    public class LELevel : MonoBehaviour
    {
#if UNITY_EDITOR
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

        // 关卡宽
        public int width
        {
            get {
                return 0;
            }
        }

        // 关卡长
        public int length
        {
            get
            {
                return 0;
            }
        }
        
        #region editor
        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            areas = new Dictionary<string, LEArea>();

            var trans = gameObject.transform.Find(GridFunction.Area.ToString());

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
                if (!name.Equals("value__") && !name.Equals(GridFunction.Area.ToString()))
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
        /// 生成关卡数据
        /// </summary>
        public void GenerateLevelData()
        {
            /*
            string content = string.Empty;

            // bundle 资源列表
            Dictionary<string, Dictionary<string, string>> bundles = new Dictionary<string, Dictionary<string, string>>();

            // layer 层
            int layerMin = 0;
            int layerMax = 0;

            // 关卡尺寸
            int widthMin = 0;
            int widthMax = 0;
            int lengthMin = 0;
            int lengthMax = 0;

            // 角色出生点格子
            LEGrid characterPoint = null;

            // 距离出生点的最大半径
            int radius = 0;

            // layer
            int layer;
            foreach (KeyValuePair<int, Transform> layerItem in groupRoots)
            {
                if (layerItem.Value.childCount == 0)
                    continue;

                // layer
                layer = int.Parse(layerItem.Value.gameObject.name);
                if (layer < layerMin)
                    layerMin = layer;
                if (layer > layerMax)
                    layerMax = layer;

                content += string.Format("  <layer name='{0}'>\n", layer);
                foreach (KeyValuePair<string, LEGrid> gridItem in grids)
                {
                    var grid = gridItem.Value;
                    if (grid.group == layer && grid.function == GridFunction.None)
                    {
                        content += "    <grid asset_name='" + grid.assetName + "' bundle_name='" + grid.bundleName + "' pos_x='" + grid.position.x + "' pos_y='" + grid.position.y + "' pos_z='" + grid.position.z + "' angle_x='" + grid.rotationAngle.x + "' angle_y='" + grid.rotationAngle.y + "' angle_z='" + grid.rotationAngle.z + "' />\n";

                        // 资源记录
                        if (!bundles.ContainsKey(grid.bundleName))
                        {
                            Dictionary<string, string> assets = new Dictionary<string, string>();
                            assets.Add(grid.assetName, grid.assetName);

                            bundles.Add(grid.bundleName, assets);
                        }
                        else {
                            var assets = bundles[grid.bundleName];
                            if (!assets.ContainsKey(grid.assetName))
                            {
                                assets.Add(grid.assetName, grid.assetName);
                            }
                        }
                    }

                    // character point
                    if (grid.function == GridFunction.CharacterPoint)
                    {
                        characterPoint = grid;
                    }

                    // size
                    if (grid.position.x < widthMin)
                        widthMin = (int)grid.position.x;
                    if (grid.position.x > widthMax)
                        widthMax = (int)grid.position.x;

                    if (grid.position.z < lengthMin)
                        lengthMin = (int)grid.position.z;
                    if (grid.position.z > lengthMax)
                        lengthMax = (int)grid.position.z;
                }
                content += "  </layer>\n";
            }

            // bundles
            foreach (KeyValuePair<string, Dictionary<string, string>> bundle in bundles)
            {
                content += string.Format("  <bundle name='{0}'>\n", bundle.Key);
                foreach (KeyValuePair<string, string> asset in bundle.Value)
                {
                    content += "    <asset name='" + asset.Key + "' />\n";
                }
                content += "  </bundle>\n";
            }

            // character point
            if (characterPoint != null)
            {
                content += "  <character_point pos_x='" + characterPoint.position.x + "' pos_y='" + characterPoint.position.y + "' pos_z='" + characterPoint.position.z + "' angle_x='" + characterPoint.rotationAngle.x + "' angle_y='" + characterPoint.rotationAngle.y + "' angle_z='" + characterPoint.rotationAngle.z + "' layer='" + characterPoint.group + "' />\n";
            }
            else {
                Debug.LogError("此关卡角色出生点未设置");
                return;
            }

            // radius
            var w1 = Mathf.Abs(widthMin) - (int)Mathf.Abs(characterPoint.position.x);
            var w2 = Mathf.Abs(widthMax) - (int)Mathf.Abs(characterPoint.position.x);
            var w = w1 > w2 ? w1 : w2;
            w += 1;

            var len1 = Mathf.Abs(lengthMin) - (int)Mathf.Abs(characterPoint.position.z);
            var len2 = Mathf.Abs(lengthMax) - (int)Mathf.Abs(characterPoint.position.z);
            var len = len1 > len2 ? len1 : len2;
            len += 1;

           radius = w > len ? w : len;
            radius += 1;

            // save xml
            var assetObj = AssetDatabase.LoadAssetAtPath(LEConst.LevelMapTemplatePath, typeof(TextAsset));
            TextAsset template = assetObj as TextAsset;
            string text = template.text.Replace("{#levelMapName}", levelName);
            text = text.Replace("{#layerMin}", layerMin.ToString());
            text = text.Replace("{#layerMax}", layerMax.ToString());
            text = text.Replace("{#widthMin}", widthMin.ToString());
            text = text.Replace("{#widthMax}", widthMax.ToString());
            text = text.Replace("{#lengthMin}", lengthMin.ToString());
            text = text.Replace("{#lengthMax}", lengthMax.ToString());
            text = text.Replace("{#radius}", radius.ToString());
            text = text.Replace("{#content}", content);

            string file = LEUtils.GetLevelDataPath(levelName);
            string dir = Path.GetDirectoryName(file);
            if (File.Exists(dir))
            {
                File.Delete(dir);
            }
            File.WriteAllText(file, text);
            AssetDatabase.Refresh();
            */
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
            if (function == GridFunction.Area)
            {
                trans = gameObject.transform.Find(GridFunction.Area.ToString());

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
#endif
    }
}
