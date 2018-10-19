using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SU.Behaviour;

public class LevelGrid : GameBehaviour
{
    // 资源名称
    public string assetName;
    // bundle 名称
    public string buneldName;
    // 位置
    public Vector3 position;
    // 旋转
    public Vector3 rotationAngle;
    // 功能
    public LevelFunctionType function;

    protected override void OnAwake()
    {
        base.OnAwake();
    }

    protected override void OnStart()
    {
        base.OnStart();
    }

    public void Initialize()
    {
        
    }

}
