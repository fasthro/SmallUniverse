using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

namespace SmallUniverse.GameEditor.XmlEditor
{
    [System.Serializable]
    public class XmlToCSVSetting
    {
        public string name;
        public string xmlPath;
        public string csvPath;
        public string codePath;
        public bool merge;

        [HideInInspector]
        public bool e_showing;

        public void Export()
        {
            XmlParser parser = new XmlParser(xmlPath);
            parser.ExportCSV(csvPath);
            parser.ExportCode(codePath, "CSV_" + name);
        }
    }

    [System.Serializable]
    public class XmlToCSVSettingList : ScriptableObject
    {
        public List<XmlToCSVSetting> settings;

        public void Initialize()
        {
            if (settings == null)
            {
                settings = new List<XmlToCSVSetting>();
            }

            Dictionary<string, XmlToCSVSetting> map = new Dictionary<string, XmlToCSVSetting>();
            for (int i = 0; i < settings.Count; i++)
            {
                map.Add(settings[i].xmlPath, settings[i]);
            }

            string rootXmlPath = Path.Combine(Application.dataPath, "Data/Xml");
            string rootCSVPath = Path.Combine(Application.dataPath, "Data/CSV");
            string rootCodePath = Path.Combine(Application.dataPath, "Scripts/Data");

            Dictionary<string, string> fileMap = new Dictionary<string, string>();

            string[] files = Directory.GetFiles(rootXmlPath, "*.xml", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                var file = files[i];
                var fileName = Path.GetFileNameWithoutExtension(file);
                if (!map.ContainsKey(file))
                {
                    XmlToCSVSetting setting = new XmlToCSVSetting();
                    setting.name = fileName;
                    setting.xmlPath = file;
                    setting.csvPath = Path.Combine(rootCSVPath, fileName + ".csv");
                    setting.codePath = Path.Combine(rootCodePath, "CSV_" + fileName + ".cs");
                    setting.merge = true;

                    settings.Add(setting);
                }

                fileMap.Add(file, file);
            }

            List<XmlToCSVSetting> dels = new List<XmlToCSVSetting>();
            for (int i = 0; i < settings.Count; i++)
            {
                if (!fileMap.ContainsKey(settings[i].xmlPath))
                {
                    dels.Add(settings[i]);
                }
            }

            for (int i = 0; i < dels.Count; i++)
            {
                settings.Remove(dels[i]);
            }

            AssetDatabase.SaveAssets();
        }
    }
}