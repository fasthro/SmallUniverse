using SU.Behaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game {
    
    // 游戏管理对象
    private static GameObject _manager = null;
    public static GameObject manager {
        get {
            if (_manager == null)
            {
                _manager = GameObject.FindWithTag("MainGame");
            }
            return _manager;
        }
    }

    // main game
    private static MainGame _mainGame = null;
    public static MainGame mainGame {
        get {
            if (_mainGame == null)
            {
                _mainGame = manager.GetComponent<MainGame>();
            }
            return _mainGame;
        }
    }

    // 获取Scene
    public static T GetScene<T>() where T : class
    {
        return BaseBehaviour.GetScene<T>();
    }

    // 获取manager
    public static T GetManager<T>() where T : class
    {
        return BaseBehaviour.GetManager<T>();
    }
}
