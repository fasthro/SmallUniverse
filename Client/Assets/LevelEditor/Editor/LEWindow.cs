using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

namespace SmallUniverse.GameEditor.LevelEditor
{
    /// <summary>
    /// 住面板页签菜单
    /// </summary>
    public enum TabMenu
    {
        Function,              // 功能模块
        Area,                    // 区域操作
    }

    /// <summary>
    /// 场景工具类型
    /// </summary>
    public enum SceneTool
    {
        None,
        Selector,                 // 选择
        Brush,                     // 笔刷
        Sucker,                   // 吸管
        Erase,                     // 擦除
        GPHeightUp,                // 调节层
        GPHeightDown,           // 调节层
        RotateX,                // 旋转 X 轴
        RotateY,                // 旋转 Y 轴
        RotateZ,                // 旋转 Z 轴
    }

    /// <summary>
    /// 鼠标在 GizmoPanel 上的状态
    /// </summary>
    public enum GizmoPanelState
    {
        Exit,
        Enter,
    }

    public class LEWindow : EditorWindow
    {
        // window
        public static LEWindow Inst;

        // config
        public static LEIconConfig IconConfig;
        public static LEPrefabConfig PrefabConfig;

        // GizmoPanel
        private LEGizmoPanel m_gizmoPanel;
        [HideInInspector]
        public Vector2Int gridDimensions;
        private Vector2Int m_gridDimensions;

        // 当前网格的高度
        private int _gridHeight;
        public int gridHeight
        {
            get
            {
                return _gridHeight;
            }
        }

        // 当前画格子所在区域
        private int m_area = 1;
        public int area
        {
            get
            {
                return m_area;
            }
        }

        // 是否开启编辑模式
        [HideInInspector]
        public bool editorEnabled = false;
        private bool m_editorEnabled = true;

        // model view
        private Vector2 m_modelViewScrollPosition;
        private int m_modelViewHorizontalCounter;
        private int m_modelViewColumn;

        // area view
        private Vector2 m_areaViewScrollPosition;

        // 当前 Tab 菜单
        public TabMenu currentSelectTabMenu = TabMenu.Function;
        // 当前选择的工具
        public SceneTool currentSelectTool;
        // 当前选择的功能
        public GridFunction currentSelectFunction = GridFunction.Ground;

        // 当前选择的 prefab
        private LEPrefab m_currentSelectPrefab;
        // 当前选中 prefab go
        private LEPrefabGo m_currentSelectPrefabGo;

        // 当前鼠标所在GizmoGrid上的位置
        private Vector3 m_mousePosition;

        // 关卡场景名称
        private string m_levelName;

        // Enumerator
        private Dictionary<string, LEPrefabGo>.Enumerator m_prefabGoEnumerator;
        private Dictionary<string, LEArea>.Enumerator m_areaEnumerator;

        // GizmoPanelState
        private GizmoPanelState m_gizmoPanelState = GizmoPanelState.Exit;

        // 选择器工具
        private LESelector m_selector;
        public LESelector selector
        {
            get
            {
                if (m_selector == null)
                {
                    m_selector = new LESelector();
                }
                return m_selector;
            }
        }

        // 笔刷工具
        private LEBrush m_brush;
        public LEBrush brush
        {
            get
            {
                if (m_brush == null)
                {
                    m_brush = new LEBrush();
                }
                return m_brush;
            }
        }

        // 吸管工具
        private LESucker m_sucker;
        public LESucker sucker
        {
            get
            {
                if (m_sucker == null)
                {
                    m_sucker = new LESucker();
                }
                return m_sucker;
            }
        }

        // 擦除工具
        private LEErase m_erase;
        public LEErase erase
        {
            get
            {
                if (m_erase == null)
                {
                    m_erase = new LEErase();
                }
                return m_erase;
            }
        }


        // content
        private GUIContent m_content;
        // controlId
        private int m_controlId;
        // GUI color
        private Color m_guiColor;


