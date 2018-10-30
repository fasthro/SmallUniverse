using FairyGUI.Utils;
using SmallUniverse.Utils;
using System.Collections.Generic;
using System.IO;

namespace SmallUniverse.Editor.ExportPanelCode
{
    public class Panel
    {
        // 包路径
        private string ppath;
        // 包名称
        private string pname;
        // 面板xml路径
        private string xpath;
        // 面板名称
        private string xname;
        // 包id
        private string pkgId;
        // 工程包对象
        private Package ppkg;
        // 组件对应的代码生成列表
        private List<ComponentScript> scripts = new List<ComponentScript>();

        public Panel(Package _ppkg, string _panelXmlPath)
        {
            ppkg = _ppkg;
            ppath = PathUtils.GetTopPath(_panelXmlPath, 2);
            pname = PathUtils.GetPathSection(_panelXmlPath, -3);
            xpath = _panelXmlPath;
            xname = Path.GetFileNameWithoutExtension(_panelXmlPath);

            var pfile = Path.Combine(ppath, "package.xml");
            var pxml = new XML(File.OpenText(pfile).ReadToEnd());
            pkgId = pxml.GetAttribute("id");
        }

        // 导出面板
        public void ExportPanelScript()
        {
            scripts.Clear();
            var component = new Component(this, new string[] { }, new XML(File.OpenText(xpath).ReadToEnd()));
            component.ExportScript();
        }

        // 添加 component script to list
        public void AddComponentScript(ComponentScript script)
        {
            scripts.Add(script);
        }

        // 获取到面板代码列表
        public List<ComponentScript> GetComponentScripts()
        {
            return scripts;
        }

        // 获取View代码类名称
        public string GetScriptViewName()
        {
            var ns = xname.Split('_');
            return StrUtils.ToFirstUpper(ns[0]) + StrUtils.ToFirstUpper(ns[1]) + "View";
        }

        // 获取 Panel 代码类名称
        public string GetScriptPanelName()
        {
            var ns = xname.Split('_');
            return StrUtils.ToFirstUpper(ns[0]) + StrUtils.ToFirstUpper(ns[1]);
        }

        // 获取主包名称
        public string GetMainPackage()
        {
            return string.Format("\"{0}\"", pname);
        }

        // 获取面板名称
        public string GetComponentName()
        {
            return string.Format("\"{0}\"", pname);
        }


        /// <summary>
        /// 通过资源ID获取包内资源路径
        /// </summary>
        /// <param name="pid"> 包id,如果为空就是当前包 </param>
        /// <param name="src"> 资源id </param>
        /// <returns></returns>
        public string GetXmlPathById(string pid, string src)
        {
            if (string.IsNullOrEmpty(pid))
            {
                pid = pkgId;
            }
            return ppkg.GetSrcPath(pid, src);
        }
    }
}
