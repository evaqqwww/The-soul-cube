using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UIMgr
{

    private static UIMgr _it;

    public static UIMgr It
    {
        get
        {
            if (_it == null)
                _it = new UIMgr();
            return _it;
        }
    }

    //初始UI界面
    public static Type[] awakePanelClasses = new[]
    {   
        typeof(LogoPanel),
        typeof(LevelSelectionPanel),
    };

    //跨视图UI
    public static Type[] commonPanelClasses = new[]
    {
        typeof(LoadingPanel),
    };

    //主界面UI（动态）
    public static Type[] mainPanelClasses = new[]
    {
        typeof(MainPanel),
    };


    GameObject _anchorObj;

    Dictionary<string, IBasePanel> _panels = new Dictionary<string, IBasePanel>();

    bool _bPanelsHide = false;

    public UIMgr()
    {

    }

    public void Init(GameObject anchorObj)
    {
        _anchorObj = anchorObj;
        InitAwakeUIs();
    }

    void InitAwakeUIs()
    {
        InitPanels(awakePanelClasses);
    }

    // 跨视图存在的界面
    public void InitCommonUIs()
    {
        InitPanels(commonPanelClasses);
    }

    //主界面
    public void InitMainUIs()
    {
        InitPanels(mainPanelClasses);
    }


    void InitPanels(Type[] types)
    {
        for (int i = 0; i < types.Length; i++)
        {
            Type t = types[i];
            LoadUI(t);
        }
    }

    private string TryGetUIName<T>()
            where T : IBasePanel
    {
        Type panelType = typeof(T);

        return TryGetUIName(panelType);
    }

    private string TryGetUIName(Type panelType)
    {
        if (panelType == null)
            return string.Empty;
        if (!typeof(IBasePanel).IsAssignableFrom(panelType))
            return string.Empty;

        if (!Attribute.IsDefined(panelType, typeof(NGUIPanelAttribute)))
        {
            throw new InvalidCastException(String.Format("This is unavaiable panel: {0}", panelType.Name));
        }

        NGUIPanelAttribute attribute =
            Attribute.GetCustomAttribute(panelType, typeof(NGUIPanelAttribute)) as NGUIPanelAttribute;
        if (String.IsNullOrEmpty(attribute.GetName()))
        {
            throw new InvalidCastException(String.Format("This is unavaiable panel: {0}, there is no name, add NGUIPanel before class.", panelType.Name));
        }

        return attribute.GetName();
    }


    public IBasePanel LoadUI<T>() where T : IBasePanel
    {
        Type panelType = typeof(T);

        return LoadUI(panelType);
    }

    public IBasePanel LoadUI(Type panelType)
    {
        String name = TryGetUIName(panelType);

        IBasePanel one;
        if (_panels.TryGetValue(name, out one))
            return one;
        one = System.Activator.CreateInstance(panelType) as IBasePanel;
        one.PanelName = name;
        if (one == null)
            return null;
        _panels[name] = one;
        try
        {
            one.AddToStage(_anchorObj);
            // 动态面板等到第一个事件处理才创建
            if (!one.IsDynamicLoadPanel())
                one.Init();
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
        return one;
    }

    public IBasePanel LoadAndGetOrgUI<T>()
            where T : IBasePanel
    {
        return LoadUI<T>().OriginalPanel;
    }

    public String[] GetAllUINames()
    {
        String[] names = new string[_panels.Keys.Count];
        _panels.Keys.CopyTo(names, 0);
        return names;
    }

    public IBasePanel GetUI(string name)
    {
        if (string.IsNullOrEmpty(name)) return null;
        IBasePanel one;
        if (_panels.TryGetValue(name, out one))
        {
            return one;
        }
        return null;
    }

    public void UnloadAllUI(bool bSkipCommon = true)
    {
        String[] uiNames = GetAllUINames();
        foreach (string uiName in uiNames)
        {
            //if (bSkipCommon)
            //{
            //    if (commonUINames.Contains(uiName))
            //        continue;
            //}
            UnloadUI(uiName);
        }
    }

    public void UnloadUI(string name)
    {
        IBasePanel one;
        if (_panels.TryGetValue(name, out one))
        {
            //if (one != null && this.CurrentPanel != null && one == this.CurrentPanel)
            //{
            //    this.CurrentPanel = null;
            //}

            one.RemoveFromStage();
            _panels.Remove(name);
        }
    }

    public void UnloadUI<T>() where T : IBasePanel
    {
        UnloadUI(typeof(T));
    }

    public void UnloadUI(Type panelType)
    {
        String name = TryGetUIName(panelType);

        UnloadUI(name);
        //IBasePanel one;
        //if (_panels.TryGetValue(name, out one))
        //{
        //    one.RemoveFromStage();
        //    _panels.Remove(name);
        //}
    }

    public void ShowUI(string name)
    {
        IBasePanel panel = GetUI(name);
        if (panel == null)
            return;

        PushUI(panel);
    }

    public void HideUI(string name)
    {
        IBasePanel panel = GetUI(name);
        if (panel == null)
            return;

        panel.HidePanel();

    }

    public void CloseUI(string name)
    {
        IBasePanel panel = GetUI(name);
        if (panel == null)
            return;

        //Debug.Log(String.Format("Close ({0})", name));
        //panel.HidePanel();
        PopUI(panel);
    }

    public void PushUI(IBasePanel panel)
    {
        if (panel.IsShow) return;
        panel.ShowPanel();

        //待堆栈需求
    }

    public void PopUI(IBasePanel panel)
    {
        panel.HidePanel();

        //待堆栈需求
    }

}
