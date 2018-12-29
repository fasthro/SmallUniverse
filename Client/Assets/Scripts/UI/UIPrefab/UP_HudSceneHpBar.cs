using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

namespace SmallUniverse
{
    public class UP_HudSceneHpBar : UIPrefabBase
    {
        public GProgressBar bar;

        public UP_HudSceneHpBar()
        {
            m_pool = true;
            Create("hud_scene_hp");
        }

        protected override void OnInitialize()
        {
            bar = ui.GetChild("@bar").asProgress;
        }

        public void SetValue(float value, float max)
        {
            bar.max = max;
            bar.value = value;
        }
    }
}
