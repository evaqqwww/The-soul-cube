using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class NGUIPanelAttribute : Attribute
{

    private String _name;

    private String _parentPanelName;
    private bool _ignoreScale;

    public NGUIPanelAttribute()
    {

    }

    public NGUIPanelAttribute(String name)
        : this(name, String.Empty)
    {
    }


    public NGUIPanelAttribute(String name, bool ignoreScale)
        : this(name, String.Empty, ignoreScale)
    {

    }

    public NGUIPanelAttribute(String name, String parentPanelName)
        : this(name, parentPanelName, false)
    {

    }

    public NGUIPanelAttribute(String name, String parentPanelName, bool ignoreScale)
    {
        this._name = name;

        this._parentPanelName = parentPanelName;
        this._ignoreScale = ignoreScale;
    }

    public String GetName()
    {
        return this._name;
    }

    public String GetParentPanelName()
    {
        return this._parentPanelName;
    }

    public bool IsIgnoreScale()
    {
        return _ignoreScale;
    }
}

