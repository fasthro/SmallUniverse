using UnityEngine;

namespace SmallUniverse.Editor.ExportPanelCode
{
    public class ComponentScript
    {
        // 组件类型
        private ComponentType componentType;
        // 组件名称
        private string cname;
        // 属性名称
        private string attrbuteName;
        // 组将路径
        private string[] path;

        // 是否导出代码
        public bool isExport
        {
            get
            {
                return cname.StartsWith("@");
            }
        }

        public ComponentScript(ComponentType _componentType, string _cname, string[] _path)
        {
            componentType = _componentType;
            cname = _cname;

            if (isExport)
            {
                attrbuteName = cname.Substring(1);
            }

            path = _path;
        }

        #region c#

        // 返回c#定义代码
        public string GetVarCodeWithCSharp()
        {
            return string.Format("public {0} {1};", GetTypeStr(), GetDefineAttrbuteName());
        }

        // 返回c#获取代码
        public string GetGetCodeWithCSharp()
        {
            string defineName = GetDefineAttrbuteName();
            string content = "";
            if (path.Length == 0)
            {
                if (componentType == ComponentType.Controller)
                {
                    content = string.Format("{0} = root.GetController(\"{1}\")", defineName, cname);
                }
                else if (componentType == ComponentType.Transition)
                {
                    content = string.Format("{0} = root.GetTransition(\"{1}\")", defineName, cname);
                }
                else
                {
                    content = string.Format("{0} = root.GetChild(\"{1}\")", defineName, cname);
                }
            }
            else
            {
                content = "root";
                for (int i = 0; i < path.Length; i++)
                {
                    content = string.Format("{0}.GetChild(\"{1}\").asCom", content, path[i]);
                }

                if (componentType == ComponentType.Controller)
                {
                    content = string.Format("{0} = {1}.GetController(\"{2}\")", defineName, content, cname);
                }
                else if (componentType == ComponentType.Transition)
                {
                    content = string.Format("{0} = {1}.GetTransition(\"{2}\")", defineName, content, cname);
                }
                else
                {
                    content = string.Format("{0} = {1}.GetChild(\"{2}\")", defineName, content, cname);
                }
            }

            // type as
            var typeAs = GetAsStr();

            if (string.IsNullOrEmpty(typeAs))
            {
                return string.Format("{0};", content);
            }
            return string.Format("{0}.{1};", content, typeAs);
        }

        // c# 初始化代码
        public string GetInitCodeWithCSharp()
        {
            var str = GetInitStrCSharp();
            if (!string.IsNullOrEmpty(str))
            {
                return string.Format("{0}{1};", GetDefineAttrbuteName(), str);
            }
            return "";
        }

        // c# 销毁代码
        public string GetDisposeCodeWithCSharp()
        {
            return string.Format("{0} = null;", GetDefineAttrbuteName());
        }
        #endregion

        #region lua
        // 返回 lua 定义代码
        public string GetDefineCodeWithLua()
        {
            return string.Format("local {0} = nil", GetDefineAttrbuteName());
        }

        // 返回 lua 获取代码
        public string GetAcquireCodeWithLua()
        {
            string defineName = GetDefineAttrbuteName();
            string content = "";
            if (path.Length == 0)
            {
                if (componentType == ComponentType.Controller)
                {
                    content = string.Format("{0} = root:GetController(\"{1}\")", defineName, cname);
                }
                else if (componentType == ComponentType.Transition)
                {
                    content = string.Format("{0} = root:GetTransition(\"{1}\")", defineName, cname);
                }
                else
                {
                    content = string.Format("{0} = root:GetChild(\"{1}\")", defineName, cname);
                }
            }
            else
            {
                content = "root";
                for (int i = 0; i < path.Length; i++)
                {
                    content = string.Format("{0}:GetChild(\"{1}\").asCom", content, path[i]);
                }

                if (componentType == ComponentType.Controller)
                {
                    content = string.Format("{0} = {1}:GetController(\"{2}\")", defineName, content, cname);
                }
                else if (componentType == ComponentType.Transition)
                {
                    content = string.Format("{0} = {1}:GetTransition(\"{2}\")", defineName, content, cname);
                }
                else
                {
                    content = string.Format("{0} = {1}:GetChild(\"{2}\")", defineName, content, cname);
                }
            }

            // type as
            var typeAs = GetAsStr();

            if (string.IsNullOrEmpty(typeAs))
            {
                return string.Format("{0}", content);
            }
            return string.Format("{0}.{1}", content, typeAs);
        }

        // lua 初始化代码
        public string GetInitCodeWithLua()
        {
            var str = GetInitStrLua();
            if (!string.IsNullOrEmpty(str))
            {
                return string.Format("{0}{1};", GetDefineAttrbuteName(), str);
            }
            return "";
        }

        // lua 销毁代码
        public string GetDisposeCodeWithLua()
        {
            return string.Format("{0} = nil;", GetDefineAttrbuteName());
        }

