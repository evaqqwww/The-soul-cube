using UnityEngine;


public class Singleton<T> : MonoBehaviour where T : Component
{
    protected static T _it;

 
    public static T It
    {
        get
        {
            if (_it == null)
            {
                _it = FindObjectOfType<T>();
                if (_it == null)
                {
                    GameObject obj = new GameObject();

                    _it = obj.AddComponent<T>();
                }
            }
            return _it;
        }
    }

    
    protected virtual void Awake()
    {
        _it = this as T;
    }
}

