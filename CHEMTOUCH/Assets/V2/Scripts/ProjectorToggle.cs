using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class ProjectorToggle : MonoBehaviour
{

    public GameObject objt;
    bool active = false;
    public bool startActive = true;

    public void Start()
    {
        if (startActive)
        {
            toggle();
        }
    }
    public void toggle()
    {
        objt.SetActive(!active);
        active = !active;
    }
    
    public void activate()
    {
        objt.SetActive(true);
        active = true;
    }

}
