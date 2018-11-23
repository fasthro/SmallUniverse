using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmallUniverse.Behaviour;
using System.Security;
using UnityEngine.AI;

namespace SmallUniverse
{
    public class LevelGround : MonoBehaviour
    {
        // 区域
        private LevelArea m_area;
        // xml 数据
        private SecurityElement m_xml;
        // 格子集合
        private Dictionary<string, LevelGrid> m_map;
        private Dictionary<string, LevelGrid>.Enumerator m_gridEnumerator;

        private int layer;

        public void Initialize(LevelArea area, SecurityElement xml)
        {
            this.m_xml = xml;
            this.m_area = area;

            layer = LayerMask.NameToLayer(LevelFunctionType.Ground.ToString());
            gameObject.layer = layer;

            m_map = m_map = new Dictionary<string, LevelGrid>();

            foreach (SecurityElement xmlChild in m_xml.Children)
            {
                GameObject go = new GameObject();
                go.name = "gird";
                go.transform.parent = transform;
                go.layer = layer;
                var grid = go.AddComponent<LevelGrid>();
                grid.assetName = xmlChild.Attribute("asset_name");
                grid.bundleName = xmlChild.Attribute("bundle_name");
                grid.position = new Vector3(int.Parse(xmlChild.Attribute("pos_x")), int.Parse(xmlChild.Attribute("pos_y")), int.Parse(xmlChild.Attribute("pos_z")));
                grid.rotationAngle = new Vector3(int.Parse(xmlChild.Attribute("angle_x")), int.Parse(xmlChild.Attribute("angle_y")), int.Parse(xmlChild.Attribute("angle_z")));
                grid.function = LevelFunctionType.Ground;
                grid.adjacentPx = xmlChild.Attribute("adjacent_px");
                grid.adjacentNx = xmlChild.Attribute("adjacent_nx");
                grid.adjacentPz = xmlChild.Attribute("adjacent_pz");
                grid.adjacentNz = xmlChild.Attribute("adjacent_nz");
                grid.layer = layer;
                grid.Initialize();

                AddGrid(grid);
            }
        }

        /// <summary>
        /// 添加格子
        /// </summary>
        /// <param name="grid"></param>
        private void AddGrid(LevelGrid grid)
        {
            if (!m_map.ContainsKey(grid.id))
            {
                m_map.Add(grid.id, grid);
            }
        }

        /// <summary>
        /// 获取格子
        /// </summary>
        /// <param name="id">格子id</param>
        public LevelGrid GetGrid(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            LevelGrid grid = null;
            if (m_map.TryGetValue(id, out grid))
            {
                return grid;
            }
            return null;
        }

        /// <summary>
        /// 加载格子
        /// </summary>
        public void LoadGrid(LevelEnvironment environment)
        {
            using (m_gridEnumerator = m_map.GetEnumerator())
            {
                while (m_gridEnumerator.MoveNext())
                {
                    m_gridEnumerator.Current.Value.LoadAsset();
                }
            }

            var sg = GetGrid(LevelGrid.GetId(new Vector3(-3, 0, -3)));
            if (sg != null)
            {
                SetGridAnimation(sg, 0.05f, 0.5f, 1f, Game.gameCurve.levelGridCurve);
            }
        }

        #region Grid Animation

        /// <summary>
        /// 设置格子动画
        /// </summary>
        /// <param name="startGrid">动画开始的格子</param>
        private void SetGridAnimation(LevelGrid startGrid, float waitTime, float totalTime, float maxDistance, AnimationCurve curve)
        {
            startGrid.animation.Initialize(0, totalTime, maxDistance, curve);

            List<LevelGrid> list = new List<LevelGrid>();
            list.Add(startGrid);

            SetGridAnimation(list, 1, waitTime, totalTime, maxDistance, curve);
        }

        private void SetGridAnimation(List<LevelGrid> list, int index, float waitTime, float totalTime, float maxDistance, AnimationCurve curve)
        {
            if (list.Count > 0)
            {
                Dictionary<string, bool> ids = new Dictionary<string, bool>();
                List<LevelGrid> arounds = new List<LevelGrid>();

                for (int i = 0; i < list.Count; i++)
                {
                    LevelGrid[] signArounds = GetAroundGrids(list[i]);
                    for (int k = 0; k < signArounds.Length; k++)
                    {
                        LevelGrid ag = signArounds[k];
                        if (ag != null && !ag.animation.ready)
                        {
                            if (!ids.ContainsKey(ag.id))
                            {
                                ids.Add(ag.id, true);
                                ag.animation.Initialize(waitTime * index, totalTime, maxDistance, curve);
                                arounds.Add(ag);
                            }
                        }
                    }
                }
                Debug.Log(index);
                SetGridAnimation(arounds, index + 1, waitTime, totalTime, maxDistance, curve);
            }
        }

        /// <summary>
        /// 获取目标格子周围的格子
        /// </summary>
        /// <param name="startGrid">动画开始的格子</param>
        private LevelGrid[] GetAroundGrids(LevelGrid tg)
        {
            LevelGrid[] arounds = new LevelGrid[4];
            arounds[0] = GetGrid(tg.adjacentPx);
            arounds[1] = GetGrid(tg.adjacentNx);
            arounds[2] = GetGrid(tg.adjacentPz);
            arounds[3] = GetGrid(tg.adjacentNz);

            return arounds;
        }
        #endregion
    }
}

