using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EraseChecker : MonoBehaviour
{
    
    [SerializeField] Material eraseMaterial;
    [SerializeField] Material noMaterial;


    public bool isInMove = true;

    // Start is called before the first frame update
    public void enableErase()
    {
        //Debug.Log("Hi");
        GameObject worldObjects = GameObject.Find("WorldObjects");
        foreach (Transform go in worldObjects.transform)
        {
            if (go.tag == "Atom")
            {
                go.GetComponent<XRGrabInteractable>().interactionLayerMask = 0;
            }
            if (go.tag == "Mollecule")
            {
                go.GetComponent<XRGrabInteractable>().interactionLayerMask = 0;
                foreach (Transform c in go.transform)
                {
                    if (c.gameObject.tag == "Connection")
                    {
                        if (c.GetComponent<ConnectionProperties>().canEnable())
                        {
                            foreach (Transform m in c.Find("Materials").transform)
                            {
                                m.GetComponent<MeshRenderer>().material = eraseMaterial;
                            }
                            c.gameObject.GetComponentInChildren<XRSimpleInteractable>().interactionLayerMask = 1;
                        }
                    }
                }
            }           
        }
    }
    public void enableMove()
    {
        //Debug.Log("Hi");
        GameObject worldObjects = GameObject.Find("WorldObjects");
        foreach (Transform go in worldObjects.transform)
        {
            if (go.tag == "Atom")
            {
                go.GetComponent<XRGrabInteractable>().interactionLayerMask = 1;
            }
            if (go.tag == "Mollecule")
            {
                
                go.GetComponent<XRGrabInteractable>().interactionLayerMask = 1;
                foreach (Transform c in go.transform)
                {
                    if (c.gameObject.tag == "Connection")
                    {
                            foreach (Transform m in c.Find("Materials").transform)
                            {
                                m.GetComponent<MeshRenderer>().material = noMaterial;
                            }
                            c.gameObject.GetComponentInChildren<XRSimpleInteractable>().interactionLayerMask = 0;
                       
                    }
                }
            }
        }
        isInMove = true;
    }
}
