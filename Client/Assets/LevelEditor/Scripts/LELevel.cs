using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SU.Editor.LevelEditor
{
    public class LELevel : MonoBehaviour
    {
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

        // 关卡场景名称
        public string levelSceneName;
        // 格子数据字典
        private Dictionary<string, LEGrid> gridMap;
        // 层数据字典
        private Dictionary<int, Transform> layerMap;
        // layer 节点
        private Transform layerTransform;

        #region editor
        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            gridMap = new Dictionary<string, LEGrid>();
            layerMap = new Dictionary<int, Transform>();

            layerTransform = gameObject.transform.Find("Layers");
            
            var layerCount = layerTransform.childCount;
            for (int i = 0; i < layerCount; i++)
            {
                var lt = layerTransform.GetChild(i);
                var layer = int.Parse(lt.gameObject.name);

                if (!layerMap.ContainsKey(layer))
                {
                    layerMap.Add(layer, lt);
                }
                else {
                    Debug.LogError("The layer already exists. layer : " + layer);
                }
            }

            foreach (KeyValuePair<int, Transform> layer in layerMap)
            {
                var count = layer.Value.childCount;
                for (int i = 0; i < count; i++)
                {
                    var gt = layer.Value.GetChild(i);
                    var key = gt.gameObject.name;
                    var grid = gt.GetComponent<LEGrid>();
                    if (grid != null)
                    {
                        gridMap.Add(key, grid);
                    }
                    else {
                        Debug.LogError("grid is null. layer" + layer.Key + " : " + key);
                    }
                }
            }
        }

        /// <summary>
        /// 获取layer节点
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        public Transform GetLayerTransform(int layer)
        {
            if (layerMap.ContainsKey(layer))
            {
                return layerMap[layer];
            }

            GameObject layerGo = new GameObject();
            layerGo.name = layer.ToString();
            layerGo.transform.parent = layerTransform;

            layerMap.Add(layer, layerGo.transform);

            return layerGo.transform;
        }


        /// <summary>
        /// 画格子
        /// </summary>
        /// <param name="repositoryName">资源库名称</param>
        /// <param name="ink">笔芯</param>
        /// <param name="inkPath">笔芯路径</param>
        /// <param name="pos">位置</param>
        /// <param name="layer">层</param>
        public void Draw(string repositoryName, GameObject ink, string inkPath, Vector3 pos, int layer)
        {
            string key = GetKey(pos, layer);

            if (GetGrid(key))
                return;

            var go = GameObject.Instantiate(ink) as GameObject;
            go.name = key;
            go.transform.parent = GetLayerTransform(layer);
            go.transform.position = pos;

            var grid = go.AddComponent<LEGrid>();
            grid.key = key;
            grid.repositoryName = repositoryName;
            grid.path = inkPath;
            grid.layer = layer;
            grid.position = pos;
            grid.rotationAngle = Vector3.zero;

            SetGrid(key, grid);
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
            if (gridMap.TryGetValue(key, out grid))
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
            if (gridMap.ContainsKey(key))
            {
                Erase(key);
            }
            gridMap.Add(key, grid);
        }

        /// <summary>
        /// 移除格子
        /// </summary>
        /// <param name="key"></param>
        public void RemoveGrid(string key)
        {
            if (gridMap.ContainsKey(key))
            {
                gridMap.Remove(key);
            }
        }
        #endregion

        #region  level data
        /// <summary>
        /// 生成关卡数据
        /// </summary>
        public void GenerateLevelData()
        {
            string content = string.Empty;

            // layer
            int layer;
            content = "  <layers>\n";
            foreach (KeyValuePair<int, Transform> layerItem in layerMap)
            {
                if (layerItem.Value.childCount == 0)
                    continue;

                layer = int.Parse(layerItem.Value.gameObject.name);
                content += string.Format("    <layer name='{0}'>\n", layer);

                foreach (KeyValuePair<string, LEGrid> gridItem in gridMap)
                {
                    var grid = gridItem.Value;
                    if (grid.layer == layer)
                    {
                        content += "      <grid path='" + grid.path + "' pos_x='" + grid.position.x + "' pos_y='" + grid.position.y + "' pos_z='" + grid.position.z + "' angle_x='" + grid.rotationAngle.x + "' angle_y='" + grid.rotationAngle.y + "' angle_z='" + grid.rotationAngle.z + "' />\n";
                    }
                }

                content += "    </layer>\n";
            }
            content += "  </layers>\n";

            // save
            var assetObj = AssetDatabase.LoadAssetAtPath(LEConst.LevelMapTemplatePath, typeof(TextAsset));
            TextAsset template = assetObj as TextAsset;
            string text = template.text.Replace("{#levelMapName}", levelSceneName);
            text = text.Replace("{#content}", content);

            string file = LEUtils.GetLevelDataPath(levelSceneName);
            string dir = Path.GetDirectoryName(file);
            if (File.Exists(dir))
            {
                File.Delete(dir);
            }
            File.WriteAllText(file, text);
            AssetDatabase.Refresh();
        }
        #endregion

        /// <summary>
        /// 获取key
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public static string GetKey(Vector3 pos, int layer)
        {
            return string.Format("x:{0}/y:{1}/z:{2}/layer:{3}", pos.x, pos.y, pos.z, layer);
        }
    }
}
