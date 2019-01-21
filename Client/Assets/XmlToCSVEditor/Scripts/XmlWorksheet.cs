using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security;
using UnityEngine;

namespace SmallUniverse.GameEditor.XmlEditor {
    public class XmlWorksheet {
        // 数据类型字典
        private static Dictionary<string, string> typeDic;

        public SecurityElement[] rowElements;

        public XmlData describeData;
        public XmlData typeData;
        public XmlData keyData;
        public XmlData[] xmlDatas;

        public int rowCount;
        public int columnCount;

        // 是否为空数据
        public bool isNull;

        private string m_xmlPath;
        public XmlWorksheet (string xmlPath, SecurityElement tableElement) {
            Debug.Log ("CSV -> " + xmlPath);
            m_xmlPath = xmlPath;
            rowCount = int.Parse (tableElement.Attribute ("ss:ExpandedRowCount"));
            columnCount = int.Parse (tableElement.Attribute ("ss:ExpandedColumnCount"));

            isNull = rowCount > 3;

            if (!isNull)
                return;

            rowElements = new SecurityElement[rowCount];
            int index = 0;
            foreach (SecurityElement child in tableElement.Children) {
                if (child.Tag.Equals ("Row")) {
                    rowElements[index] = child;
                    index++;
                }
            }

            describeData = CreateXmlData (rowElements[0]);
            typeData = CreateXmlData (rowElements[1]);
            keyData = CreateXmlData (rowElements[2]);

            xmlDatas = new XmlData[rowCount - 3];
            for (int i = 3; i < rowCount; i++) {
                xmlDatas[i - 3] = CreateXmlData (rowElements[i]);
            }
        }

        private XmlData CreateXmlData (SecurityElement rowElement) {
            XmlData xmlData = new XmlData ();
            xmlData.values = new string[columnCount];
            int index = 0;
            foreach (SecurityElement cellChild in rowElement.Children) {
                if (cellChild.Tag.Equals ("Cell")) {
                    if (cellChild.Children != null) {
                        foreach (SecurityElement dataChild in cellChild.Children) {
                            if (dataChild.Tag.Equals ("Data") || dataChild.Tag.Equals ("ss:Data")) {
                                string text = string.Empty;
                                GetText (dataChild, out text);
                                xmlData.values[index] = text;
                            }
                        }
                        index++;
                    } else {
                        throw new Exception ("excel have empty data grid! " + m_xmlPath);
                    }

                }
            }
            return xmlData;
        }

        private void GetText (SecurityElement element, out string text) {
            
            text = string.Empty;

            if (element.Children != null) {
                foreach (SecurityElement child in element.Children) {
                    GetText (child, out text);
                }
            } else {
                text = element.Text;
            }
        }

        public string ToCSV () {
            string csv = string.Empty;
            for (int i = 0; i < xmlDatas.Length; i++) {
                csv += xmlDatas[i].ToCSV ();
            }
            return csv;
        }

        public string ToCode (string className) {
            string vars = string.Empty;
            for (int i = 0; i < columnCount; i++) {
                vars += GetVarStr (i);
            }
            vars = vars.TrimEnd ('\n');

            string template = GetCodeTemplate ();
            template = template.Replace ("[CLASS_NAME]", className);
            template = template.Replace ("[VARS_CODE]", vars);

            return template;
        }

        private string GetVarStr (int index) {
            string content = string.Format ("        // {0}\n", describeData.values[index]);
            content += string.Format ("        public {0} {1};\n\n", GetTypeStr (typeData.values[index]), keyData.values[index]);
            return content;
        }

        public static string GetTypeStr (string type) {
            if (typeDic == null) {
                typeDic = new Dictionary<string, string> ();
                typeDic.Add ("int", "int");
                typeDic.Add ("float", "float");
                typeDic.Add ("string", "string");
                typeDic.Add ("Vector2", "Vector2");
                typeDic.Add ("Vector3", "Vector3");
                typeDic.Add ("arrayInt", "int[]");
                typeDic.Add ("arrayFloat", "float[]");
                typeDic.Add ("arrayString", "string[]");
            }
            string ts = string.Empty;
            if (typeDic.TryGetValue (type, out ts)) {
                return ts;
            }
            return ts;
        }

        public static string GetCodeTemplate () {
            string template = File.ReadAllText (Path.Combine (Application.dataPath, "XmlToCSVEditor/CodeTemplate.txt"));
            return template;
        }
    }
}