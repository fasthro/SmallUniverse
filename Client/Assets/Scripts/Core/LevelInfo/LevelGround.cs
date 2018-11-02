using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmallUniverse.Behaviour;
using System.Security;

namespace SmallUniverse
{
    public class LevelGround : MonoBehaviour {
        // 区域
        private LevelArea area;
        // xml 数据
        private SecurityElement xml;
        // 格子集合
        private List<LevelGrid> grids;
        
        public void Initialize(LevelArea _area, SecurityElement _xml)
        {
            xml = _xml;
            area = _area;
            grids = new List<LevelGrid>();

            foreach (SecurityElement xmlChild in xml.Children)
            {
                GameObject go = new GameObject();
                go.name = "gird";
                go.transform.parent = transform;
                var grid = go.AddComponent<LevelGrid>();
                grid.assetName = xmlChild.Attribute("asset_name");
                grid.bundleName = xmlChild.Attribute("bundle_name");
                grid.position = new Vector3(int.Parse(xmlChild.Attribute("pos_x")), int.Parse(xmlChild.Attribute("pos_y")), int.Parse(xmlChild.Attribute("pos_z")));
                grid.rotationAngle = new Vector3(int.Parse(xmlChild.Attribute("angle_x")), int.Parse(xmlChild.Attribute("angle_y")), int.Parse(xmlChild.Attribute("angle_z")));
                grid.function = LevelFunctionType.Ground;

                grids.Add(grid);
            }
        }
        
        /// <summary>
        /// 加载格子
        /// </summary>
        /// <param name="aniType">动画类型</param>
        public void LoadGrid(LevelAnimationType aniType)
        {
            for (int i = 0; i < grids.Count; i++)
            {
                grids[i].LoadAsset();
            }
        }
    }
}

