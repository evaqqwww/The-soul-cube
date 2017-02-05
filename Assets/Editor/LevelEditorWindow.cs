using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 实时显示场景中的对象
/// </summary>
public class LevelEditorWindow : EditorWindow
{

    private GameObject[] _cubes;
    private Transform Cube
    {
        get
        {
            _cubes = GameObject.FindGameObjectsWithTag("cube");
            if (_cubes.Length == 1)
                return _cubes[0].transform;
            else
            {
                for (int i = 0; i < _cubes.Length; i++)
                {
                    if (_cubes[i].activeInHierarchy)
                        return _cubes[i].transform;
                }
            }
            Debug.LogError("No cube is active!!!");
            return null;
        }
    }

    private List<Transform> _sides = new List<Transform>();

    private Color[] randomColor = new Color[6] { Color.blue, Color.green, Color.magenta, Color.red, Color.white, Color.yellow };

    private List<Transform> Sides
    {
        get
        {
            _sides.Clear();
            for (int i = 0; i < 6; i++)
            {
                _sides.Add(Cube.FindChild("side" + i).transform);
            }

            return _sides;
        }
    }

    private CubeState _cubeState;
    private CubeState CubeState
    {
        get
        {
            _cubeState = Cube.GetComponent<CubeState>();
            return _cubeState;
        }
    }

    private string assetPath = @"Assets\Resources\Prefabs\DemoStandard\mapNode.prefab";

    //下拉菜单相关属性
    int _curShowSide = 0;
    int _curAngles = 0;
    
    string[] _popSides = {"0", "1", "2", "3", "4", "5" };
    string[] _sideAngles = { "90", "180" };
    
    //地图快捷编辑
    RaycastHit _hitInfo;
    SceneView.OnSceneFunc _delegate;
    static LevelEditorWindow _windowInstance;
    private List<Rect> _mapRectList = new List<Rect>();        //地块rect集合
    private List<Rect> MapRectList
    {
        get
        {
            if (_mapRectList.Count == 0)
            {
                InitMapRects();
            }
            return _mapRectList;
        }
    }

    private GameObject[] mSelectObjs = null;

    [MenuItem("LevelEditor/Scene Editor")]
    static void Init()
    {
        if (_windowInstance == null)
        {
            _windowInstance = EditorWindow.GetWindow(typeof(LevelEditorWindow)) as LevelEditorWindow;
            _windowInstance._delegate = new SceneView.OnSceneFunc(OnSceneFunc);
            SceneView.onSceneGUIDelegate += _windowInstance._delegate;
        }
    }

    void OnEnable()
    {
    }

    void OnDisable()
    {
    }

    void OnDestroy()
    {
        if (_delegate != null)
        {
            SceneView.onSceneGUIDelegate -= _delegate;
        }
    }

    void OnSelectionChange()
    {
        mSelectObjs = Selection.gameObjects;
    }

    void OnGUI()
    {
        
        if (GUILayout.Button("初始化地图块"))
            MakeInitMaps();
        EditorGUILayout.HelpBox("场景初始化后将不被再执行！！！，操作双Cube的场景，注意时刻保持单个方块的显示！！！", MessageType.Warning);

        
        EditorGUILayout.LabelField("根据序号选择显示的平面", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        _curShowSide = EditorGUILayout.Popup(_curShowSide, _popSides, EditorStyles.popup,GUILayout.Width(100.0f));
        if (GUILayout.Button("选择", GUILayout.Width(100.0f)))
            SelectCurSide();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("旋转当前平面", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        _curAngles = EditorGUILayout.Popup(_curAngles, _sideAngles, EditorStyles.popup, GUILayout.Width(100.0f));
        if (GUILayout.Button("旋转", GUILayout.Width(100.0f)))
            RotationCurSide();
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("编辑当前选择的平面"))
            EditSingle();
        EditorGUILayout.HelpBox("选择的平面转换到0面的位置，其他平面隐藏，编辑结束后点击“生成游戏立方体”按钮！！！", MessageType.Info);
        if (GUILayout.Button("生成游戏立方体"))
            MakeGameCube();
        EditorGUILayout.HelpBox("生成游戏立方体，并显示所有平面！！！", MessageType.Info);
        
    }

    void OnInspectorGUI()
    {
        Debug.Log("OnInspectorGUI");
    }

    static public void OnSceneFunc(SceneView sceneView)
    {
        _windowInstance.CustomSceneGUI(sceneView);
    }

    void SelectCurSide()
    {

        switch (_curShowSide)
        {
            case 0:
                Quaternion angle0 = Quaternion.Euler(Vector3.zero);
                Cube.localRotation = angle0;
                Selection.activeGameObject = Cube.FindChild("side0").gameObject;
                break;
            case 1:
                Quaternion angle1 = Quaternion.Euler(new Vector3(0, 90.0f, 0));
                Cube.localRotation = angle1;
                Selection.activeGameObject = Cube.FindChild("side1").gameObject;
                break;
            case 2:
                Quaternion angle2 = Quaternion.Euler(new Vector3(0, 180.0f, 0));
                Cube.localRotation = angle2;
                Selection.activeGameObject = Cube.FindChild("side2").gameObject;
                break;
            case 3:
                Quaternion angle3 = Quaternion.Euler(new Vector3(0, 270.0f, 0));
                Cube.localRotation = angle3;
                Selection.activeGameObject = Cube.FindChild("side3").gameObject;
                break;
            case 4:
                Quaternion angle4 = Quaternion.Euler(new Vector3(-90.0f, 0, 0));
                Cube.localRotation = angle4;
                Selection.activeGameObject = Cube.FindChild("side4").gameObject;
                break;
            case 5:
                Quaternion angle = Quaternion.Euler(new Vector3(90.0f, 0, 0));
                Cube.localRotation = angle;
                Selection.activeGameObject = Cube.FindChild("side5").gameObject;
                break;
            default:
                break;
        }
    }

    void RotationCurSide()
    {
        switch (int.Parse(_sideAngles[_curAngles]))
        {
            case 90:
                Cube.Rotate(Vector3.forward, 90.0f, Space.World);
                break;
            case 180:
                Cube.Rotate(Vector3.forward, 180.0f, Space.World);
                break;
            default:
                break;
        }
    }

    void MakeInitMaps()
    {
        if (CubeState.initCompleted)
            return;
        for (int side = 0; side < 6; side++)
        {
            for (int row = 0; row < 16; row++)
            {
                for (int col = 0; col < 16; col++)
                {
                    MakeOoneMap(side, new Vector3(col, row, 0));
                }
            }
        }
        CubeState.initCompleted = true;
        MakeGameCube();
    }

    //生成单个地块
    void MakeOoneMap(int side, Vector3 pos, bool isInit = true)
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;
        if (prefab == null)
        {
            Debug.Log("prefab is null!!!");
            return;
        }
        GameObject instance = GameObject.Instantiate(prefab) as GameObject;
        instance.transform.FindChild("map").GetComponent<SpriteRenderer>().color = randomColor[side];
        if (isInit)
            instance.transform.position += pos;
        else
        {
            Vector3 mapPos=Vector3.zero;
            if (ReturnMapRectPos(pos, out mapPos))
            {
                instance.transform.position = mapPos;
            }
            else
            {
                GameObject.DestroyImmediate(instance);
                Debug.LogError("destory");
            }
        }
        
        instance.transform.SetParent(Sides[side]);
    }