        [MenuItem("SmallUniverse/LevelEditor/Open Level Editor Window")]
        public static void Initialize()
        {
            Inst = GetWindow<LEWindow>(false, "Level Editor");
            Inst.titleContent.text = "Level Editor";
        }

        void OnEnable()
        {
            if (GameObject.Find(LEConst.EditorGizmoGridPanelName) == null
                || GameObject.Find(LEConst.EditorLevelName) == null)
                return;

            if (Inst == null)
            {
                Initialize();
            }

            m_levelName = LELevel.Inst.levelName;

            // 关卡初始化
            LELevel.Inst.Initialize();

            // 网格尺寸
            var dim = LELevel.Inst.GetGizmoDimension();
            // 网格实例获取
            if (m_gizmoPanel == null)
            {
                var gizmoGridGo = GameObject.Find(LEConst.EditorGizmoGridPanelName);
                if (gizmoGridGo != null)
                {
                    m_gizmoPanel = gizmoGridGo.GetComponent<LEGizmoPanel>();
                }
            }
            gridDimensions.x = dim.x;
            gridDimensions.y = dim.y;

            m_gridDimensions.x = dim.x;
            m_gridDimensions.y = dim.y;

            _gridHeight = 0;

            m_gizmoPanel.SetSize(dim.x, dim.y);
            m_gizmoPanel.SetHight(_gridHeight);

            // config
            IconConfig = AssetDatabase.LoadAssetAtPath(LEConst.IconConfigPath, typeof(LEIconConfig)) as LEIconConfig;
            PrefabConfig = new LEPrefabConfig();
            PrefabConfig.Initialize();

            // scene view
            SceneView.onSceneGUIDelegate -= OnSceneGUI;
            SceneView.onSceneGUIDelegate += OnSceneGUI;

            EditorSceneManager.sceneOpened += OnSceneOpened;

            // 将资源预览缓存设置为可以同时保存屏幕上所有可见预览的大小
            AssetPreview.SetPreviewTextureCacheSize(1000);
        }

        void OnDestroy()
        {
            Inst = null;

            // 网格实例销毁
            if (m_gizmoPanel != null)
            {
                m_gizmoPanel.GizmoGridEnabled = false;
            }
            m_gizmoPanel = null;

            // 选择器工具
            selector.Destroy();
            m_selector = null;

            // 笔刷工具
            brush.Destroy();
            m_brush = null;

            // 吸管工具
            sucker.Destroy();
            m_sucker = null;

            // 擦除工具
            erase.Destroy();
            m_erase = null;

            SceneView.onSceneGUIDelegate -= OnSceneGUI;
            EditorSceneManager.sceneOpened -= OnSceneOpened;
        }

        /// <summary>
        /// 场景打开
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
        void OnSceneOpened(UnityEngine.SceneManagement.Scene scene, OpenSceneMode mode)
        {
            // 关闭关卡编辑器
            Close();

            if (GameObject.Find(LEConst.EditorGizmoGridPanelName) != null
               && GameObject.Find(LEConst.EditorLevelName) != null)
            {
                // 重启关卡编辑器
                Initialize();
            }
        }