        #endregion

        // 获取定义的属性名称
        private string GetDefineAttrbuteName()
        {
            if (path.Length == 0)
            {
                return attrbuteName;
            }

            string name = "";
            for (int i = 0; i < path.Length; i++)
            {
                string p = path[i];
                if (path[i].StartsWith("@"))
                {
                    p = path[i].Substring(1);
                }

                if (string.IsNullOrEmpty(name))
                {
                    name = string.Format("{0}_{1}", attrbuteName, p);
                }
                else
                {
                    name = string.Format("{0}_{1}", name, p);
                }
            }
            return name;
        }

        private string GetTypeStr()
        {
            switch (componentType)
            {
                case ComponentType.Component:
                    return "GComponent";
                case ComponentType.Text:
                    return "GTextField";
                case ComponentType.Richtext:
                    return "GRichTextField";
                case ComponentType.TextInput:
                    return "GTextInput";
                case ComponentType.Loader:
                    return "GLoader";
                case ComponentType.Image:
                    return "GImage";
                case ComponentType.Graph:
                    return "GGraph";
                case ComponentType.Button:
                    return "GButton";
                case ComponentType.List:
                    return "GList";
                case ComponentType.Group:
                    return "GGroup";
                case ComponentType.Slider:
                    return "GSlider";
                case ComponentType.ProgressBar:
                    return "GProgressBar";
                case ComponentType.ComboBox:
                    return "GComboBox";
                case ComponentType.Controller:
                    return "Controller";
                case ComponentType.Transition:
                    return "Transition";
                default:
                    return "";
            }
        }

        private string GetAsStr()
        {
            switch (componentType)
            {
                case ComponentType.Component:
                    return "asCom";
                case ComponentType.Text:
                    return "asTextField";
                case ComponentType.Richtext:
                    return "asRichTextField";
                case ComponentType.TextInput:
                    return "asTextInput";
                case ComponentType.Loader:
                    return "asLoader";
                case ComponentType.Image:
                    return "asImage";
                case ComponentType.Graph:
                    return "asGraph";
                case ComponentType.Button:
                    return "asButton";
                case ComponentType.List:
                    return "asList";
                case ComponentType.Group:
                    return "asGroup";
                case ComponentType.Slider:
                    return "asSlider";
                case ComponentType.ProgressBar:
                    return "asProgress";
                case ComponentType.ComboBox:
                    return "asComboBox";
                case ComponentType.Controller:
                    return "";
                case ComponentType.Transition:
                    return "";
                default:
                    return "";
            }
        }

        private string GetInitStrCSharp()
        {
            switch (componentType)
            {
                case ComponentType.Component:
                    return "";
                case ComponentType.Text:
                    return ".text = \"\"";
                case ComponentType.Richtext:
                    return ".text = \"\"";
                case ComponentType.TextInput:
                    return ".text = \"\"";
                case ComponentType.Loader:
                    return ".url = null";
                case ComponentType.Image:
                    return "";
                case ComponentType.Graph:
                    return "";
                case ComponentType.Button:
                    return "";
                case ComponentType.List:
                    return ".RemoveChildrenToPool();";
                case ComponentType.Group:
                    return "";
                case ComponentType.Slider:
                    return "";
                case ComponentType.ProgressBar:
                    return "";
                case ComponentType.ComboBox:
                    return "";
                case ComponentType.Controller:
                    return ".SetSelectedIndex(0)";
                case ComponentType.Transition:
                    return "";
                default:
                    return "";
            }
        }

        private string GetInitStrLua()
        {
            switch (componentType)
            {
                case ComponentType.Component:
                    return "";
                case ComponentType.Text:
                    return ".text = \"\"";
                case ComponentType.Richtext:
                    return ".text = \"\"";
                case ComponentType.TextInput:
                    return ".text = \"\"";
                case ComponentType.Loader:
                    return ".url = nil";
                case ComponentType.Image:
                    return "";
                case ComponentType.Graph:
                    return "";
                case ComponentType.Button:
                    return "";
                case ComponentType.List:
                    return ":RemoveChildrenToPool();";
                case ComponentType.Group:
                    return "";
                case ComponentType.Slider:
                    return "";
                case ComponentType.ProgressBar:
                    return "";
                case ComponentType.ComboBox:
                    return "";
                case ComponentType.Controller:
                    return ":SetSelectedIndex(0)";
                case ComponentType.Transition:
                    return "";
                default:
                    return "";
            }
        }

        public void Log()
        {
            Debug.Log("-------------------------------------------------------------");
            Debug.Log("组件类型：" + componentType.ToString() + "  属性名称： " + attrbuteName);

            for (int i = 0; i < path.Length; i++)
            {
                Debug.Log("组件路径：" + path[i]);
            }

            Debug.Log("定义代码 ： " + GetVarCodeWithCSharp());
            Debug.Log("获取代码 ： " + GetGetCodeWithCSharp());
        }
    }
}