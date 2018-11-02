using FairyGUI.Utils;
using SmallUniverse.UI;
using SmallUniverse.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

/**
 * 1.导出 Fairy UI组件为代码格式，直接在项目中使用
 * 检索组件名称开头带@符号的组件都会被导出
 * 自动生成组件定义，获取，初始化，和销毁代码
 * 导出框架 Panel 默认代码
 * 支持导出c#和lua代码格式
 * 2.根据 Panel Name 导出框架 Panel Map 代码
 */
namespace SmallUniverse.GameEditor.ExportPanelCode
{
    public enum ComponentType
    {
        Component,
        Text,
        Richtext,
        TextInput,
        Loader,
        Image,
        Graph,
        Button,
        List,
        Group,
        Slider,
        ProgressBar,
        ComboBox,
        Controller,
        Transition,
    }

    public enum ExportLanauge
    {
        CSharp,
        Lua,
    }

    public class ExportPanelCode
    {
        // 导出语言类型
        private static ExportLanauge exportLanauge = ExportLanauge.CSharp;
        
        #region csharp
        [MenuItem("SmallUniverse/Panel/export panel-view code", false, 1)]
        public static void ExportViewScript()
        {
            // 全局包对象
            Package pkgObj = new Package();

            var packages = Directory.GetDirectories(PathUtils.UIEditorAssetPath(), "*_panel", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < packages.Length; i++)
            {
                var panels = Directory.GetFiles(packages[i], "*_panel.xml", SearchOption.AllDirectories);
                for (int p = 0; p < panels.Length; p++)
                {
                    Panel panel = new Panel(pkgObj, panels[p]);
                    panel.ExportPanelScript();

                    // 生成代码
                    if (exportLanauge == ExportLanauge.CSharp)
                    {
                        GenerateViewCodeWithCSharp(panel);
                        GeneratePanelCodeWithCSharp(panel);
                    }
                }
            }

            AssetDatabase.Refresh();
        }

        // view code
        public static void GenerateViewCodeWithCSharp(Panel panel)
        {
            string view_source = ReadCodeTemplate("ViewTemplate.txt");
            string vars = string.Empty;
            string gets = string.Empty;
            string inits = string.Empty;
            string disposes = string.Empty;

            var scripts = panel.GetComponentScripts();

            string code = view_source.Replace("[CLASS_NAME]", panel.GetScriptViewName());

            for (int i = 0; i < scripts.Count; i++)
            {
                var script = scripts[i];

                if (!script.isExport)
                    continue;

                // 定义
                vars += "      " + script.GetVarCodeWithCSharp() + "\n";
                // 组件获取
                gets += "           " + script.GetGetCodeWithCSharp() + "\n";
                // 初始化
                var istr = script.GetInitCodeWithCSharp();
                if (!string.IsNullOrEmpty(istr))
                {
                    inits += "           " + istr + "\n";
                }
                // 销毁
                disposes += "           " + script.GetDisposeCodeWithCSharp() + "\n";
            }

            vars = vars.TrimEnd('\n');
            gets = gets.TrimEnd('\n');
            inits = inits.TrimEnd('\n');
            disposes = disposes.TrimEnd('\n');

            code = code.Replace("[VARS_CODE]", vars);
            code = code.Replace("[GET_CODE]", gets);
            code = code.Replace("[INIT_CODE]", inits);
            code = code.Replace("[DISPOSE_CODE]", disposes);

            string path = Application.dataPath + "/Scripts/UI/PanelView/" + panel.GetScriptViewName() + ".cs";
            File.WriteAllText(path, code);
            
            Debug.Log("export view code : " + panel.GetScriptViewName() + ".cs");
        }

        // panel code
        public static void GeneratePanelCodeWithCSharp(Panel panel)
        {
            string  path = Application.dataPath + "/Scripts/UI/Panel/" + panel.GetScriptPanelName() + ".cs";
            if (File.Exists(path))
                return;

            string panel_source = ReadCodeTemplate("PanelTemplate.txt");
            string code = string.Empty;

            code = panel_source.Replace("[CLASS_NAME]", panel.GetScriptPanelName());
            code = code.Replace("[VIEW_NAME]", panel.GetScriptViewName());
            code = code.Replace("[CONSTRUCTOR_NAME]", panel.GetScriptPanelName());
            code = code.Replace("[PANEL_NAME]", panel.GetScriptPanelName());
            code = code.Replace("[MAIN_PACKAGE]", panel.GetMainPackage());
            code = code.Replace("[PANEL_COMPONENT_NAME]", panel.GetComponentName());

            File.WriteAllText(path, code);
            
            Debug.Log("export panel code : " + panel.GetScriptPanelName() + ".cs");
        }

        // panel map code
        [MenuItem("SmallUniverse/Panel/export panel map code", false, 2)]
        public static void GenerateMapCodeWithCSharp()
        {
            string map_source = ReadCodeTemplate("MapTemplate.txt");

            string case_panel = @"                case PanelName.[PANEL_NAME]:
                    return new [PANEL]();";
            string case_view = @"                case PanelName.[PANEL_NAME]:
                    return new [VIEW]View();";
            string code = string.Empty;

            FieldInfo[] fields = typeof(PanelName).GetFields();

            string panel_value = "";
            string view_value = "";

            for (int i = 0; i < fields.Length; i++)
            {
                var name = fields[i].Name;
                if (!name.Equals("value__"))
                {
                    panel_value += case_panel.Replace("[PANEL_NAME]", name).Replace("[PANEL]", name) + "\n";
                    view_value += case_view.Replace("[PANEL_NAME]", name).Replace("[VIEW]", name) + "\n";
                }
            }
            panel_value = panel_value.TrimEnd('\n');
            view_value = view_value.TrimEnd('\n');

            code = map_source.Replace("[CASE_PANEL_CODE]", panel_value);
            code = code.Replace("[CASE_VIEW_CODE]", view_value);

            string path = Application.dataPath + "/Scripts/UI/PanelMap.cs";
            File.WriteAllText(path, code);

            Debug.Log("generate panel map finished!");

            AssetDatabase.Refresh();
        }

        // panel name code
        [MenuItem("SmallUniverse/Panel/export panel name code", false, 3)]
        public static void GenerateNameCodeWithCSharp()
        {
            string name_source = ReadCodeTemplate("NameTemplate.txt");

            string name_code = string.Empty;

            // 全局包对象
            Package pkgObj = new Package();

            var packages = Directory.GetDirectories(PathUtils.UIEditorAssetPath(), "*_panel", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < packages.Length; i++)
            {
                var panels = Directory.GetFiles(packages[i], "*_panel.xml", SearchOption.AllDirectories);
                for (int p = 0; p < panels.Length; p++)
                {
                    Panel panel = new Panel(pkgObj, panels[p]);

                    name_code += "        " + panel.GetScriptPanelName() + ",\n";
                }
            }
            
            string path = Application.dataPath + "/Scripts/UI/PanelName.cs";
            File.WriteAllText(path, name_source.Replace("[PANEL_NAME_CODE]", name_code));

            Debug.Log("generate panel name finished!");

            AssetDatabase.Refresh();
        }
        #endregion

        /// <summary>
        /// 读取模版
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        private static string ReadCodeTemplate(string template)
        {
            string path = "";
            if (exportLanauge == ExportLanauge.CSharp)
            {
                path = Application.dataPath + "/Editor/ExportPanelCode/Template/CSharp/" + template;
            }
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            return "";
        }
     }
}

