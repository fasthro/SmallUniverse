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
        private LevelArea m_area;
		private SecurityElement m_xml;
        private LevelGrid m_grid;

 		public void Initialize(LevelArea _area, SecurityElement _xml)
        {
            m_xml = _xml;
            m_area = _area;

            GameObject go = new GameObject();
            go.name = "gird";
            go.transform.parent = transform;
            
            m_grid = go.AddComponent<LevelGrid>();
            m_grid.assetName = m_xml.Attribute("asset_name");
            m_grid.bundleName = m_xml.Attribute("bundle_name");
            m_grid.position = new Vector3(int.Parse(m_xml.Attribute("pos_x")), int.Parse(m_xml.Attribute("pos_y")), int.Parse(m_xml.Attribute("pos_z")));
            m_grid.rotationAngle = new Vector3(int.Parse(m_xml.Attribute("angle_x")), int.Parse(m_xml.Attribute("angle_y")), int.Parse(m_xml.Attribute("angle_z")));
            m_grid.function = LevelFunctionType.Door;
        }
        
        /// <summary>
        /// 加载门
        /// </summary>
        public void LoadDoor()
        {
            m_grid.LoadAsset();
        }

        /// <summary>
        /// 开门
        /// </summary>
        public void SetState(LevelDoorState state)
        {
            if(state == LevelDoorState.Open)
            {
                m_grid.gameObject.SetActive(false);
            }
            else if(state == LevelDoorState.Close)
            {
                m_grid.gameObject.SetActive(true);
            }
        }
    }
}
