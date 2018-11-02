using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse.GameEditor.LevelEditor
{
    public class LESetting
    {
        // 关卡场景保存路径
        public static string LevelSceneDirectory = "Levels/Scenes/";
        // Prefab 路径
        public static string PrefabDirectory = "Levels/Prefabs/";

        // 场景工具初始 x,y 位置
        public static int SceneTooIX = 10;
        public static int SceneTooIY = 10;
        // 场景工具 间隔
        public static int SceneTooIInterval = 5;
        // 场景工具尺寸
        public static int SceneToolSize = 40;
    }
}
