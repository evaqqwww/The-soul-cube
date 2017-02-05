using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(MovePlatform),true)]
public class MovePlatformEditor : Editor
{
    eTowards _towards;
    
    void OnEnable()
    {
        SerializedProperty towards = serializedObject.FindProperty("_towards");

        _towards = (eTowards)towards.enumValueIndex;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

#if UNITY_EDITOR
        SerializedProperty dis = serializedObject.FindProperty("dis");
        EditorGUILayout.PropertyField(dis);
        SerializedProperty time = serializedObject.FindProperty("time");
        EditorGUILayout.PropertyField(time);

        _towards = (eTowards)EditorGUILayout.EnumPopup(_towards, "DropDown", GUILayout.Width(100.0f));
        SerializedProperty towards = serializedObject.FindProperty("_towards");
        towards.enumValueIndex = (int)_towards;
       
        switch (_towards)
        {
            case eTowards.up:
                SetForwardSide(false, 1);
                break;
            case eTowards.down:
                SetForwardSide(false, -1);
                break;
            case eTowards.left:
                SetForwardSide(true, -1);
                break;
            case eTowards.right:
                SetForwardSide(true, 1);
                break;
            default:
                break;
        }
        
#endif
    }

    private void SetForwardSide(bool side,int value)
    {
        EditorUtility.SetDirty(target);
       
        MovePlatform move = (MovePlatform)target;
        move.SetMoveToward(side, value);
        serializedObject.ApplyModifiedProperties();

    }
         
}
