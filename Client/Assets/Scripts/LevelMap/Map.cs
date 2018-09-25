using Mono.Xml;
using SU.Behaviour;
using SU.Manager;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

namespace SU.Level
{
    public class Map : GameBehaviour
    {
        // 资源管理
        private ResManager resMgr;
        
        // 地图名称
        private string mapName;
        // 地图数据配置
        private string mapDataStr;

        // 玩家出生点坐标
        private Vector3 bornPosition;
        // 最小层
        private int layerMin;
        // 最大层
        private int layerMax;
        // 宽度
        private int widthMin;
        private int widthMax;
        // 长度
        private int lengthMin;
        private int lengthMax;
        // 距离出生点最大维度
        private int largestDimension;
        // 地面层<layer, MapLayer>
        private Dictionary<int, MapLayer> mapLayers;
        // 资源<bundleName, MapAsset>
        private Dictionary<string, MapAsset> mapAssets;

        // layer 层节点
        private Transform layerTrans;

        protected override void OnAwake()
        {
            base.OnAwake();

            resMgr = Game.GetManager<ResManager>();
            mapLayers = new Dictionary<int, MapLayer>();
            mapAssets = new Dictionary<string, MapAsset>();
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_mapName"></param>
        public void Initialize(string _mapName)
        {
            mapName = _mapName;
            
            // 加载配置
            var mapBundleName = "level/scene/" + mapName.ToLower();
            var mapBundle = resMgr.LoadAssetBundle(mapBundleName);
            var textAsset = mapBundle.LoadAsset(mapName + ".xml") as TextAsset;
            mapDataStr = textAsset.text;
            resMgr.UnLoadAssetBundle(mapBundleName);

            // 解析配置
            ParseMap();

            // 加载bundle
            LoadBundle();

            // 生成地图
            StartCoroutine(GenerateMap());
        }

        #region map asset
        /// <summary>
        ///  加载 MapAsset bundle
        /// </summary>
        private void LoadBundle()
        {
            foreach (KeyValuePair<string, MapAsset> asset in mapAssets)
            {
                asset.Value.Load();
            }
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="assetName"></param>
        /// <returns></returns>
        private T GetAsset<T>(string bundleName, string assetName) where T: class
        {
            MapAsset asset = null;
            Object obj = null;
            if (mapAssets.TryGetValue(bundleName, out asset))
            {
                obj = asset.GetAsset(assetName);
            }
            if (obj != null)
            {
                return obj as T;
            }
            return null;
        }

        #endregion

        #region Generate map
        IEnumerator GenerateMap()
        {
            var w1 = Mathf.Abs(widthMin) - (int)Mathf.Abs(bornPosition.x);
            var w2 = Mathf.Abs(widthMax) - (int)Mathf.Abs(bornPosition.x);
            var w = w1 > w2 ? w1 : w2;
            w += 1;

            var len1 = Mathf.Abs(lengthMin) - (int)Mathf.Abs(bornPosition.z);
            var len2 = Mathf.Abs(lengthMax) - (int)Mathf.Abs(bornPosition.z);
            var len = len1 > len2 ? len1 : len2;
            len += 1;

            largestDimension = w > len ? w : len;

            // 层节点
            if (layerTrans == null)
            {
                GameObject layerGo = new GameObject();
                layerGo.name = "layer";
                layerGo.transform.parent = gameObject.transform;

                layerTrans = layerGo.transform;
            }

            for (int i = 0; i < largestDimension; i++)
            {
                var points = GetRangePoint(0, i);
                for (int k = 0; k < points.Count; k++)
                {
                    var grid = mapLayers[0].GetGrid(points[k]);
                    if (grid != null)
                    {
                        var prefab = GetAsset<GameObject>(grid.buneldName, grid.assetName);
                        if (prefab != null)
                        {
                            var go = GameObject.Instantiate(prefab) as GameObject;
                            go.transform.position = grid.position;
                            go.name = grid.position.ToString();
                            go.transform.parent = layerTrans;
                        }
                    }
                }
                yield return new WaitForEndOfFrame();
            }
            yield return null;
        }
        
        /// <summary>
        /// 获取范围坐标
        /// </summary>
        /// <param name="layer">层</param>
        /// <param name="lap">第几圈范围</param>
        /// <returns></returns>
        private List<Vector3> GetRangePoint(int layer, int lap)
        {
            List<Vector3> points = new List<Vector3>();
            // 直接返回角色出生点
            if (lap == 0)
            {
                points.Add(bornPosition);
            }
            else {
                Vector3[] fixedPoint = new Vector3[] {
                    new Vector3(bornPosition.x, layer, bornPosition.z + lap),
                    new Vector3(bornPosition.x + lap, layer, bornPosition.z + lap),
                    new Vector3(bornPosition.x + lap, layer, bornPosition.z),
                    new Vector3(bornPosition.x + lap, layer, bornPosition.z -lap),
                    new Vector3(bornPosition.x, layer, bornPosition.z -lap),
                    new Vector3(bornPosition.x - lap, layer, bornPosition.z - lap),
                    new Vector3(bornPosition.x - lap, layer, bornPosition.z),
                    new Vector3(bornPosition.x - lap, layer, bornPosition.z + lap)};

                int pointCount = lap * 8;
                int notFixed = pointCount - 8;
                int nfTimes = notFixed / 8;

                // 1 fixed point
                points.Add(fixedPoint[0]);

                for (int i = 0; i < nfTimes; i++)
                {
                    var p = fixedPoint[0];

                    Vector3 np = Vector3.zero;
                    np.x = p.x + i + 1;
                    np.y = p.y;
                    np.z = p.z;

                    points.Add(np);
                }

                // 2 fixed point
                points.Add(fixedPoint[1]);

                for (int i = 0; i < nfTimes; i++)
                {
                    var p = fixedPoint[1];

                    Vector3 np = Vector3.zero;
                    np.x = p.x;
                    np.y = p.y;
                    np.z = p.z - i - 1;

                    points.Add(np);
                }

                // 3 fixed point
                points.Add(fixedPoint[2]);

                for (int i = 0; i < nfTimes; i++)
                {
                    var p = fixedPoint[2];

                    Vector3 np = Vector3.zero;
                    np.x = p.x;
                    np.y = p.y;
                    np.z = p.z - i - 1;

                    points.Add(np);
                }

                // 4 fixed point
                points.Add(fixedPoint[3]);

                for (int i = 0; i < nfTimes; i++)
                {
                    var p = fixedPoint[3];

                    Vector3 np = Vector3.zero;
                    np.x = p.x - i - 1;
                    np.y = p.y;
                    np.z = p.z;

                    points.Add(np);
                }

                // 5 fixed point
                points.Add(fixedPoint[4]);

                for (int i = 0; i < nfTimes; i++)
                {
                    var p = fixedPoint[4];

                    Vector3 np = Vector3.zero;
                    np.x = p.x - i - 1;
                    np.y = p.y;
                    np.z = p.z;

                    points.Add(np);
                }

                // 6 fixed point
                points.Add(fixedPoint[5]);

                for (int i = 0; i < nfTimes; i++)
                {
                    var p = fixedPoint[5];

                    Vector3 np = Vector3.zero;
                    np.x = p.x;
                    np.y = p.y;
                    np.z = p.z + i + 1;

                    points.Add(np);
                }

                // 7 fixed point
                points.Add(fixedPoint[6]);

                for (int i = 0; i < nfTimes; i++)
                {
                    var p = fixedPoint[6];

                    Vector3 np = Vector3.zero;
                    np.x = p.x;
                    np.y = p.y;
                    np.z = p.z + i + 1;

                    points.Add(np);
                }

                // 8 fixed point
                points.Add(fixedPoint[7]);

                for (int i = 0; i < nfTimes; i++)
                {
                    var p = fixedPoint[7];

                    Vector3 np = Vector3.zero;
                    np.x = p.x + i + 1;
                    np.y = p.y;
                    np.z = p.z ;

                    points.Add(np);
                }
            }

            return points;
        }
        #endregion

        #region Parse Map
        public void ParseMap()
        {
            // 地面 map
            mapLayers.Clear();

            // 出生点坐标
            bornPosition = Vector3.zero;

            SecurityParser parser = new SecurityParser();
            parser.LoadXml(mapDataStr);

            SecurityElement element = parser.ToXml();

            layerMin = int.Parse(element.Attribute("layerMin"));
            layerMax = int.Parse(element.Attribute("layerMax"));
            widthMin = int.Parse(element.Attribute("widthMin"));
            widthMax = int.Parse(element.Attribute("widthMax"));
            lengthMin = int.Parse(element.Attribute("lengthMin"));
            lengthMax = int.Parse(element.Attribute("lengthMax"));
            
            foreach (SecurityElement child in element.Children)
            {
                if (child.Tag == "layer")
                {
                    LayerParse(child);
                }
                else if (child.Tag == "p_bron")
                {
                    bornPosition = new Vector3(int.Parse(child.Attribute("pos_x")), int.Parse(child.Attribute("pos_y")), int.Parse(child.Attribute("pos_z")));
                }
                else if (child.Tag == "bundle")
                {
                    BundleParse(child);
                }
            }
        }

        /// <summary>
        /// 解析 map layer
        /// </summary>
        /// <param name="element"></param>
        private void LayerParse(SecurityElement element)
        {
            MapLayer layerMap = new MapLayer();
            layerMap.layer = int.Parse(element.Attribute("name"));

            foreach (SecurityElement child in element.Children)
            {
                if (child.Tag == "grid")
                {
                    layerMap.AddGrid(GridParse(layerMap.layer, child));
                }
            }

            mapLayers.Add(layerMap.layer, layerMap);
        }

        /// <summary>
        /// 解析 map grid
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        private MapGrid GridParse(int layer, SecurityElement element)
        {
            MapGrid grid = new MapGrid();
            grid.assetName = element.Attribute("asset_name");
            grid.buneldName = element.Attribute("bundle_name");
            grid.layer = layer;
            grid.position = new Vector3(int.Parse(element.Attribute("pos_x")), int.Parse(element.Attribute("pos_y")), int.Parse(element.Attribute("pos_z")));
            grid.rotationAngle = new Vector3(int.Parse(element.Attribute("angle_x")), int.Parse(element.Attribute("angle_y")), int.Parse(element.Attribute("angle_z")));

            return grid;
        }

        /// <summary>
        /// 解析 map asset
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private void BundleParse(SecurityElement element)
        {
            mapAssets.Clear();

            MapAsset asset = new MapAsset(resMgr);
            asset.bundleName = element.Attribute("name");
            mapAssets.Add(asset.bundleName, asset);
        }

        #endregion

        #region Load Res

        #endregion
    }

}