    //初始化地块rect集合
    void InitMapRects()
    {
        for (int row = 0; row < 16; row++)
        {
            for (int col = 0; col < 16; col++)
            {
                Vector3 initialPos = new Vector3(-7.5f, -7.5f, 0);
                initialPos += new Vector3(col, row, 0);
                Rect rect = new Rect(Vector2.zero, Vector2.one);
                rect.center = initialPos;
                _mapRectList.Add(rect);
            }
        }
    }


    //判断当前鼠标点的地块位置
    bool ReturnMapRectPos(Vector3 curPos, out Vector3 mapPos)
    {
        for (int i = 0; i < MapRectList.Count; i++)
        {
            if (!MapRectList[i].Contains(curPos,true))
                continue;
            mapPos = MapRectList[i].center;
            return true;
        }
        mapPos = Vector3.zero;
        return false;
    }


    void EditSingle()
    {
        if (mSelectObjs.Length == 0)
        {
            Debug.Log("No selected objects!!!");
            return;
        }
        if (mSelectObjs.Length > 1)
        {
            Debug.Log("Select multiple objects!!!");
            return;
        }
        for (int i = 0; i < 6; i++)
        {
            if (Sides[i].Equals(mSelectObjs[0].transform))
            {
                Quaternion angle = Quaternion.Euler(Vector3.zero);
                Sides[i].localRotation = angle;
                Sides[i].localPosition = new Vector3(0, 0, -0.5f);
                Sides[i].gameObject.SetActive(true);
                CubeState.curSide = i;
                continue;
            }
            Sides[i].gameObject.SetActive(false);
        }

    }


    void MakeGameCube()
    {
        for (int i = 0; i < 6; i++)
        {
            Sides[i].gameObject.SetActive(true);
            switch (i)
            {
                case 0:
                    Sides[i].localPosition = new Vector3(0, 0, -0.5f);
                    break;
                case 1:
                    Quaternion angle1 = Quaternion.Euler(new Vector3(0, -90.0f, 0));
                    Sides[i].localRotation = angle1;
                    Sides[i].localPosition = new Vector3(0.5f, 0, 0);
                    break;
                case 2:
                    Quaternion angle2 = Quaternion.Euler(new Vector3(0, -180.0f, 0));
                    Sides[i].localRotation = angle2;
                    Sides[i].localPosition = new Vector3(0, 0, 0.5f);
                    break;
                case 3:
                    Quaternion angle3 = Quaternion.Euler(new Vector3(0, 90.0f, 0));
                    Sides[i].localRotation = angle3;
                    Sides[i].localPosition = new Vector3(-0.5f, 0, 0);
                    break;
                case 4:
                    Quaternion angle4 = Quaternion.Euler(new Vector3(90.0f, 0f, 0));
                    Sides[i].localRotation = angle4;
                    Sides[i].localPosition = new Vector3(0, 0.5f, 0);
                    break;
                case 5:
                    Quaternion angle5 = Quaternion.Euler(new Vector3(-90.0f, 0f, 0));
                    Sides[i].localRotation = angle5;
                    Sides[i].localPosition = new Vector3(0, -0.5f, 0);
                    break;
                default:
                    break;
            }
        }
    }


    void CustomSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;
        if (!e.control && !e.capsLock)
            return;
        //Camera cameara = sceneView.camera;
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        

        if (e.isMouse && e.type == EventType.MouseDown)
        {
            if (Physics.Raycast(ray, out _hitInfo, 1000, -1))
            {
                //Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
                //Vector3 origin = _hitInfo.point;
                if (e.control)
                    GameObject.DestroyImmediate(_hitInfo.collider.gameObject);
            }
            else
            {
                if (e.capsLock)
                    MakeOoneMap(CubeState.curSide, ray.origin, false);
            }
        }

        SceneView.RepaintAll();
    }


}