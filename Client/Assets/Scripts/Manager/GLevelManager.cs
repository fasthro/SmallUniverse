using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SU.Manager
{
    public class GLevelManager : BaseManager
    {
        // 关卡
        public LevelInfo levelInfo;
        // 角色控制器
        public CharacterControler characterContorler;
        // 相机控制器
        public CameraControler cameraControler;

        public override void Initialize()
        {
        }

        public override void OnFixedUpdate(float deltaTime)
        {
        }

        public override void OnUpdate(float deltaTime)
        {
        }

        public override void OnDispose()
        {
        }

        public void InitLevel(string _levelName, string _characterName)
        {
            levelInfo = LevelInfo.Create(_levelName);
            characterContorler = CharacterControler.Create(_characterName);
            cameraControler = CameraControler.Create();
        }
    }
}

