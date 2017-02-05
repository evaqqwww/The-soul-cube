using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public interface IBasePanel
{

    bool IsDynamicLoadPanel();
    
    GameObject GetPrefab();

    IBasePanel OriginalPanel { get; }

    string PanelName { get; set; }

    void Init();

    void AddToStage(GameObject anchorobj);
    string GetRegisterName();
    void UnloadTexture();
    void RemoveFromStage();
    void OnUpdate(float delta);
    void OnBackRestore();
    void OnShowing();
    void OnHidding();
    void Show();
    void Hide();
    void OnShown();
    void OnHidden();
    void ShowPanel();
    void HidePanel();
    bool IsShow { get; set; }
    Transform PanelTransform { get; }
    void SetPos(Vector2 pos);
    void SetCenterPos(Vector2 pos);
   
    void Close();
}


public abstract class NGUIBasePanel : IBasePanel
{


    protected string _panelRes;

    protected GameObject _panelParent;

    protected Transform _panelObjT;

    protected UIPanel _panel;

    protected Vector3 _localPosition = Vector3.zero;

    protected Vector3 _localScale;

    protected Transform _anchorBottomT;
    protected Transform _anchorBottomLeftT;
    protected Transform _anchorBottomRightT;
    protected Transform _anchorCenterT;
    protected Transform _anchorLeftT;
    protected Transform _anchorRightT;
    protected Transform _anchorTopT;
    protected Transform _anchorTopLeftT;
    protected Transform _anchorTopRightT;


    public IBasePanel OriginalPanel
    {
        get
        {
            return this;
        }
    }

    public string PanelName
    {
        get;
        set;
    }

    #region DynamicLoadPanel

    bool _dynamicLoadPanel;
    bool _panelLoaded;

    public bool IsDynamicLoadPanel() { return _dynamicLoadPanel; }
    private void SetPanelLoaded(bool bSet) { _panelLoaded = bSet; }

    protected bool IsPanelLoaded() { return _panelLoaded; }

    PanelEventsForwarder _dynamicPanelEventsForwarder;

    #endregion

    public NGUIBasePanel(string res,bool dynamicLoad=false)
    {
        _panelRes = res;
        _localScale = Vector3.one;
        _panelLoaded = false;
        
        _dynamicLoadPanel = dynamicLoad;
        if (_dynamicLoadPanel)
        {
            InitDynamicLoadPanel();
        } 
    }

    private void InitDynamicLoadPanel()
    {
        if (!_dynamicLoadPanel)
            return;
        _dynamicPanelEventsForwarder = new PanelEventsForwarder();
        _dynamicPanelEventsForwarder.Init(this);
    }

    private void ReleaseDynamicLoadPanel()
    {
        if (!_dynamicLoadPanel)
            return;
        _dynamicPanelEventsForwarder.Release();
    }

    public virtual GameObject GetPrefab()
    {
        return Resources.Load(_panelRes) as GameObject;
    }


    public virtual void AddToStage(GameObject panelGo)
    {
        _panelParent = panelGo;
        if (IsDynamicLoadPanel())
        {
            DL_RegisterEvents();
            return;
        }
        InstantiatePanel(panelGo);
    }

    protected void InstantiatePanel(GameObject panelGo)
    {
        SetPanelLoaded(true);

        string[] names = _panelRes.Split('/');

        string objectName = names[names.Length - 1];

        _panelObjT = panelGo.transform.FindChild(objectName);

        if (_panelObjT == null)
        {
            GameObject _prefabRes = GetPrefab();
            if (_panelRes == null)
                Debug.LogError("This panelPath is null!!!");
            _panelObjT = MonoBehaviour.Instantiate(_prefabRes.transform) as Transform;
            Vector3 localPos = _panelObjT.position;
            Vector3 localScale = _panelObjT.localScale;

            _panelObjT.parent = panelGo.transform;
            _panelObjT.gameObject.name = objectName;
            _panelObjT.localScale = localScale;
            _panelObjT.localPosition = localPos;
            _panel = _panelObjT.GetComponent<UIPanel>();
            _prefabRes = null;
        }

        UIAnchor[] anchors = _panelObjT.GetComponentsInChildren<UIAnchor>();
        if (anchors != null && anchors.Length > 0)
        {
            foreach (UIAnchor uiAnchor in anchors)
            {
                uiAnchor.uiCamera = NGUITools.FindCameraForLayer(10);
            }
        }

        this._localScale = _panelObjT.localScale;
        _localPosition = _panelObjT.localPosition;
        Transform anchorTransform = _panelObjT.FindChild("Anchor_bottom/Offset");
        if (anchorTransform != null)
            _anchorBottomT = anchorTransform;
        anchorTransform = _panelObjT.FindChild("Anchor_bottomleft/Offset");
        if (anchorTransform != null)
            _anchorBottomLeftT = anchorTransform;
        anchorTransform = _panelObjT.FindChild("Anchor_bottomright/Offset");
        if (anchorTransform != null)
            _anchorBottomRightT = anchorTransform;
        anchorTransform = _panelObjT.FindChild("Anchor_center/Offset");
        if (anchorTransform != null)
            _anchorCenterT = anchorTransform;
        anchorTransform = _panelObjT.FindChild("Anchor_left/Offset");
        if (anchorTransform != null)
            _anchorLeftT = anchorTransform;
        anchorTransform = _panelObjT.FindChild("Anchor_right/Offset");
        if (anchorTransform != null)
            _anchorRightT = anchorTransform;
        anchorTransform = _panelObjT.FindChild("Anchor_top/Offset");
        if (anchorTransform != null)
            _anchorTopT = anchorTransform;
        anchorTransform = _panelObjT.FindChild("Anchor_topleft/Offset");
        if (anchorTransform != null)
            _anchorTopLeftT = anchorTransform;
        anchorTransform = _panelObjT.FindChild("Anchor_topright/Offset");
        if (anchorTransform != null)
            _anchorTopRightT = anchorTransform;
    }


