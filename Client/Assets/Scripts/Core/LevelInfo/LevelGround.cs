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
        // 动画开始的格子Id
        private string m_animationStartGridId;
        // 格子集合
        private Dictionary<string, LevelGrid> m_map;
        private Dictionary<string, LevelGrid>.Enumerator m_gridEnumerator;
        // 层
        private int m_layer;
        // 环境引用
        private LevelEnvironment m_environment;

        public void Initialize(LevelArea area, SecurityElement xml)
        {
            this.m_xml = xml;
            this.m_area = area;

            m_layer = LayerMask.NameToLayer(LevelFunctionType.Ground.ToString());
            gameObject.layer = m_layer;

            // 动画开始格子id
            m_animationStartGridId = m_xml.Attribute("animation_start_id");

            m_map = m_map = new Dictionary<string, LevelGrid>();

            foreach (SecurityElement xmlChild in m_xml.Children)
            {
                GameObject go = new GameObject();
                go.name = "gird";
                go.transform.parent = transform;
                go.layer = m_layer;
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
                grid.layer = m_layer;
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
        /// 初始化环境
        /// </summary>
        /// <param name="environment">环境</param>
        public void InitEnvironment(LevelEnvironment environment)
        {
            m_environment = environment;

            // 加载格子
            using (m_gridEnumerator = m_map.GetEnumerator())
            {
                while (m_gridEnumerator.MoveNext())
                {
                    var isShow = !environment.animationEnabled;
                    m_gridEnumerator.Current.Value.LoadAsset(isShow);
                }
            }

            m_area.OnLoadedArea();

            if(environment.animationEnabled)
            {
                PlayAnimation();
            }
        }    

        #region Grid Animation

        /// <summary>
        /// 播放动画
        /// </summary>
        public void PlayAnimation()
        {
            var startGrid = GetGrid(m_animationStartGridId);
            if(startGrid != null)
                SetGridAnimation(startGrid, m_environment.animationWaitTime, m_environment.animationTotalTime, m_environment.animationOffset, Game.gameCurve.levelGridCurve);
        }

        /// <summary>
        /// 设置格子动画
        /// </summary>
        /// <param name="startGrid">动画开始的格子</param>
        private void SetGridAnimation(LevelGrid startGrid, float waitTime, float totalTime, Vector3 offset, AnimationCurve curve)
        {
            startGrid.animation.Initialize(0, totalTime, offset, curve);

            List<LevelGrid> list = new List<LevelGrid>();
            list.Add(startGrid);
            SetGridAnimation(list, 1, waitTime, totalTime, offset, curve);
        }

        private void SetGridAnimation(List<LevelGrid> list, int index, float waitTime, float totalTime, Vector3 offset, AnimationCurve curve)
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
                                ag.animation.Initialize(waitTime * index, totalTime, offset, curve);
                                arounds.Add(ag);
                            }
                        }
                    }
                }
                SetGridAnimation(arounds, index + 1, waitTime, totalTime, offset, curve);
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
        
        /// <summary>
        /// 获取格子
        /// </summary>
        /// <param name="position"></param>
        public LevelGrid GetGrid(Vector3 position)
        {
            string id = LevelGrid.GetId(position);
            LevelGrid grid = null;
            if(m_map.TryGetValue(id, out grid))
            {
                return grid;
            }
            return null;
        }
    }
}

