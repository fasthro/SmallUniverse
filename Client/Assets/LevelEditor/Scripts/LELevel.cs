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

        // 格子
        private Dictionary<string, LEGrid> grids;

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

        // 玩家格子
        private LEGrid playerGrid;
        
        #region editor
        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            grids = new Dictionary<string, LEGrid>();

            var trans = gameObject.transform.Find(GridFunction.Ground.ToString());

            // groud
            var transCount = trans.childCount;
            for (int i = 0; i < transCount; i++)
            {
                var gt = trans.GetChild(i);
                var gtc = gt.childCount;

                // grid
                for (int k = 0; k < gtc; k++)
                {
                    var gridRoot = gt.GetChild(k);

                    var key = gridRoot.gameObject.name;
                    var grid = gridRoot.GetComponent<LEGrid>();
                    if (grid != null)
                        grids.Add(key, grid);
                }
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
                        var grid = gridRoot.GetComponent<LEGrid>();
                        if (grid != null)
                        {
                            grids.Add(key, grid);

                            // player grid
                            if (grid.function == GridFunction.Player)
                            {
                                playerGrid = grid;
                            }
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
        /// <param name="groud"></param>
        public void Draw(LEPrefabGo prefabGo, Vector3 position, Vector3 rotationAngle, GridFunction function, int groud)
        {
            string key = GetKey(position, groud);

            if (GetGrid(key))
                return;

            var go = GameObject.Instantiate(prefabGo.go) as GameObject;
            go.name = key;
            go.transform.parent = GetGridRoot(function, groud);
            go.transform.position = position;

            var grid = go.AddComponent<LEGrid>();
            grid.key = key;
            grid.prefabGo = prefabGo;
            grid.groud = groud;
            grid.position = position;
            grid.rotationAngle = rotationAngle;
            grid.function = function;

            SetGrid(key, grid);

            // 只允许有一个 Player Grid
            if (function == GridFunction.Player)
            {
                if (playerGrid != null)
                {
                    Erase(playerGrid.key);
                }
                playerGrid = grid;
            }
        }

        /// <summary>
        /// 擦除格子
        /// </summary>
        /// <param name="key"></param>
        public void Erase(string key)
        {
            var grid = GetGrid(key);
            if (grid == null)
                return;

            GameObject.DestroyImmediate(grid.gameObject);

            RemoveGrid(key);
        }

        #endregion

        #region grid map
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
        /// 设置格子，如果已经有就擦除之前的
        /// </summary>
        /// <param name="key"></param>
        /// <param name="grid"></param>
        public void SetGrid(string key, LEGrid grid)
        {
            if (grids.ContainsKey(key))
            {
                Erase(key);
            }
            grids.Add(key, grid);
        }

        /// <summary>
        /// 移除格子
        /// </summary>
        /// <param name="key"></param>
        public void RemoveGrid(string key)
        {
            if (grids.ContainsKey(key))
            {
                grids.Remove(key);
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
        /// <param name="group"></param>
        /// <returns></returns>
        public Transform GetGridRoot(GridFunction function, int group)
        {
            Transform trans = null;
            // Ground
            if (function == GridFunction.Ground)
            {
                trans = gameObject.transform.Find(GridFunction.Ground.ToString());

                var transCount = trans.childCount;
                for (int i = 0; i < transCount; i++)
                {
                    var gt = trans.GetChild(i);
                    if (int.Parse(gt.name) == group)
                    {
                        return gt;
                    }
                }

                GameObject groupGo = new GameObject();
                groupGo.name = group.ToString();
                groupGo.transform.parent = trans;

                return groupGo.transform;
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
        /// <param name="pos"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public static string GetKey(Vector3 pos, int group)
        {
            return string.Format("x:{0}/y:{1}/z:{2}/group:{3}", pos.x, pos.y, pos.z, group);
        }

#endif
    }
}
