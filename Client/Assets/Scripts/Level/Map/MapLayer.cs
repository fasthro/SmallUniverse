using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SU.Level
{
    public class MapLayer
    {
        // 层
        public int layer;
        // 本层的格子<GetGridKey(vector3)，MapGrid>
        private Dictionary<string, MapGrid> gridMap;

        public MapLayer()
        {
            gridMap = new Dictionary<string, MapGrid>();
        }

        /// <summary>
        /// 添加格子
        /// </summary>
        /// <param name="grid"></param>
        public void AddGrid(MapGrid grid)
        {
            var key = GetGridKey(grid.position);
            if (!gridMap.ContainsKey(key))
            {
                gridMap.Add(key, grid);
            }
            else {
                gridMap[key] = grid;
            }
        }

        /// <summary>
        /// 获取本层格子
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public MapGrid GetGrid(Vector3 position)
        {
            var key = GetGridKey(position);

            MapGrid grid = null;
            if (gridMap.TryGetValue(key, out grid))
            {
                return grid;
            }
            return null;
        }

        /// <summary>
        /// 通过格子位置返回格子所在字典中的key
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        static string GetGridKey(Vector3 position)
        {
            return string.Format("({0},{1},{2})", position.x, position.y, position.z);
        }
    }
}