        void OnGUI()
        {
            if (Application.isPlaying)
                return;

            if (GameObject.Find(LEConst.EditorGizmoGridPanelName) == null
                || GameObject.Find(LEConst.EditorLevelName) == null)
            {
                editorEnabled = false;

                if (GUILayout.Button("Open Create Level Window", GUILayout.Height(30)))
                {
                    LECreateWindow.Initialize();
                    Inst.Close();
                }
            }
            else
            {
                EditorGUILayout.BeginVertical("box");

                // 编辑模式选项按钮
                editorEnabled = GUILayout.Toggle(editorEnabled, "Enable Editor", "Button", GUILayout.Height(30));

                if (editorEnabled != m_editorEnabled)
                {
                    m_editorEnabled = editorEnabled;

                    if (editorEnabled)
                    {
                        // 编辑模式

                        // 开启网格
                        m_gizmoPanel.GizmoGridEnabled = true;
                    }
                    else
                    {
                        // 非编辑模式

                        // 关闭网格
                        m_gizmoPanel.GizmoGridEnabled = false;
                        // 销毁画刷
                        brush.Destroy();
                    }

                    // 场景重绘
                    SceneView.RepaintAll();
                }

                // 网格规模设置
                EditorGUILayout.BeginVertical("box");
                gridDimensions = EditorGUILayout.Vector2IntField("Grid Dimensions", gridDimensions);

                // 网格规模不能小于2 * 2
                if (gridDimensions.x < 2)
                    gridDimensions.x = 2;
                if (gridDimensions.y < 2)
                    gridDimensions.y = 2;

                // 网格规模必须是 2 的整数倍
                if (gridDimensions.x % 2 != 0)
                    gridDimensions.x += 1;
                if (gridDimensions.y % 2 != 0)
                    gridDimensions.y += 1;

                if (gridDimensions.x != m_gridDimensions.x || gridDimensions.y != m_gridDimensions.y)
                {
                    m_gridDimensions.x = gridDimensions.x;
                    m_gridDimensions.y = gridDimensions.y;

                    m_gizmoPanel.SetSize(gridDimensions.x, gridDimensions.y);
                }

                EditorGUILayout.EndVertical();

                // 网格高度设置
                EditorGUILayout.BeginVertical("box");
                GUILayout.Label("Grid Height: " + _gridHeight.ToString());
                EditorGUILayout.EndVertical();

                #region tab menu

                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.BeginHorizontal();
                // function tab menu
                if (GUILayout.Toggle(currentSelectTabMenu == TabMenu.Function, m_content = new GUIContent(TabMenu.Function.ToString()), "Button", GUILayout.Height(30)))
                {
                    currentSelectTabMenu = TabMenu.Function;
                }

                // group tab menu
                if (GUILayout.Toggle(currentSelectTabMenu == TabMenu.Area, m_content = new GUIContent(TabMenu.Area.ToString()), "Button", GUILayout.Height(30)))
                {
                    currentSelectTabMenu = TabMenu.Area;
                }
                EditorGUILayout.EndHorizontal();

                // 画功能库预览
                if (currentSelectTabMenu == TabMenu.Function)
                {
                    DrawFunctionView();
                }
                else if (currentSelectTabMenu == TabMenu.Area)
                {
                    DrawAreaView();
                }
                EditorGUILayout.EndVertical();
                #endregion

                // 保存场景
                EditorGUILayout.BeginVertical("box");
                if (GUILayout.Button("Save Level", GUILayout.Height(30)))
                {
                    EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), LEUtils.GetLevelScenePath(LELevel.Inst.levelName));
                }

                // 生成场景配置
                if (GUILayout.Button("Export Xml", GUILayout.Height(30)))
                {
                    EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), LEUtils.GetLevelScenePath(LELevel.Inst.levelName));

