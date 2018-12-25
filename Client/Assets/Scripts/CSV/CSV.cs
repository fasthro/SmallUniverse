using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SmallUniverse
{
    public class CSV
    {
        #region  private
        // 数据字典
        private Dictionary<int, CSVDataBase> m_DataMap;
        // 数据列表
        private List<CSVDataBase> m_list;

        // 字段索引字典
        private Dictionary<string, int> m_filedMap;
        // 字段类型字典
        private Dictionary<int, string> m_typeMap;
        
        // 是否支持id查询value
        private bool m_supportIdToValue;
        #endregion
        
        // 数据长度
        public int count
        {
            get{
                return m_list.Count;
            }
        }

        public CSV()
        {
            m_DataMap = new Dictionary<int, CSVDataBase>();
            m_list = new List<CSVDataBase>();
            m_filedMap = new Dictionary<string, int>();
            m_typeMap = new Dictionary<int, string>();
            m_supportIdToValue = false;
        }

        public void LoadCSV<T>(string csv) where T : CSVDataBase
        {
            string[] csvs = csv.Split('\n');
            
            // 第一个字段决定是否支持id-value
            string firstFieldStr = string.Empty;
            string firstTypeStr = string.Empty;

            // field
            string[] fields = csvs[0].Split(',');
            for (int i = 0; i < fields.Length; i++)
            {
                if(i == 0)
                {
                    firstFieldStr = fields[i];
                }
                m_filedMap.Add(fields[i], i);
            }

            // types
            string[] types = csvs[1].Split(',');
            for (int i = 0; i < types.Length; i++)
            {
                if(i == 0)
                {
                    firstTypeStr = types[i];
                }
                m_typeMap.Add(i, types[i]);
            }

            m_supportIdToValue = firstFieldStr.Equals("id") && firstTypeStr.Equals("int");

            // data
            for (int i = 2; i < csvs.Length; i++)
            {
                var values = csvs[i].Split(',');
                var data = CreateData<T>(values);

                // data map
                if(m_supportIdToValue)
                {
                    m_DataMap.Add(Convert.ToInt32(values[0]), data);
                }

                // data list
                m_list.Add(data);
            }
        }

        private T CreateData<T>(string[] values) where T : CSVDataBase
        {
            var data = Activator.CreateInstance<T>();
            var fields = typeof(T).GetFields();
            foreach (var field in fields)
            {
                var valueIndex = GetValueIndex(field.Name);
                var valueTypeStr = GetValueTypeStr(valueIndex);
                var value = GetValue(values[valueIndex], valueTypeStr);
                field.SetValue(data, value);
            }
            return data;
        }

        private int GetValueIndex(string fieldName)
        {
            int index = -1;
            if (m_filedMap.TryGetValue(fieldName, out index))
            {
                return index;
            }
            return index;
        }

        private string GetValueTypeStr(int valueIndex)
        {
            string ts = string.Empty;
            if (m_typeMap.TryGetValue(valueIndex, out ts))
            {
                return ts;
            }
            return ts;
        }

        private object GetValue(string valueStr, string valueTypeStr)
        {
            if (valueTypeStr.Equals("int"))
            {
                return Convert.ToInt32(valueStr);
            }
            else if (valueTypeStr.Equals("float"))
            {
                return Convert.ToSingle(valueStr);
            }
            else if (valueTypeStr.Equals("string"))
            {
                return valueStr;
            }
            else if (valueTypeStr.Equals("Vector2") || valueTypeStr.Equals("Vector3"))
            {
                string[] vecs = valueStr.Split('|');
                if (vecs.Length == 2)
                {
                    return new Vector2(Convert.ToSingle(vecs[0]), Convert.ToSingle(vecs[1]));
                }
                else if (vecs.Length == 3)
                {
                    return new Vector3(Convert.ToSingle(vecs[0]), Convert.ToSingle(vecs[1]), Convert.ToSingle(vecs[2]));
                }
            }
            return null;
        }

        public T GetDataById<T>(int id) where T : CSVDataBase
        {
            if(!m_supportIdToValue)
            {
                Debug.LogError("csv [" + typeof(T).Name + "] id-value is not supported");
                return null;
            }

            CSVDataBase data = null;
            if(m_DataMap.TryGetValue(id, out data))
            {
                return data as T;
            }
            return null;
        }

        public T GetDataByIndex<T>(int index) where T : CSVDataBase
        {
            if(index >= 0 || index < m_list.Count)
            {
                return m_list[index] as T;
            }
            return null;
        }

        public List<CSVDataBase> GetDataList()
        {
            return m_list;
        }
    }
}

