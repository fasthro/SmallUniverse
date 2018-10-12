using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.Collections.Generic;

namespace SU.Editor.LevelEditor
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
        private LEGizmoPanel gizmoPanel;
        private Vector2Int gridDimensions;
        private Vector2Int _gridDimensions;

        // 当前网格的高度
        private int _gridHeight;
        public int gridHeight {
            get {
                return _gridHeight;
            }
        }

        // 当前画格子所在区域
        private int _area = 1;
        public int area
        {
            get
            {
                return _area;
            }
        }

        // 是否开启编辑模式
        private bool editorEnabled = false;
        private bool _editorEnabled = true;
        
        // model view
        private Vector2 modelViewScrollPosition;
        private int modelViewHorizontalCounter;
        private int modelViewColumn;

        // 当前 Tab 菜单
        private TabMenu currentSelectTabMenu = TabMenu.Function;
        // 当前选择的工具
        private SceneTool currentSelectTool;
        // 当前选择的功能
        public GridFunction currentSelectFunction = GridFunction.Ground;
        // 当前选择的 prefab
        private LEPrefab currentSelectPrefab;
        // 当前选中 prefab go
        private LEPrefabGo currentSelectPrefabGo;

        // 当前鼠标所在GizmoGrid上的位置
        private Vector3 mousePosition;

        // 关卡场景名称
        private string levelName;
        
        // Enumerator
        private Dictionary<string, LEPrefabGo>.Enumerator prefabGoEnumerator;

        // GizmoPanelState
        private GizmoPanelState gizmoPanelState = GizmoPanelState.Exit;

        // 选择器工具
        private LESelector _selector;
        public LESelector selector
        {
            get
            {
                if (_selector == null)
                {
                    _selector = new LESelector();
                }
                return _selector;
            }
        }

        // 笔刷工具
        private LEBrush _brush;
        public LEBrush brush
        {
            get
            {
                if (_brush == null)
                {
                    _brush = new LEBrush();
                }
                return _brush;
            }
        }

        // 吸管工具
        private LESucker _sucker;
        public LESucker sucker
        {
            get
            {
                if (_sucker == null)
                {
                    _sucker = new LESucker();
                }
                return _sucker;
            }
        }

        // 擦除工具
        private LEErase _erase;
        public LEErase erase
        {
            get
            {
                if (_erase == null)
                {
                    _erase = new LEErase();
                }
                return _erase;
            }
        }


        // content
        private GUIContent content;
        // controlId
        private int controlId;

        [MenuItem("SU/LevelEditor/Open Level Editor Window")]
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

            levelName = LELevel.Inst.levelName;

            // 网格实例获取
            if (gizmoPanel == null)
            {
                var gizmoGridGo = GameObject.Find(LEConst.EditorGizmoGridPanelName);
                if (gizmoGridGo != null)
                {
                    gizmoPanel = gizmoGridGo.GetComponent<LEGizmoPanel>();

                    gridDimensions.x = gizmoPanel.width;
                    gridDimensions.y = gizmoPanel.lenght;

                    _gridDimensions.x = gizmoPanel.width;
                    _gridDimensions.y = gizmoPanel.lenght;

                    _gridHeight = gizmoPanel.height;
                }
            }

            // 关卡初始化
            LELevel.Inst.Initialize();

            // config
            IconConfig = AssetDatabase.LoadAssetAtPath(LEConst.IconConfigPath, typeof(LEIconConfig)) as LEIconConfig;
            PrefabConfig = AssetDatabase.LoadAssetAtPath(LEConst.RepositoryConfigPath, typeof(LEPrefabConfig)) as LEPrefabConfig;
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
            if (gizmoPanel != null)
            {
                gizmoPanel.GizmoGridEnabled = false;
            }
            gizmoPanel = null;

            // 选择器工具
            selector.Destroy();
            _selector = null;

            // 笔刷工具
            brush.Destroy();
            _brush = null;

            // 吸管工具
            sucker.Destroy();
            _sucker = null;

            // 擦除工具
            erase.Destroy();
            _erase = null;

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

                if (editorEnabled != _editorEnabled)
                {
                    _editorEnabled = editorEnabled;

                    if (editorEnabled)
                    {
                        // 编辑模式

                        // 开启网格
                        gizmoPanel.GizmoGridEnabled = true;
                    }
                    else
                    {
                        // 非编辑模式

                        // 关闭网格
                        gizmoPanel.GizmoGridEnabled = false;
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

                if (gridDimensions.x != _gridDimensions.x || gridDimensions.y != _gridDimensions.y)
                {
                    _gridDimensions.x = gridDimensions.x;
                    _gridDimensions.y = gridDimensions.y;

                    gizmoPanel.SetSize(gridDimensions.x, gridDimensions.y);
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
                if (GUILayout.Toggle(currentSelectTabMenu == TabMenu.Function, content = new GUIContent(TabMenu.Function.ToString()), "Button", GUILayout.Height(30)))
                {
                    currentSelectTabMenu = TabMenu.Function;
                }

                // group tab menu
                if (GUILayout.Toggle(currentSelectTabMenu == TabMenu.Area, content = new GUIContent(TabMenu.Area.ToString()), "Button", GUILayout.Height(30)))
                {
                    currentSelectTabMenu = TabMenu.Area;
                }
                EditorGUILayout.EndHorizontal();

                // 画功能库预览
                if (currentSelectTabMenu == TabMenu.Function)
                {
                    DrawFunctionView();
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
                if (GUILayout.Button("Generate Level Data", GUILayout.Height(30)))
                {
                    EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), LEUtils.GetLevelScenePath(LELevel.Inst.levelName));
                    
                    LELevel.Inst.GenerateLevelData();
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
            if (GUILayout.Toggle(currentSelectFunction == GridFunction.Ground, content = new GUIContent(GridFunction.Ground.ToString()), "Button", GUILayout.Height(20)))
            {
                currentSelectFunction = GridFunction.Ground;
                currentSelectPrefab = PrefabConfig.GetPrefab(currentSelectFunction.ToString());
                if (currentSelectTool == SceneTool.Brush)
                {
                    brush.CleanPrefabGo();
                }
            }

            // Door
            if (GUILayout.Toggle(currentSelectFunction == GridFunction.Door, content = new GUIContent(GridFunction.Door.ToString()), "Button", GUILayout.Height(20)))
            {
                currentSelectFunction = GridFunction.Door;
                currentSelectPrefab = PrefabConfig.GetPrefab(currentSelectFunction.ToString());
                if (currentSelectTool == SceneTool.Brush)
                {
                    brush.CleanPrefabGo();
                }
            }

            // Transfer
            if (GUILayout.Toggle(currentSelectFunction == GridFunction.Transfer, content = new GUIContent(GridFunction.Transfer.ToString()), "Button", GUILayout.Height(20)))
            {
                currentSelectFunction = GridFunction.Transfer;
                currentSelectPrefab = PrefabConfig.GetPrefab(currentSelectFunction.ToString());
                if (currentSelectTool == SceneTool.Brush)
                {
                    brush.CleanPrefabGo();
                }
            }

            // Trap
            if (GUILayout.Toggle(currentSelectFunction == GridFunction.Trap, content = new GUIContent(GridFunction.Trap.ToString()), "Button", GUILayout.Height(20)))
            {
                currentSelectFunction = GridFunction.Trap;
                currentSelectPrefab = PrefabConfig.GetPrefab(currentSelectFunction.ToString());
                if (currentSelectTool == SceneTool.Brush)
                {
                    brush.CleanPrefabGo();
                }
            }

            // Player
            if (GUILayout.Toggle(currentSelectFunction == GridFunction.Player, content = new GUIContent(GridFunction.Player.ToString()), "Button", GUILayout.Height(20)))
            {
                currentSelectFunction = GridFunction.Player;
                currentSelectPrefab = PrefabConfig.GetPrefab(currentSelectFunction.ToString());
                if (currentSelectTool == SceneTool.Brush)
                {
                    brush.CleanPrefabGo();
                }
            }

            // Monster
            if (GUILayout.Toggle(currentSelectFunction == GridFunction.Monster, content = new GUIContent(GridFunction.Monster.ToString()), "Button", GUILayout.Height(20)))
            {
                currentSelectFunction = GridFunction.Monster;
                currentSelectPrefab = PrefabConfig.GetPrefab(currentSelectFunction.ToString());
                if (currentSelectTool == SceneTool.Brush)
                {
                    brush.CleanPrefabGo();
                }
            }
            EditorGUILayout.EndHorizontal();

            // 资源库预览界面
            DrawModelView();
        }

        /// <summary>
        /// 画资源库物体预览展示
        /// </summary>
        private void DrawModelView()
        {
            modelViewScrollPosition = EditorGUILayout.BeginScrollView(modelViewScrollPosition);

            modelViewHorizontalCounter = 0;

            modelViewColumn = (int)(Inst.position.width / 200f);
            if (modelViewColumn <= 1)
            {
                modelViewColumn = 1;
            }

            EditorGUILayout.BeginHorizontal();

            using (prefabGoEnumerator = currentSelectPrefab.gos.GetEnumerator())
            {
                while (prefabGoEnumerator.MoveNext())
                {
                    var prefabGo = prefabGoEnumerator.Current.Value;

                    EditorGUILayout.BeginVertical();

                    // 模型预览
                    Texture2D previewImage = AssetPreview.GetAssetPreview(prefabGo.go);
                    content = new GUIContent(previewImage);
                    // 选中状态
                    bool selected = false;
                    if (currentSelectPrefabGo != null)
                    {
                        if (currentSelectPrefabGo.name == prefabGo.go.name)
                        {
                            selected = true;
                        }
                    }

                    bool isSelected = GUILayout.Toggle(selected, content, GUI.skin.button);
                    if (isSelected && editorEnabled)
                    {
                        currentSelectPrefabGo = prefabGo;
                        // 选中，设置笔刷
                        ChangeSceneTool(SceneTool.Brush);
                        brush.SetPrefabGo(currentSelectPrefabGo);
                    }

                    EditorGUILayout.BeginHorizontal("Box");
                    EditorGUILayout.LabelField(prefabGo.go.name);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();

                    modelViewHorizontalCounter++;
                    if (modelViewHorizontalCounter == modelViewColumn)
                    {
                        modelViewHorizontalCounter = 0;
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                    }
                }
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
        }

        void OnSceneGUI(SceneView sceneView)
        {
            if (!editorEnabled)
                return;

            // 计算鼠标在Grid上的位置
            CalculateSceneMousePosition();
            
            controlId = GUIUtility.GetControlID(FocusType.Passive);
            
            // 画选择工具栏
            DrawToolbar(sceneView);
            
            switch (currentSelectTool)
            {
                case SceneTool.Selector:
                    selector.DrawScenePreview(sceneView, mousePosition);
                    break;
                case SceneTool.Brush:
                    brush.DrawScenePreview(sceneView, mousePosition);
                    break;
                case SceneTool.Sucker:
                    sucker.DrawScenePreview(sceneView, mousePosition);
                    break;
                case SceneTool.Erase:
                    erase.DrawScenePreview(sceneView, mousePosition);
                    break;
            }

            HandleSceneInput();

            HandleUtility.AddDefaultControl(controlId);
        }

        /// <summary>
        /// 场景按键输入
        /// </summary>
        private void HandleSceneInput()
        {
            switch (currentSelectTool)
            {
                case SceneTool.Selector:
                    selector.HandleInput(mousePosition);
                    break;
                case SceneTool.Brush:
                    brush.HandleInput(mousePosition);
                    break;
                case SceneTool.Sucker:
                    sucker.HandleInput(mousePosition);
                    break;
                case SceneTool.Erase:
                    erase.HandleInput(mousePosition);
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
            content = new GUIContent(IconConfig.GetIconTexture("iconCursor"));
            if (GUI.Toggle(new Rect(LESetting.SceneTooIX, LESetting.SceneTooIY, LESetting.SceneToolSize, LESetting.SceneToolSize), currentSelectTool == SceneTool.Selector, content, GUI.skin.button))
            {
                brush.Close();
                sucker.Close();
                erase.Close();
                currentSelectTool = SceneTool.Selector;
            }
            
            // 笔刷工具
            content = new GUIContent(IconConfig.GetIconTexture("iconBlockMode"));
            if (GUI.Toggle(new Rect(LESetting.SceneTooIX, LESetting.SceneTooIY + nextInterval, LESetting.SceneToolSize, LESetting.SceneToolSize), currentSelectTool == SceneTool.Brush, content, GUI.skin.button))
            {
                selector.Close();
                sucker.Close();
                erase.Close();
                currentSelectTool = SceneTool.Brush;
            }

            // 吸管工具
            content = new GUIContent(IconConfig.GetIconTexture("iconPicker"));
            if (GUI.Toggle(new Rect(LESetting.SceneTooIX, LESetting.SceneTooIY + nextInterval * 2, LESetting.SceneToolSize, LESetting.SceneToolSize), currentSelectTool == SceneTool.Sucker, content, GUI.skin.button))
            {
                selector.Close();
                brush.Close();
                erase.Close();
                currentSelectTool = SceneTool.Sucker;
            }

            // 擦除工具
            content = new GUIContent(IconConfig.GetIconTexture("iconErase"));
            if (GUI.Toggle(new Rect(LESetting.SceneTooIX, LESetting.SceneTooIY + nextInterval * 3, LESetting.SceneToolSize, LESetting.SceneToolSize), currentSelectTool == SceneTool.Erase, content, GUI.skin.button))
            {
                selector.Close();
                brush.Close();
                sucker.Close();
                currentSelectTool = SceneTool.Erase;
            }

            #endregion

            #region 左下角
           
            // 上一区域
            content = new GUIContent(IconConfig.GetIconTexture("iconArrowUp"));
            if (GUI.Button(new Rect(LESetting.SceneTooIX, downY, LESetting.SceneToolSize, LESetting.SceneToolSize / 2), content))
            {
                _area++;
            }

            // 下一区域
            content = new GUIContent(IconConfig.GetIconTexture("iconArrowDown"));
            if (GUI.Button(new Rect(LESetting.SceneTooIX, downY + LESetting.SceneToolSize / 2, LESetting.SceneToolSize, LESetting.SceneToolSize / 2), content))
            {
                _area--;

                if (_area <= 0)
                {
                    _area = 1;
                }
            }

            GUILayout.BeginArea(new Rect(LESetting.SceneTooIX + nextInterval, downY, 50, LESetting.SceneToolSize), EditorStyles.textArea);
            {
                GUILayout.BeginVertical();
                GUILayout.Label("Area", EditorStyles.label, GUILayout.Width(50));
                GUILayout.Label(_area.ToString(), EditorStyles.label, GUILayout.Width(50));
                GUILayout.EndVertical();
            }
            GUILayout.EndArea();

            #endregion

            #region 右下角

            // Show Gizmo Panel Base
            content = new GUIContent(IconConfig.GetIconTexture("iconIsolate"));
            gizmoPanel.BaseEnabled = GUI.Toggle(new Rect(rightX, downY, LESetting.SceneToolSize, LESetting.SceneToolSize), gizmoPanel.BaseEnabled, content, GUI.skin.button);


            // GizmoPanelUp
            content = new GUIContent(IconConfig.GetIconTexture("iconGridUp"));
            if (GUI.Toggle(new Rect(rightX - nextInterval, downY, LESetting.SceneToolSize, LESetting.SceneToolSize), false, content, GUI.skin.button))
            {
                currentSelectTool = SceneTool.None;
                _gridHeight++;

                gizmoPanel.SetHight(_gridHeight);

                Repaint();
            }

            // GizmoPanelDown
            content = new GUIContent(IconConfig.GetIconTexture("iconGridDown"));
            if (GUI.Toggle(new Rect(rightX - nextInterval * 2, downY, LESetting.SceneToolSize, LESetting.SceneToolSize), false, content, GUI.skin.button))
            {
                currentSelectTool = SceneTool.None;
                _gridHeight--;

                gizmoPanel.SetHight(_gridHeight);

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
                Vector3 shiftOffset = gizmoPanel.transform.position;
                shiftOffset.x = shiftOffset.x - (int)shiftOffset.x;
                shiftOffset.y = shiftOffset.y - (int)shiftOffset.y;
                shiftOffset.z = shiftOffset.z - (int)shiftOffset.z;

                mousePosition.x = Mathf.Round(((hit.point.x + shiftOffset.x) - hit.normal.x * 0.001f) / 1) * 1 - shiftOffset.x;
                mousePosition.z = Mathf.Round(((hit.point.z + shiftOffset.z) - hit.normal.z * 0.001f) / 1) * 1 - shiftOffset.z;
                mousePosition.y = _gridHeight + gizmoPanel.transform.position.y;

                if (gizmoPanelState == GizmoPanelState.Exit)
                {
                    gizmoPanelState = GizmoPanelState.Enter;
                    OnGizmoPanelState();
                }
            }
            else {
                if (gizmoPanelState == GizmoPanelState.Enter)
                {
                    gizmoPanelState = GizmoPanelState.Exit;
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
                    selector.HaneleGizmoPanelState(gizmoPanelState);
                    break;
                case SceneTool.Brush:
                    brush.HaneleGizmoPanelState(gizmoPanelState);
                    break;
                case SceneTool.Sucker:
                    sucker.HaneleGizmoPanelState(gizmoPanelState);
                    break;
                case SceneTool.Erase:
                    erase.HaneleGizmoPanelState(gizmoPanelState);
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
            currentSelectPrefabGo = grid.prefabGo;
            brush.SetPrefabGo(currentSelectPrefabGo);
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
