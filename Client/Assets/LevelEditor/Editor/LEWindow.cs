using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SU.Editor.LevelEditor
{
    /// <summary>
    /// 住面板页签菜单
    /// </summary>
    public enum TabMenus
    {
        Function,              // 功能模块
        Group,                  // 分组设置模块
    }

    /// <summary>
    /// 场景工具类型
    /// </summary>
    public enum SceneTools
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
        public static LERepositoryConfig RepositoryConfig;

        // GizmoPanel
        private LEGizmoPanel gizmoPanel;
        private Vector2Int gridDimensions;
        private Vector2Int _gridDimensions;
        private int gridHeight;
        private int gridGroud = 1;

        // 当前网格的高度
        public int GridHeight {
            get {
                return gridHeight;
            }
        }

        // 当前画格子所在地
        public int GridGroud
        {
            get
            {
                return gridGroud;
            }
        }

        // 是否开启编辑模式
        private bool editorEnabled = false;
        private bool _editorEnabled = true;

        // 当前选择的资源库
        private LERepository repository;
        // 资源库资源预览列表
        private Vector2 modelViewScrollPosition;
        private int modelViewHorizontalCounter;
        private int modelViewColumn;

        // 当前 Tab 菜单
        private TabMenus currentSelectTabMenu = TabMenus.Function;
        // 当前选择的工具
        private SceneTools currentSelectTool;
        // 当前选择的功能
        public GridFunctions currentSelectFunction = GridFunctions.Ground;
        // 当前选中的仓库模型
        private LERepositoryAsset currentModel;
        // 当前鼠标所在GizmoGrid上的位置
        private Vector3 mousePosition;

        // 编辑器编辑的关卡场景名称
        private string levelSceneName;

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

            levelSceneName = LELevel.Inst.levelName;

            // 网格实例获取
            if (gizmoPanel == null)
            {
                var gizmoGridGo = GameObject.Find(LEConst.EditorGizmoGridPanelName);
                if (gizmoGridGo != null)
                {
                    gizmoPanel = gizmoGridGo.GetComponent<LEGizmoPanel>();

                    gridDimensions.x = gizmoPanel.GridWidth;
                    gridDimensions.y = gizmoPanel.GridDepth;

                    _gridDimensions.x = gizmoPanel.GridWidth;
                    _gridDimensions.y = gizmoPanel.GridDepth;

                    gridHeight = gizmoPanel.GridHeight;
                }
            }

            // 关卡初始化
            LELevel.Inst.Initialize();

            // config
            IconConfig = AssetDatabase.LoadAssetAtPath(LEConst.IconConfigPath, typeof(LEIconConfig)) as LEIconConfig;
            RepositoryConfig = AssetDatabase.LoadAssetAtPath(LEConst.RepositoryConfigPath, typeof(LERepositoryConfig)) as LERepositoryConfig;
            RepositoryConfig.Initialize();

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

                    gizmoPanel.SetGridSize(gridDimensions.x, gridDimensions.y);
                }

                EditorGUILayout.EndVertical();

                // 网格高度设置
                EditorGUILayout.BeginVertical("box");
                GUILayout.Label("Grid Height: " + gridHeight.ToString());
                EditorGUILayout.EndVertical();

                #region tab menu

                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.BeginHorizontal();
                // function tab menu
                if (GUILayout.Toggle(currentSelectTabMenu == TabMenus.Function, content = new GUIContent(TabMenus.Function.ToString()), "Button", GUILayout.Height(30)))
                {
                    currentSelectTabMenu = TabMenus.Function;
                }

                // group tab menu
                if (GUILayout.Toggle(currentSelectTabMenu == TabMenus.Group, content = new GUIContent(TabMenus.Group.ToString()), "Button", GUILayout.Height(30)))
                {
                    currentSelectTabMenu = TabMenus.Group;
                }
                EditorGUILayout.EndHorizontal();

                // 画功能库预览
                if (currentSelectTabMenu == TabMenus.Function)
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
            if (GUILayout.Toggle(currentSelectFunction == GridFunctions.Ground, content = new GUIContent(GridFunctions.Ground.ToString()), "Button", GUILayout.Height(20)))
            {
                currentSelectFunction = GridFunctions.Ground;
                repository = RepositoryConfig.GetRepository(currentSelectFunction.ToString());
                if (currentSelectTool == SceneTools.Brush)
                {
                    brush.SetModel(null);
                }
            }

            // Door
            if (GUILayout.Toggle(currentSelectFunction == GridFunctions.Door, content = new GUIContent(GridFunctions.Door.ToString()), "Button", GUILayout.Height(20)))
            {
                currentSelectFunction = GridFunctions.Door;
                repository = RepositoryConfig.GetRepository(currentSelectFunction.ToString());
                if (currentSelectTool == SceneTools.Brush)
                {
                    brush.SetModel(null);
                }
            }

            // Transfer
            if (GUILayout.Toggle(currentSelectFunction == GridFunctions.Transfer, content = new GUIContent(GridFunctions.Transfer.ToString()), "Button", GUILayout.Height(20)))
            {
                currentSelectFunction = GridFunctions.Transfer;
                repository = RepositoryConfig.GetRepository(currentSelectFunction.ToString());
                if (currentSelectTool == SceneTools.Brush)
                {
                    brush.SetModel(null);
                }
            }

            // Trap
            if (GUILayout.Toggle(currentSelectFunction == GridFunctions.Trap, content = new GUIContent(GridFunctions.Trap.ToString()), "Button", GUILayout.Height(20)))
            {
                currentSelectFunction = GridFunctions.Trap;
                repository = RepositoryConfig.GetRepository(currentSelectFunction.ToString());
                if (currentSelectTool == SceneTools.Brush)
                {
                    brush.SetModel(null);
                }
            }

            // Player
            if (GUILayout.Toggle(currentSelectFunction == GridFunctions.Player, content = new GUIContent(GridFunctions.Player.ToString()), "Button", GUILayout.Height(20)))
            {
                currentSelectFunction = GridFunctions.Player;
                repository = RepositoryConfig.GetRepository(currentSelectFunction.ToString());
                if (currentSelectTool == SceneTools.Brush)
                {
                    brush.SetModel(null);
                }
            }

            // Monster
            if (GUILayout.Toggle(currentSelectFunction == GridFunctions.Monster, content = new GUIContent(GridFunctions.Monster.ToString()), "Button", GUILayout.Height(20)))
            {
                currentSelectFunction = GridFunctions.Monster;
                repository = RepositoryConfig.GetRepository(currentSelectFunction.ToString());
                if (currentSelectTool == SceneTools.Brush)
                {
                    brush.SetModel(null);
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

            for (int i = 0; i < repository.assets.Length; i++)
            {
                EditorGUILayout.BeginVertical();

                // 模型预览
                Texture2D previewImage = AssetPreview.GetAssetPreview(repository.assets[i].asset);
                content = new GUIContent(previewImage);
                // 选中状态
                bool selected = false;
                if (currentModel != null)
                {
                    if (currentModel.assetName == repository.assets[i].assetName)
                    {
                        selected = true;
                    }
                }

                bool isSelected = GUILayout.Toggle(selected, content, GUI.skin.button);
                if (isSelected)
                {
                    currentModel = repository.assets[i];
                    // 选中，设置笔刷
                    brush.SetModel(currentModel);
                    ChangeSceneTool(SceneTools.Brush);
                }

                EditorGUILayout.BeginHorizontal("Box");
                EditorGUILayout.LabelField(repository.assets[i].assetName);
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
                case SceneTools.Selector:
                    selector.DrawScenePreview(sceneView, mousePosition);
                    break;
                case SceneTools.Brush:
                    brush.DrawScenePreview(sceneView, mousePosition);
                    break;
                case SceneTools.Sucker:
                    sucker.DrawScenePreview(sceneView, mousePosition);
                    break;
                case SceneTools.Erase:
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
                case SceneTools.Selector:
                    selector.HandleInput(mousePosition);
                    break;
                case SceneTools.Brush:
                    brush.HandleInput(mousePosition);
                    break;
                case SceneTools.Sucker:
                    sucker.HandleInput(mousePosition);
                    break;
                case SceneTools.Erase:
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
            if (GUI.Toggle(new Rect(LESetting.SceneTooIX, LESetting.SceneTooIY, LESetting.SceneToolSize, LESetting.SceneToolSize), currentSelectTool == SceneTools.Selector, content, GUI.skin.button))
            {
                brush.Close();
                sucker.Close();
                erase.Close();
                currentSelectTool = SceneTools.Selector;
            }
            
            // 笔刷工具
            content = new GUIContent(IconConfig.GetIconTexture("iconBlockMode"));
            if (GUI.Toggle(new Rect(LESetting.SceneTooIX, LESetting.SceneTooIY + nextInterval, LESetting.SceneToolSize, LESetting.SceneToolSize), currentSelectTool == SceneTools.Brush, content, GUI.skin.button))
            {
                selector.Close();
                sucker.Close();
                erase.Close();
                currentSelectTool = SceneTools.Brush;
            }

            // 吸管工具
            content = new GUIContent(IconConfig.GetIconTexture("iconPicker"));
            if (GUI.Toggle(new Rect(LESetting.SceneTooIX, LESetting.SceneTooIY + nextInterval * 2, LESetting.SceneToolSize, LESetting.SceneToolSize), currentSelectTool == SceneTools.Sucker, content, GUI.skin.button))
            {
                selector.Close();
                brush.Close();
                erase.Close();
                currentSelectTool = SceneTools.Sucker;
            }

            // 擦除工具
            content = new GUIContent(IconConfig.GetIconTexture("iconErase"));
            if (GUI.Toggle(new Rect(LESetting.SceneTooIX, LESetting.SceneTooIY + nextInterval * 3, LESetting.SceneToolSize, LESetting.SceneToolSize), currentSelectTool == SceneTools.Erase, content, GUI.skin.button))
            {
                selector.Close();
                brush.Close();
                sucker.Close();
                currentSelectTool = SceneTools.Erase;
            }

            #endregion

            #region 左下角
           
            // 上一地块
            content = new GUIContent(IconConfig.GetIconTexture("iconArrowUp"));
            if (GUI.Button(new Rect(LESetting.SceneTooIX, downY, LESetting.SceneToolSize, LESetting.SceneToolSize / 2), content))
            {
                gridGroud++;
            }

            // 下一地块
            content = new GUIContent(IconConfig.GetIconTexture("iconArrowDown"));
            if (GUI.Button(new Rect(LESetting.SceneTooIX, downY + LESetting.SceneToolSize / 2, LESetting.SceneToolSize, LESetting.SceneToolSize / 2), content))
            {
                gridGroud--;

                if (gridGroud <= 0)
                {
                    gridGroud = 1;
                }
            }

            GUILayout.BeginArea(new Rect(LESetting.SceneTooIX + nextInterval, downY, 50, LESetting.SceneToolSize), EditorStyles.textArea);
            {
                GUILayout.BeginVertical();
                GUILayout.Label("Groud", EditorStyles.label, GUILayout.Width(50));
                GUILayout.Label(gridGroud.ToString(), EditorStyles.label, GUILayout.Width(50));
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
                currentSelectTool = SceneTools.None;
                gridHeight++;

                gizmoPanel.SetGridHight(gridHeight);

                Repaint();
            }

            // GizmoPanelDown
            content = new GUIContent(IconConfig.GetIconTexture("iconGridDown"));
            if (GUI.Toggle(new Rect(rightX - nextInterval * 2, downY, LESetting.SceneToolSize, LESetting.SceneToolSize), false, content, GUI.skin.button))
            {
                currentSelectTool = SceneTools.None;
                gridHeight--;

                gizmoPanel.SetGridHight(gridHeight);

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
                mousePosition.y = gridHeight + gizmoPanel.transform.position.y;

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
                case SceneTools.Selector:
                    selector.HaneleGizmoPanelState(gizmoPanelState);
                    break;
                case SceneTools.Brush:
                    brush.HaneleGizmoPanelState(gizmoPanelState);
                    break;
                case SceneTools.Sucker:
                    sucker.HaneleGizmoPanelState(gizmoPanelState);
                    break;
                case SceneTools.Erase:
                    erase.HaneleGizmoPanelState(gizmoPanelState);
                    break;
            }
        }

        /// <summary>
        /// 切换场景工具
        /// </summary>
        /// <param name="tool"></param>
        public void ChangeSceneTool(SceneTools tool)
        {
            currentSelectTool = tool;
        }
        
    }
}
