using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using SU.Behaviour;

public class LevelArea : GameBehaviour
{
    // 关卡信息
    private LevelInfo levelInfo;
    // xml 数据
    private SecurityElement xml;

    // 区域索引
    public int index;
    // 地面
    public LevelGround ground;


    protected override void OnAwake()
    {
        base.OnAwake();
    }

    protected override void OnStart()
    {
        base.OnStart();
    }

    public void Initialize(LevelInfo _levelInfo, int _index, SecurityElement _xml)
    {
        xml = _xml;
        index = _index;
        levelInfo = _levelInfo;

        Transform root = null;

        foreach (SecurityElement xmlChild in xml.Children)
        {
            if (xmlChild.Tag == LevelFunctionType.Ground.ToString().ToLower())
            {
                root = CreateRoot(transform, LevelFunctionType.Ground.ToString());
                ground = root.gameObject.AddComponent<LevelGround>();
                ground.Initialize(this, xmlChild);
            }
            else if (xmlChild.Tag == LevelFunctionType.Decoration.ToString().ToLower())
            {

            }
            else if (xmlChild.Tag == LevelFunctionType.Door.ToString().ToLower())
            {

            }
            else if (xmlChild.Tag == LevelFunctionType.Trap.ToString().ToLower())
            {

            }
            else if (xmlChild.Tag == LevelFunctionType.Transfer.ToString().ToLower())
            {

            }
            else if (xmlChild.Tag == LevelFunctionType.Player.ToString().ToLower())
            {

            }
            else if (xmlChild.Tag == LevelFunctionType.Monster.ToString().ToLower())
            {

            }
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
}
