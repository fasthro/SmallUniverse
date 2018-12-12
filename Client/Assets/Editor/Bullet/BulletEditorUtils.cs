using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SmallUniverse.GameEditor
{
    public class BulletEditorUtils
    {
        public static void SetAssetBackgroundColor(string path)
        {
            var errorCode = CheckFileExists(path);
            if (errorCode == -1 || errorCode == 1)
            {
                GUI.backgroundColor = Color.green;
            }
            else
            {
                GUI.backgroundColor = Color.red;
            }
        }

        public static int CheckFileExists(string path)
        {
            if (string.IsNullOrEmpty(path))
                return -1;

            string[] temps = path.Split('/');
            if (temps.Length == 3)
            {
                string fp = Path.Combine(Application.dataPath, "Art/" + temps[0] + "/" + temps[1] + "/Prefabs/" + temps[2] + ".prefab");
                if (File.Exists(fp))
                {
                    return 1;
                }
            }
            return 0;
        }
    }

}
