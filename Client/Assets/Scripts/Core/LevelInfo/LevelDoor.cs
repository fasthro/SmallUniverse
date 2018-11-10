using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.AI;

namespace SmallUniverse
{
    public class LevelDoor : MonoBehaviour
    {
		// 区域
        private LevelArea area;
		private SecurityElement xml;
        private LevelGrid grid;
        
        // 障碍物
        private NavMeshObstacle mavmeshObstacle;
 		public void Initialize(LevelArea _area, SecurityElement _xml)
        {
            xml = _xml;
            area = _area;

            GameObject go = new GameObject();
            go.name = "gird";
            go.transform.parent = transform;
            
            grid = go.AddComponent<LevelGrid>();
            grid.assetName = xml.Attribute("asset_name");
            grid.bundleName = xml.Attribute("bundle_name");
            grid.position = new Vector3(int.Parse(xml.Attribute("pos_x")), int.Parse(xml.Attribute("pos_y")), int.Parse(xml.Attribute("pos_z")));
            grid.rotationAngle = new Vector3(int.Parse(xml.Attribute("angle_x")), int.Parse(xml.Attribute("angle_y")), int.Parse(xml.Attribute("angle_z")));
            grid.function = LevelFunctionType.Door;
        }
        
        /// <summary>
        /// 加载门
        /// </summary>
        public void LoadDoor()
        {
            grid.LoadAsset();
        }

        /// <summary>
        /// 开门
        /// </summary>
        public void Open()
        {
            
        }

        /// <summary>
        /// 关门
        /// </summary>
        public void Close()
        {
            
        }
    }
}
