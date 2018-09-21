using FairyGUI.Utils;
using System.Collections.Generic;
using System.IO;

namespace SU.Editor.ExportPanelCode
{
    public class PackageInfo
    {
        // 包id
        public string id;
        // 包路径，非包文件路径
        public string path;
        // 包对应的xml
        public XML xml;
        // 包内资源<id, path> 字典
        public Dictionary<string, string> srcDic = new Dictionary<string, string>();

        public PackageInfo(string _path)
        {
            path = _path;

            var pkgPath = _path + "/package.xml";

            xml = new XML(File.OpenText(pkgPath).ReadToEnd());
            id = xml.GetAttribute("id");

            var items = xml.Elements();
            foreach (XML item in items)
            {
                if (item.name == "resources")
                {
                    var pchilds = item.Elements();
                    foreach (XML child in pchilds)
                    {
                        srcDic.Add(child.GetAttribute("id"), path + child.GetAttribute("path") + child.GetAttribute("name"));
                    }
                }
            }
        }

        // 获取包内资源路径
        public string GetSrcPath(string src)
        {
            string p = "";
            if (srcDic.TryGetValue(src, out p))
            {
                return p;
            }
            return "";
        }
    }
}