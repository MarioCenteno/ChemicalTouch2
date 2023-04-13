using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CheckHit : MonoBehaviour
{
    private Transform l_hand_position;
    private Transform r_hand_position;
    private GameObject molleculePoint;
    // Start is called before the first frame update

    private void Start()
    {
        l_hand_position = GameObject.Find("LeftHandController").transform;
        r_hand_position = GameObject.Find("RightHandController").transform;
    }
    public void check()
    {
        RaycastHit hit;
        if (Physics.SphereCast(r_hand_position.position, 0.1f, transform.forward, out hit, 0.01f))
        {
            if (hit.transform.gameObject.tag == "Atom" && hit.transform.parent!=null) {
                molleculePoint = hit.transform.parent.Find("ConnectionPoint").gameObject;
                molleculePoint.transform.position = hit.transform.position;
                hit.transform.parent.GetComponent<XRGrabInteractable>().attachTransform = hit.transform;
            }
            
        }
    }
}
