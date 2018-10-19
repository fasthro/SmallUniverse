using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SU.Behaviour;
using System.Security;

public class LevelGround : GameBehaviour {

    // 区域
    private LevelArea area;
    // xml 数据
    private SecurityElement xml;
    // 格子集合
    private List<LevelGrid> grids;

    protected override void OnAwake()
    {
        base.OnAwake();
    }

    protected override void OnStart()
    {
        base.OnStart();
    }

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
    /// 初始格子
    /// </summary>
    /// <param name="aniType">动画类型</param>
    public void InitGrid(LevelAnimationType aniType)
    {
        for (int i = 0; i < grids.Count; i++)
        {
            grids[i].LoadAsset();
        }
    }
}
