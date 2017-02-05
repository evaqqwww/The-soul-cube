using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class LevelMgrEditor : Editor
{

    [CustomEditor(typeof(LevelMgr))]
    [InitializeOnLoad]

    public class LevelManagerEditor : Editor
    {
        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
        static void DrawGameObjectName(LevelMgr levelManager, GizmoType gizmoType)
        {
            GUIStyle style = new GUIStyle();
            Vector3 v3FrontTopLeft;

            if (levelManager.levelBounds.size != Vector3.zero)
            {
                style.normal.textColor = Color.yellow;
                v3FrontTopLeft = new Vector3(levelManager.levelBounds.center.x - levelManager.levelBounds.extents.x, 
                                             levelManager.levelBounds.center.y + levelManager.levelBounds.extents.y + 0.5f, 
                                             levelManager.levelBounds.center.z - levelManager.levelBounds.extents.z);
                Handles.Label(v3FrontTopLeft, "Level Bounds", style);

                BoundsUtil.DrawHandlesBounds(levelManager.levelBounds, Color.yellow);

            }
        }
    }
}