    public virtual void Init()
    {
        
    }

    protected void bindEventListener(bool bind, string eventName,
            EventDispatcher.ObjectDelegate callback, bool strongEvent = true)
    {
        if (!this.IsDynamicLoadPanel())
        {
            GlobalEvents.It.events.bindEventListener(bind, eventName, callback);
        }
        else
        {
            if (_dynamicPanelEventsForwarder != null)
                _dynamicPanelEventsForwarder.Bind(bind, eventName,
                    callback, strongEvent);
        }
    }

    public void ForwardEvent(
            HEvent e,
            EventDispatcher.ObjectDelegate callback,
            bool strongEvent)
    {
        if (!this.IsDynamicLoadPanel())
            return;
        if (strongEvent)
        {
            DL_ValidPanel();
            callback(e);
        }
        else
        {
            if (!this.IsPanelLoaded())
                return;
            DL_ValidPanel();
            callback(e);
        }
    }

    protected void DL_ValidPanel()
    {
        if (!IsDynamicLoadPanel())
            return;
        if (IsPanelLoaded())
            return;
        try
        {
            InstantiatePanel(_panelParent);
            Init();
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }

    protected virtual void DL_RegisterEvents()
    {
    }

    public void DL_Release()
    {
        if (!IsDynamicLoadPanel())
            return;
        DestroyPanel();
        DL_OnRelease();
    }

    protected virtual void DL_OnRelease()
    {
    }

    public virtual string GetRegisterName()
    {
        string panelName = GetType().FullName;
        panelName = panelName.Replace("NGUI.", "");
        return panelName;
    }

    public virtual void UnloadTexture()
    {
        
    }

    public virtual void RemoveFromStage()
    {
        if (IsShow)
        {
            try
            {
                OnHidden();
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
        }
        DestroyPanel();
        ReleaseDynamicLoadPanel();
    }

    protected void DestroyPanel()
    {
        if (!IsPanelLoaded())
            return;
        
        if (_panel != null)
            _panel.enabled = true;

        if (_panelObjT != null)
        {
            GameObject go = _panelObjT.gameObject;
            go.name = "__" + go.name;
            GameObject.DestroyImmediate(_panelObjT.gameObject);
        }

        SetPanelLoaded(false);
    }

    public void OnUpdate(float delta)
    {
        
    }

    public void OnBackRestore()
    {
        DL_ValidPanel();
        
    }

    #region UIMgr全局调用

    public void ShowPanel()
    {
        this.IsShow = true;
        
        this.OnShown();
    }

    public void HidePanel()
    {
        if (!IsPanelLoaded())
            return;
        
        this.IsShow = false;

        this.OnHidden();
    }

    #endregion

    public virtual void OnShowing()
    {
        //_perfCtrl.Restore();
        //if (_dynamicLifeCtrl != null)
        //    _dynamicLifeCtrl.OnShowing();
    }

    public virtual void OnHidding()
    {
        
    }

    public virtual void Show()
    {
        DL_ValidPanel();
        UIMgr.It.ShowUI(this.PanelName);

    }

    public virtual void Close()
    {
        UIMgr.It.CloseUI(this.PanelName);
    }

    public virtual void Hide()
    {
        UIMgr.It.HideUI(this.PanelName);
    }

    public void OnShown()
    {
        
    }

    public virtual void OnHidden()
    {
        //_perfCtrl.Release();
        //if (_dynamicLifeCtrl != null)
        //    _dynamicLifeCtrl.OnHidden();
    }

    
    public bool IsShow
    {
        get
        {
            if (!IsPanelLoaded())
                return false;
            return _panelParent != null && _panel != null && _panel.enabled;
        }
        set
        {
            if (value)
            {
                if (_panel != null && _panel.enabled) return;
                
                //if (_panel != null && _panel._fullScreen)
                //{
                //    if (_panel.GetComponent<BoxCollider>() == null)
                //    {
                //        BoxCollider box = _panel.gameObject.AddComponent<BoxCollider>();
                //        box.center = new Vector3(0, 0, 10);
                //        box.size = new Vector3(Screen.width * 2, Screen.height * 2, 1);
                //    }
                //}

                if (_panel != null)
                {
                    _panel.enabled = true;
                    _panel.gameObject.SetActive(true);
                }

                if (_panelObjT != null)
                    _panelObjT.gameObject.SetActive(true);

                OnShowing();
            }
            else
            {
                if (_panel != null && !_panel.enabled) return;
                
                if (_panel != null)
                {
                    _panel.enabled = false;
                    _panel.gameObject.SetActive(false);
                }
                if (_panelObjT != null)
                    _panelObjT.gameObject.SetActive(false);
            }
        }
    }

    public Transform PanelTransform
    {
        get
        {
            return _panelObjT;
        }
    }

    public void SetPos(Vector2 pos)
    {
        
    }

    public void SetCenterPos(Vector2 pos)
    {
        
    }

    
}

