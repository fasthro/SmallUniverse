using Mono.Xml;
using SU.Behaviour;
using SU.Manager;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public enum LevelFunctionType
{
    Ground,              // 地面
    Player,                // 玩家
    Monster,            // 怪
    Door,                 // 门
    Trap,                  // 陷阱
    Transfer,            // 传送门
    Decoration,       // 装饰品
}

public enum LevelAnimationType
{
    None,               // 无
}

public class LevelInfo : GameBehaviour
{
    // 关卡名称
    private string levelName;
    // xml 数据
    private SecurityElement xml;
    // 区域数量
    private int areaCount;
    // 区域列表
    private LevelArea[] areas;
    
    protected override void OnAwake()
    {
        base.OnAwake();
    }

    protected override void OnStart()
    {
        base.OnStart();
    }

    /// <summary>
    /// 创建关卡信息
    /// </summary>
    /// <param name="parent"></param>
    public static LevelInfo Create(string _levelName)
    {
        GameObject go = new GameObject();
        go.name = "LevelInfo";
        var levelInfo = go.AddComponent<LevelInfo>();
        levelInfo.Initialize(_levelName);
        return levelInfo;
    }

    private void Initialize(string _levelName)
    {
        levelName = _levelName;
        xml = LoadXml();

        areaCount = int.Parse(xml.Attribute("area_count"));
        areas = new LevelArea[areaCount];

        // 初始化关卡区域
        foreach (SecurityElement xmlChild in xml.Children)
        {
            var areaIndex = int.Parse(xmlChild.Attribute("index"));
            var areaTrans = CreateRoot(transform, "Area_" + areaIndex);
            var area = areaTrans.gameObject.AddComponent<LevelArea>();
            area.Initialize(this, areaIndex, xmlChild);
            areas[areaIndex - 1] = area;
        }
    }

    public void InitEnvironment(LevelEnvironment _environment)
    {
        for(int i = 0; i < areas.Length; i++)
        {
            areas[i].InitEnvironment(_environment);
        }
    }
    
    /// <summary>
    /// 创建节点
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    private Transform CreateRoot(Transform parent, string name)
    {
        GameObject go = new GameObject();
        go.name = name;
        go.transform.parent = parent;
        return go.transform;
    }

    /// <summary>
    ///  加载场景配置
    /// </summary>
    /// <returns></returns>
    private SecurityElement LoadXml()
    {
        // 加载配置
        var resMgr = Game.GetManager<GResManager>();
        var bundleName = "level/scene/" + levelName.ToLower();
        var bundle = resMgr.LoadAssetBundle(bundleName);
        var textAsset = bundle.LoadAsset(levelName + ".xml") as TextAsset;

        SecurityParser parser = new SecurityParser();
        parser.LoadXml(textAsset.text);

        resMgr.UnLoadAssetBundle(bundleName);

        return parser.ToXml();
    }
}

