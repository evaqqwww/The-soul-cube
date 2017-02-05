using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;

public class SystemUtil 
{
    public static List<Type> GetAllClass<T>() where T : class
    {
        List<Type> ret = new List<Type>();
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (System.Type type in assembly.GetExportedTypes())
            {
                if (((typeof(T).IsAssignableFrom(type) && type.IsClass) && !type.IsAbstract))
                {
                    ret.Add(type);
                }
            }
        }
        return ret;
    }

    public static T AddOrGetComponent<T>(Transform root, string path = null)
            where T : Component
    {
        Transform tran;
        if (string.IsNullOrEmpty(path))
            tran = root;
        else
            tran = root.FindChild(path);
        if (tran == null)
            return null;
        T t = tran.GetComponent<T>();
        if (t != null)
            return t;
        return tran.gameObject.AddComponent<T>();
    }

    public static RaycastHit RayCast(Vector3 rayOriginPoint, Vector3 rayDirection, float rayDistance, LayerMask mask, Color color, bool drawGizmo = false)
    {
        if (drawGizmo)
        {
            Debug.DrawRay(rayOriginPoint, rayDirection * rayDistance, color);
        }
        RaycastHit hit;
        Physics.Raycast(rayOriginPoint, rayDirection, out hit, rayDistance, mask);
        return hit;
    }
	
}
