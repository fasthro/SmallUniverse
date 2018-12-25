using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmallUniverse;
using System.IO;
using SmallUniverse.Manager;

namespace SmallUniverse
{
    public class GameCSV : MonoBehaviour
    {
		private Dictionary<Type, CSV> m_map;

        void Awake()
        {
            m_map = new Dictionary<Type, CSV>();
        }

        public T GetData<T>(int id) where T : CSVDataBase
        {
            return GetCSV<T>().GetDataById<T>(id) as T;
        }

        public List<T> GetDataList<T>() where T : CSVDataBase
        {
            return new List<T>();
        }

        private CSV GetCSV<T>() where T : CSVDataBase
        {
            CSV csv = null;
            if(m_map.TryGetValue(typeof(T), out csv))
            {
                return csv;
            }
            return LoadCSV<T>();
        }

        private CSV LoadCSV<T>() where T : CSVDataBase
        {
            Type type = typeof(T);
            string className = type.Name;
            string csvName = className.Substring("CSV_".Length);
            string bundleName = "csv/" + csvName.ToLower();
            
            var bundle = Game.GetManager<GResManager>().LoadAssetBundle(bundleName);
            var textAsset = bundle.LoadAsset(csvName + ".csv") as TextAsset;
            
            CSV csv = new CSV();
            csv.LoadCSV<T>(textAsset.text.TrimEnd('\n'));
            m_map.Add(type, csv);

            bundle.Unload(true);
            bundle = null;

            return csv;
        }
    }
}
