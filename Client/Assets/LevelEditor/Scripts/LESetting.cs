using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SU.Editor.LevelEditor
{
    public class LESetting
    {
        // 关卡场景保存路径
        public static string LevelSceneDirectory = "Levels/Scenes/";
        // 资源库路径
        public static string RepositoryDirectory = "Levels/Repository/";

        // 场景工具初始 x,y 位置
        public static int SceneTooIX = 10;
        public static int SceneTooIY = 10;
        // 场景工具 间隔
        public static int SceneTooIInterval = 5;
        // 场景工具尺寸
        public static int SceneToolSize = 32;
    }
}
