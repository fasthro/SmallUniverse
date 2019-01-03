using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse.GameEditor.XmlEditor
{
    public class XmlData
    {
        public string[] values;

        public string ToCSV()
        {
            string csv = string.Empty;

            for (int i = 0; i < values.Length; i++)
            {
                if (string.IsNullOrEmpty(csv))
                {
                    csv = values[i];
                }
                else
                {
                    csv = string.Format("{0},{1}", csv, values[i].TrimEnd('\r'));
                }
            }
            return csv + "\n";
        }
    }
}