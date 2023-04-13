using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraseConnection : MonoBehaviour
{
    public bool erasing = false;
    private void Awake()
    {
        //erasing = false;
    }
    public void eraseActivate()
    {
        erasing = true;
    }

    public void eraseDeactivate()
    {
        erasing = false;
    }
}
