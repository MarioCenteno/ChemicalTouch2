using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAtom : MonoBehaviour
{
    private bool hasScaled;
    private bool noParent;
    private GameObject worldObjects;
    [SerializeField] Vector3 scale;
    [SerializeField] GameObject parentAll;
    
    // Start is called before the first frame update
    void Start()
    {
        noParent = false;      
        hasScaled = false;
        worldObjects = GameObject.Find("WorldObjects");
    }

    // Update is called once per frame


    public void updateAtomScale()
    {
        
        if (!hasScaled)
        {
            this.transform.localScale = scale;
            hasScaled = true;
        }        
    }

    public void updateAtomParent()
    {
        if (!noParent)
        {
            this.transform.SetParent(worldObjects.transform);
            this.transform.tag = "Atom";
            noParent = true;
        }
    }
}
