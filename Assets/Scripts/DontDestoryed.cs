using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DontDestoryed : MonoBehaviour
{
    public bool bOnly = true;
    private bool _bInit = false;


    // Use this for initialization
    void Start()
    {
        if (bOnly && _bInit)
        {
            GameObject.Destroy(gameObject);
            return;
        }
        _bInit = true;

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}