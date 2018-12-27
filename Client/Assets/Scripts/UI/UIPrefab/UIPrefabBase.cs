using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

namespace SmallUniverse
{
	public enum UIPrefabAlign
	{
		CENTER,
		CENTER_TOP,
		CENTER_BOOTTOM,
		LEFT_TOP,
		LEFT_BOTTOM,
		LEFT_CENTER,
		RIGHT_TOP,
		RIGHT_BOTTOM,
		RIGHT_CENTER,
	}

    public abstract class UIPrefabBase
    {
		public GameObject gameObject;
		public UIPanel panel;
		public GComponent ui;

		private Vector3 m_position;

		/// <summary>
        /// 创建物体
        /// </summary>
        /// <param name="prefabName">prefab name</param>
		protected void Create(string prefabName)
		{
			string bundleName = "uiprefabs/" + prefabName.ToLower();
			string assetName = prefabName + ".prefab";
			var prefab = LevelAsset.GetGameObject(bundleName, assetName);
			gameObject = GameObject.Instantiate<GameObject>(prefab);

			panel = gameObject.GetComponentInChildren<UIPanel>();
			ui = panel.ui;

			OnInitialize();
		}

		/// <summary>
        /// 初始化
        /// </summary>
		protected virtual void OnInitialize()
		{

		}

		/// <summary>
        /// 设置对齐方式
        /// </summary>
        /// <param name="align"></param>
		public void SetAlign(UIPrefabAlign align)
		{
			if(ui == null)
				return;

			if(align == UIPrefabAlign.CENTER)
			{
				m_position = new Vector3(-ui.width / 2, -ui.height / 2, 0);
			}
			else if(align == UIPrefabAlign.CENTER_TOP)
			{
				m_position = new Vector3(-ui.width / 2, 0, 0);
			}
			else if(align == UIPrefabAlign.CENTER_BOOTTOM)
			{
				m_position = new Vector3(-ui.width / 2, -ui.height, 0);
			}
			else if(align == UIPrefabAlign.LEFT_TOP)
			{
				m_position = new Vector3(0, 0, 0);
			}
			else if(align == UIPrefabAlign.LEFT_BOTTOM)
			{
				m_position = new Vector3(0, -ui.height, 0);
			}
			else if(align == UIPrefabAlign.LEFT_CENTER)
			{
				m_position = new Vector3(0, -ui.height / 2, 0);
			}
			else if(align == UIPrefabAlign.RIGHT_TOP)
			{
				m_position = new Vector3(-ui.width, 0, 0);
			}
			else if(align == UIPrefabAlign.RIGHT_BOTTOM)
			{
				m_position = new Vector3(-ui.width, -ui.height, 0);
			}
			else if(align == UIPrefabAlign.RIGHT_CENTER)
			{
				m_position = new Vector3(-ui.width, -ui.height / 2, 0);
			}
			panel.MoveUI(m_position);
		}
    }
}