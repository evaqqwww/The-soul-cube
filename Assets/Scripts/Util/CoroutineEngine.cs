using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CoroutineEngine : MonoBehaviour
{

    public static CoroutineEngine instance;

    public void Awake()
    {
        instance = this;
    }

}
