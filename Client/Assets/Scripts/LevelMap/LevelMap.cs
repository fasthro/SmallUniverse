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

        // 地面格子<layer, MapLayer>
        private Dictionary<int, MapLayer> ground;

        private AssetBundle normalBundle;

        protected override void OnAwake()
        {
            base.OnAwake();

            resMgr = Game.GetManager<ResManager>();
            ground = new Dictionary<int, MapLayer>();
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
            var bundle = resMgr.LoadAssetBundle(mapName.ToLower());
            var textAsset = bundle.LoadAsset(mapName + ".xml") as TextAsset;
            mapDataStr = textAsset.text;

            // 解析配置
            ParseMap();

            normalBundle = resMgr.LoadAssetBundle("normal");
            // 生成地图
            StartCoroutine(GenerateMap());
        }

        #region Generate map
       IEnumerator GenerateMap()
        {
            for (int i = 0; i < 10; i++)
            {
                var points = GetRangePoint(0, i);
                for (int k = 0; k < points.Count; k++)
                {
                    var grid = ground[0].GetGrid(points[k]);
                    if (grid != null)
                    {
                        var prefab = normalBundle.LoadAsset("Cube1") as GameObject;
                        var go = GameObject.Instantiate(prefab) as GameObject;
                        go.transform.position = grid.position;
                        go.name = grid.position.ToString();
                    }
                }
                yield return new WaitForSeconds(0.2f);
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
            ground.Clear();

            // 出生点坐标
            bornPosition = Vector3.zero;

            SecurityParser parser = new SecurityParser();
            parser.LoadXml(mapDataStr);

            SecurityElement element = parser.ToXml();

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

            ground.Add(layerMap.layer, layerMap);
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
            grid.path = element.Attribute("path");
            grid.layer = layer;
            grid.position = new Vector3(int.Parse(element.Attribute("pos_x")), int.Parse(element.Attribute("pos_y")), int.Parse(element.Attribute("pos_z")));
            grid.rotationAngle = new Vector3(int.Parse(element.Attribute("angle_x")), int.Parse(element.Attribute("angle_y")), int.Parse(element.Attribute("angle_z")));

            return grid;
        }

        #endregion

        #region Load Res

        #endregion
    }

}

