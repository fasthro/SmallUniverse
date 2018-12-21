using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Xml;
using System.IO;
using System.Security;
using System.Text;

namespace SmallUniverse.GameEditor.XmlEditor
{
    public class XmlParser
    {
        public List<XmlWorksheet> workSheetList;
        private string m_xmlPath;
        public XmlParser(string xmlPath)
        {
            if (!File.Exists(xmlPath))
            {
                Debug.Log("xml file exist! [" + xmlPath + "]");
                return;
            }
            m_xmlPath = xmlPath;
            ReadXml(File.ReadAllText(xmlPath));
        }

        private void ReadXml(string xml)
        {
            workSheetList = new List<XmlWorksheet>();

            SecurityParser parser = new SecurityParser();
            parser.LoadXml(xml);

            SecurityElement rootElement = parser.ToXml();
            foreach (SecurityElement childRoot in rootElement.Children)
            {
                if (childRoot.Tag.Equals("Worksheet"))
                {
                    foreach (SecurityElement child in childRoot.Children)
                    {
                        if (child.Tag.Equals("Table"))
                        {
                            workSheetList.Add(new XmlWorksheet(m_xmlPath, child));
                        }
                    }
                }
            }
        }

        public void ExportCSV(string csvPath)
        {
            if (string.IsNullOrEmpty(csvPath))
            {
                Debug.Log("export csv path is empty!");
                return;
            }

            if (workSheetList.Count <= 0)
            {
                Debug.Log("export csv path xml is empty!");
                return;
            }

            string csv = string.Empty;

            for (int i = 0; i < workSheetList.Count; i++)
            {
                csv += workSheetList[i].ToCSV();
            }

            WriteFile(csvPath, csv);
        }

        public void ExportCode(string codePath, string className)
        {
            if (string.IsNullOrEmpty(codePath))
            {
                Debug.Log("export code path is empty!");
                return;
            }

            if (workSheetList.Count <= 0)
            {
                Debug.Log("export csv path xml is empty!");
                return;
            }

            WriteFile(codePath, workSheetList[0].ToCode(className));
        }


        private void WriteFile(string filePath, string content)
        {
            string dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            File.WriteAllText(filePath, content, Encoding.UTF8);
        }
    }
}
