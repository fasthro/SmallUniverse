using FairyGUI.Utils;
using System.IO;

namespace SmallUniverse.GameEditor.ExportPanelCode
{
    public class Component
    {
        // 当前面板
        private Panel panel;
        // 当前组件岁对应的数据
        private XML xml;
        // 当前组件的索引路径
        private string[] path;
        // 组件类型
        private ComponentType ctype;

        public Component(Panel _panel, string[] _path, XML _xml)
        {
            xml = _xml;
            path = _path;
            panel = _panel;
        }

        public void ExportScript()
        {
            Export(xml, path);
        }

        private void Export(XML dataXml, string[] _path)
        {
            XMLList childrens = dataXml.Elements();
            foreach (XML child in childrens)
            {
                var typeName = child.name;
                var attrbuteName = child.GetAttribute("name");

                if (typeName == "controller")
                {
                    panel.AddComponentScript(new ComponentScript(ComponentType.Controller, attrbuteName, _path));
                }
                else if (typeName == "transition")
                {
                    panel.AddComponentScript(new ComponentScript(ComponentType.Transition, attrbuteName, _path));
                }
                else if (typeName == "displayList")
                {
                    ExportDisplay(child, _path);
                }
            }
        }

        private void ExportDisplay(XML dataXml, string[] _path)
        {
            XMLList childrens = dataXml.Elements();
            foreach (XML child in childrens)
            {
                var typeName = child.name;
                var attrbuteName = child.GetAttribute("name");

                if (typeName == "text")
                {
                    if (child.GetAttributeBool("input"))
                    {
                        panel.AddComponentScript(new ComponentScript(ComponentType.TextInput, attrbuteName, _path));
                    }
                    else
                    {
                        panel.AddComponentScript(new ComponentScript(ComponentType.Text, attrbuteName, _path));
                    }
                }
                else if (typeName == "richtext")
                {
                    panel.AddComponentScript(new ComponentScript(ComponentType.Richtext, attrbuteName, _path));
                }
                else if (typeName == "loader")
                {
                    panel.AddComponentScript(new ComponentScript(ComponentType.Loader, attrbuteName, _path));
                }
                else if (typeName == "image")
                {
                    panel.AddComponentScript(new ComponentScript(ComponentType.Image, attrbuteName, _path));
                }
                else if (typeName == "graph")
                {
                    panel.AddComponentScript(new ComponentScript(ComponentType.Graph, attrbuteName, _path));
                }
                else if (typeName == "list")
                {
                    panel.AddComponentScript(new ComponentScript(ComponentType.List, attrbuteName, _path));
                }
                else if (typeName == "group")
                {
                    panel.AddComponentScript(new ComponentScript(ComponentType.Group, attrbuteName, _path));
                }
                else if (typeName == "component")
                {
                    ExportComponent(child, _path);
                }
            }
        }

        private void ExportComponent(XML dataXml, string[] _path)
        {
            var attrbuteName = dataXml.GetAttribute("name");

            var src = dataXml.GetAttribute("src");
            var pkg = dataXml.GetAttribute("pkg");
            var xp = panel.GetXmlPathById(pkg, src);

            XML nxml = new XML(File.OpenText(xp).ReadToEnd());
            var extention = nxml.GetAttribute("extention");
            if (!string.IsNullOrEmpty(extention))
            {
                if (extention == "Button")
                {
                    panel.AddComponentScript(new ComponentScript(ComponentType.Button, attrbuteName, _path));
                }
                else if (extention == "Slider")
                {
                    panel.AddComponentScript(new ComponentScript(ComponentType.Slider, attrbuteName, _path));
                }
                else if (extention == "ProgressBar")
                {
                    panel.AddComponentScript(new ComponentScript(ComponentType.ProgressBar, attrbuteName, _path));
                }
                else if (extention == "ComboBox")
                {
                    panel.AddComponentScript(new ComponentScript(ComponentType.ComboBox, attrbuteName, _path));
                }
            }
            else
            {
                // Component 类型
                panel.AddComponentScript(new ComponentScript(ComponentType.Component, attrbuteName, _path));
            }

            // 导出 src xml
            var childPath = GetChildPath(dataXml, _path);
            Component ncp = new Component(panel, childPath, nxml);
            ncp.ExportScript();
        }

        // 获取子组件路径
        private string[] GetChildPath(XML cDataXml, string[] _path)
        {
            var pl = _path.Length;
            string[] childPath = new string[pl + 1];
            for (int i = 0; i < pl; i++)
            {
                childPath[i] = path[i];
            }
            childPath[childPath.Length - 1] = cDataXml.GetAttribute("name");
            return childPath;
        }
    }
}
