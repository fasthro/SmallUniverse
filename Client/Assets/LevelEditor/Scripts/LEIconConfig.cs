using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse.GameEditor.LevelEditor
{
    [System.Serializable]
    public class LEIcon
    {
        public string Name;
        public Texture2D Texture;
    }

    [System.Serializable]
    public class LEIconConfig : ScriptableObject
    {
        public List<LEIcon> icons = new List<LEIcon>();

        public Texture2D GetIconTexture(string name)
        {
            for (int i = 0; i < icons.Count; i++)
            {
                if (icons[i].Name.Equals(name))
                {
                    return icons[i].Texture;
                }
            }
            return null;
        }
    }
}