                    LELevel.Inst.ExportXml();
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.EndVertical();
            }
        }

        /// <summary>
        /// 画功能库预览
        /// </summary>
        private void DrawFunctionView()
        {
            EditorGUILayout.BeginHorizontal();
            // Ground
            if (GUILayout.Toggle(currentSelectFunction == GridFunction.Ground, m_content = new GUIContent(GridFunction.Ground.ToString()), "Button", GUILayout.Height(20)))
            {
                currentSelectFunction = GridFunction.Ground;
                m_currentSelectPrefab = PrefabConfig.GetPrefab(currentSelectFunction.ToString());
                if (currentSelectTool == SceneTool.Brush)
                {
                    brush.CleanPrefabGo();
                }
            }

            // Door
            if (GUILayout.Toggle(currentSelectFunction == GridFunction.Door, m_content = new GUIContent(GridFunction.Door.ToString()), "Button", GUILayout.Height(20)))
            {
                currentSelectFunction = GridFunction.Door;
                m_currentSelectPrefab = PrefabConfig.GetPrefab(currentSelectFunction.ToString());
                if (currentSelectTool == SceneTool.Brush)
                {
                    brush.CleanPrefabGo();
                }
            }

            // Transfer
            if (GUILayout.Toggle(currentSelectFunction == GridFunction.Transfer, m_content = new GUIContent(GridFunction.Transfer.ToString()), "Button", GUILayout.Height(20)))
            {
                currentSelectFunction = GridFunction.Transfer;
                m_currentSelectPrefab = PrefabConfig.GetPrefab(currentSelectFunction.ToString());
                if (currentSelectTool == SceneTool.Brush)
                {
                    brush.CleanPrefabGo();
                }
            }

            // Trap
            if (GUILayout.Toggle(currentSelectFunction == GridFunction.Trap, m_content = new GUIContent(GridFunction.Trap.ToString()), "Button", GUILayout.Height(20)))
            {
                currentSelectFunction = GridFunction.Trap;
                m_currentSelectPrefab = PrefabConfig.GetPrefab(currentSelectFunction.ToString());
                if (currentSelectTool == SceneTool.Brush)
                {
                    brush.CleanPrefabGo();
                }
            }

            // Player
            if (GUILayout.Toggle(currentSelectFunction == GridFunction.Player, m_content = new GUIContent(GridFunction.Player.ToString()), "Button", GUILayout.Height(20)))
            {
                currentSelectFunction = GridFunction.Player;
                m_currentSelectPrefab = PrefabConfig.GetPrefab(currentSelectFunction.ToString());
                if (currentSelectTool == SceneTool.Brush)
                {
                    brush.CleanPrefabGo();
                }
            }

            // Monster
            if (GUILayout.Toggle(currentSelectFunction == GridFunction.Monster, m_content = new GUIContent(GridFunction.Monster.ToString()), "Button", GUILayout.Height(20)))
            {
                currentSelectFunction = GridFunction.Monster;
                m_currentSelectPrefab = PrefabConfig.GetPrefab(currentSelectFunction.ToString());
                if (currentSelectTool == SceneTool.Brush)
                {
                    brush.CleanPrefabGo();
                }
            }
            EditorGUILayout.EndHorizontal();

            // 资源库预览界面
            if (m_currentSelectPrefab != null)
            {
                DrawModelView();
            }
        }

        /// <summary>
        /// 画资源库物体预览展示
        /// </summary>
        private void DrawModelView()
        {
            if (Inst == null)
                return;

            m_modelViewScrollPosition = EditorGUILayout.BeginScrollView(m_modelViewScrollPosition);

            m_modelViewHorizontalCounter = 0;

            m_modelViewColumn = (int)(Inst.position.width / 200f);
            if (m_modelViewColumn <= 1)
            {
                m_modelViewColumn = 1;
            }

            EditorGUILayout.BeginHorizontal();

            using (m_prefabGoEnumerator = m_currentSelectPrefab.gos.GetEnumerator())
            {
                while (m_prefabGoEnumerator.MoveNext())
                {
                    var prefabGo = m_prefabGoEnumerator.Current.Value;

                    EditorGUILayout.BeginVertical();

                    // 模型预览
                    Texture2D previewImage = AssetPreview.GetAssetPreview(prefabGo.go);
                    m_content = new GUIContent(previewImage);
                    // 选中状态
                    bool selected = false;
                    if (m_currentSelectPrefabGo != null)
                    {
                        if (m_currentSelectPrefabGo.name == prefabGo.go.name)
                        {
                            selected = true;
                        }
                    }

                    bool isSelected = GUILayout.Toggle(selected, m_content, GUI.skin.button);
                    if (isSelected && editorEnabled)
                    {
                        m_currentSelectPrefabGo = prefabGo;
                        // 选中，设置笔刷
                        ChangeSceneTool(SceneTool.Brush);
                        brush.SetPrefabGo(m_currentSelectPrefabGo);
                    }

                    EditorGUILayout.BeginHorizontal("Box");
                    EditorGUILayout.LabelField(prefabGo.go.name);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();

                    m_modelViewHorizontalCounter++;
                    if (m_modelViewHorizontalCounter == m_modelViewColumn)
                    {
                        m_modelViewHorizontalCounter = 0;
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                    }
                }
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        /// 画区域管理视图
        /// </summary>
        private void DrawAreaView()
        {
            if (Inst == null)
                return;

            m_areaViewScrollPosition = EditorGUILayout.BeginScrollView(m_areaViewScrollPosition);

            using (m_areaEnumerator = LELevel.Inst.areas.GetEnumerator())
            {
                while (m_areaEnumerator.MoveNext())
                {
                    var area = m_areaEnumerator.Current.Value;

                    if (GUILayout.Button("Area : " + area.areaName, "box", GUILayout.Width(Inst.position.width - 28)))
                    {
                        area.e_showing = !area.e_showing;
                    }
                    if (area.e_showing)
                    {
                        EditorGUILayout.BeginVertical("box");
                        // showiung
                        area.showing = GUILayout.Toggle(area.showing, "Showing Scene");
                        
                        // 导出格子四周
                        area.e_exportAround = EditorGUILayout.Toggle("Export Grid Around : ", area.e_exportAround);
                        
                        // animation start position
                        area.e_animationStartPosition = EditorGUILayout.Vector3IntField("Animation Start Position : ", area.e_animationStartPosition);
                        var startGrid = area.GetGrid(LELevel.GetKey(area.e_animationStartPosition, area.areaName));
                        if(startGrid == null)
                        {
                            EditorGUILayout.BeginHorizontal("box");
                            m_guiColor = GUI.color;
                            GUI.color = Color.red;
                            EditorGUILayout.LabelField("Error : Animation Start Position Invalid!");
                            GUI.color = m_guiColor;
                            EditorGUILayout.EndHorizontal();
                        }
                        area.SetAnimationStartGrid(startGrid);
                        EditorGUILayout.EndVertical();
                    }

                }
            }

            EditorGUILayout.EndScrollView();
        }

        void OnSceneGUI(SceneView sceneView)
        {
            if (!editorEnabled)
                return;

            // 计算鼠标在Grid上的位置
            CalculateSceneMousePosition();

            m_controlId = GUIUtility.GetControlID(FocusType.Passive);

            // 画选择工具栏
            DrawToolbar(sceneView);

            switch (currentSelectTool)
            {
                case SceneTool.Selector:
                    selector.DrawScenePreview(sceneView, m_mousePosition);
                    break;
                case SceneTool.Brush:
                    brush.DrawScenePreview(sceneView, m_mousePosition);
                    break;
                case SceneTool.Sucker:
                    sucker.DrawScenePreview(sceneView, m_mousePosition);
                    break;
                case SceneTool.Erase:
                    erase.DrawScenePreview(sceneView, m_mousePosition);
                    break;
            }

            HandleSceneInput();

            HandleUtility.AddDefaultControl(m_controlId);
        }

        /// <summary>
        /// 场景按键输入
        /// </summary>
        private void HandleSceneInput()
        {
            switch (currentSelectTool)
            {
                case SceneTool.Selector:
                    selector.HandleInput(m_mousePosition);
                    break;
                case SceneTool.Brush:
                    brush.HandleInput(m_mousePosition);
                    break;
                case SceneTool.Sucker:
                    sucker.HandleInput(m_mousePosition);
                    break;
                case SceneTool.Erase:
                    erase.HandleInput(m_mousePosition);
                    break;
            }
        }

        /// <summary>
        /// 画工具栏
        /// </summary>
        private void DrawToolbar(SceneView sceneView)
        {
            Handles.BeginGUI();

            GUI.skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene);

            int nextInterval = LESetting.SceneToolSize + LESetting.SceneTooIInterval;
            float rightX = sceneView.position.width - LESetting.SceneToolSize - 5;
            float downY = sceneView.position.height - LESetting.SceneToolSize - 25;

            #region  左上角

            // 选择工具
            m_content = new GUIContent(IconConfig.GetIconTexture("iconCursor"));
            if (GUI.Toggle(new Rect(LESetting.SceneTooIX, LESetting.SceneTooIY, LESetting.SceneToolSize, LESetting.SceneToolSize), currentSelectTool == SceneTool.Selector, m_content, GUI.skin.button))
            {
                brush.Close();
                sucker.Close();
                erase.Close();
                currentSelectTool = SceneTool.Selector;
            }

            // 笔刷工具
            m_content = new GUIContent(IconConfig.GetIconTexture("iconBlockMode"));
            if (GUI.Toggle(new Rect(LESetting.SceneTooIX, LESetting.SceneTooIY + nextInterval, LESetting.SceneToolSize, LESetting.SceneToolSize), currentSelectTool == SceneTool.Brush, m_content, GUI.skin.button))
            {
                selector.Close();
                sucker.Close();
                erase.Close();
                currentSelectTool = SceneTool.Brush;
            }

            // 吸管工具
            m_content = new GUIContent(IconConfig.GetIconTexture("iconPicker"));
            if (GUI.Toggle(new Rect(LESetting.SceneTooIX, LESetting.SceneTooIY + nextInterval * 2, LESetting.SceneToolSize, LESetting.SceneToolSize), currentSelectTool == SceneTool.Sucker, m_content, GUI.skin.button))
            {
                selector.Close();
                brush.Close();
                erase.Close();
                currentSelectTool = SceneTool.Sucker;
            }

            // 擦除工具
            m_content = new GUIContent(IconConfig.GetIconTexture("iconErase"));
            if (GUI.Toggle(new Rect(LESetting.SceneTooIX, LESetting.SceneTooIY + nextInterval * 3, LESetting.SceneToolSize, LESetting.SceneToolSize), currentSelectTool == SceneTool.Erase, m_content, GUI.skin.button))
            {
                selector.Close();
                brush.Close();
                sucker.Close();
                currentSelectTool = SceneTool.Erase;
            }

            #endregion

            #region 左下角

            // 上一区域
            m_content = new GUIContent(IconConfig.GetIconTexture("iconArrowUp"));
            if (GUI.Button(new Rect(LESetting.SceneTooIX, downY, LESetting.SceneToolSize, LESetting.SceneToolSize / 2), m_content))
            {
                m_area++;
            }

            // 下一区域
            m_content = new GUIContent(IconConfig.GetIconTexture("iconArrowDown"));
            if (GUI.Button(new Rect(LESetting.SceneTooIX, downY + LESetting.SceneToolSize / 2, LESetting.SceneToolSize, LESetting.SceneToolSize / 2), m_content))
            {
                m_area--;

                if (m_area <= 0)
                {
                    m_area = 1;
                }
            }

            GUILayout.BeginArea(new Rect(LESetting.SceneTooIX + nextInterval, downY, 50, LESetting.SceneToolSize), EditorStyles.textArea);
            {
                GUILayout.BeginVertical();
                GUILayout.Label("Area", EditorStyles.label, GUILayout.Width(50));
                GUILayout.Label(m_area.ToString(), EditorStyles.label, GUILayout.Width(50));
                GUILayout.EndVertical();
            }
            GUILayout.EndArea();

            #endregion

            #region 右下角

            // Show Gizmo Panel Base
            m_content = new GUIContent(IconConfig.GetIconTexture("iconIsolate"));
            m_gizmoPanel.BaseEnabled = GUI.Toggle(new Rect(rightX, downY, LESetting.SceneToolSize, LESetting.SceneToolSize), m_gizmoPanel.BaseEnabled, m_content, GUI.skin.button);


            // GizmoPanelUp
            m_content = new GUIContent(IconConfig.GetIconTexture("iconGridUp"));
            if (GUI.Toggle(new Rect(rightX - nextInterval, downY, LESetting.SceneToolSize, LESetting.SceneToolSize), false, m_content, GUI.skin.button))
            {
                currentSelectTool = SceneTool.None;
                _gridHeight++;

                m_gizmoPanel.SetHight(_gridHeight);

                Repaint();
            }

            // GizmoPanelDown
            m_content = new GUIContent(IconConfig.GetIconTexture("iconGridDown"));
            if (GUI.Toggle(new Rect(rightX - nextInterval * 2, downY, LESetting.SceneToolSize, LESetting.SceneToolSize), false, m_content, GUI.skin.button))
            {
                currentSelectTool = SceneTool.None;
                _gridHeight--;

                m_gizmoPanel.SetHight(_gridHeight);

                Repaint();
            }
            #endregion

            Handles.EndGUI();
        }

        /// <summary>
        /// 计算鼠标在场景中Grid上的位置
        /// </summary>
        private void CalculateSceneMousePosition()
        {
            if (Event.current == null)
            {
                return;
            }

            Vector2 mp = new Vector2(Event.current.mousePosition.x, Event.current.mousePosition.y);

            Ray ray = HandleUtility.GUIPointToWorldRay(mp);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer(LEConst.EditorGizmoGridLayerName)) == true)
            {
                Vector3 shiftOffset = m_gizmoPanel.transform.position;
                shiftOffset.x = shiftOffset.x - (int)shiftOffset.x;
                shiftOffset.y = shiftOffset.y - (int)shiftOffset.y;
                shiftOffset.z = shiftOffset.z - (int)shiftOffset.z;

                m_mousePosition.x = Mathf.Round(((hit.point.x + shiftOffset.x) - hit.normal.x * 0.001f) / 1) * 1 - shiftOffset.x;
                m_mousePosition.z = Mathf.Round(((hit.point.z + shiftOffset.z) - hit.normal.z * 0.001f) / 1) * 1 - shiftOffset.z;
                m_mousePosition.y = _gridHeight + m_gizmoPanel.transform.position.y;

                if (m_gizmoPanelState == GizmoPanelState.Exit)
                {
                    m_gizmoPanelState = GizmoPanelState.Enter;
                    OnGizmoPanelState();
                }
            }
            else
            {
                if (m_gizmoPanelState == GizmoPanelState.Enter)
                {
                    m_gizmoPanelState = GizmoPanelState.Exit;
                    OnGizmoPanelState();
                }
            }
        }

        /// <summary>
        /// GizmoPanel State
        /// </summary>
        private void OnGizmoPanelState()
        {
            switch (currentSelectTool)
            {
                case SceneTool.Selector:
                    selector.HaneleGizmoPanelState(m_gizmoPanelState);
                    break;
                case SceneTool.Brush:
                    brush.HaneleGizmoPanelState(m_gizmoPanelState);
                    break;
                case SceneTool.Sucker:
                    sucker.HaneleGizmoPanelState(m_gizmoPanelState);
                    break;
                case SceneTool.Erase:
                    erase.HaneleGizmoPanelState(m_gizmoPanelState);
                    break;
            }
        }

        /// <summary>
        /// 吸管工具吸附
        /// </summary>
        /// <param name="function"></param>
        public void OnSucker(LEGrid grid)
        {
            ChangeFunction(grid.function);
            ChangeSceneTool(SceneTool.Brush);
            m_currentSelectPrefabGo = grid.prefabGo;
            brush.SetPrefabGo(m_currentSelectPrefabGo);
        }

        /// <summary>
        /// 切换场景工具
        /// </summary>
        /// <param name="tool"></param>
        public void ChangeSceneTool(SceneTool tool)
        {
            selector.Close();
            brush.Close();
            sucker.Close();
            erase.Close();

            currentSelectTool = tool;
        }

        /// <summary>
        /// 切换 function
        /// </summary>
        /// <param name="function"></param>
        public void ChangeFunction(GridFunction function)
        {
            currentSelectFunction = function;

            Repaint();
        }
    }
}
