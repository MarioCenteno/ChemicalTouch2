using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ConnectionProperties : MonoBehaviour
{
    public XRDirectInteractor l_hand;
    public XRDirectInteractor r_hand;
    [SerializeField] int numConnection;
    private GameObject leftAtom;
    private GameObject leftConnection;
    private GameObject rightAtom;
    private GameObject rightConnection;
    private GameObject GameManager;
    // Start is called before the first frame update

    private void Start()
    {
        GameManager = GameObject.Find("GameManager");
        l_hand = GameObject.Find("LeftHandController").GetComponent<XRDirectInteractor>();
        r_hand = GameObject.Find("RightHandController").GetComponent<XRDirectInteractor>();
    }
    public void setLeftAtom(GameObject leftAtom)
    {
        this.leftAtom = leftAtom;
    }
    public void setRightAtom(GameObject rightAtom)
    {
        this.rightAtom = rightAtom;
    }
    public void setLeftAtomConnections(GameObject leftAtomConnections)
    {
        this.leftConnection = leftAtomConnections;
    }
    public void setRightAtomConnections(GameObject rightAtomConnections)
    {
        this.rightConnection = rightAtomConnections;
    }

    public GameObject getLeftAtom()
    {
        return this.leftAtom;
    }
    public GameObject getRightAtom()
    {
        return this.rightAtom;
    }
    public GameObject getLeftAtomConnections()
    {
        return this.leftConnection;
    }
    public GameObject getRightAtomConnections()
    {
        return this.rightConnection;
    }

    public bool canEnable()
    {
        return leftAtom.GetComponent<Atom>().isSolo() || rightAtom.GetComponent<Atom>().isSolo();
    }

    public void disableConnection(GameObject collider)
    {
       collider.GetComponent<XRSimpleInteractable>().interactionLayerMask = 0;
    }

    public void deleteConnection()
    {
        GameManager.GetComponent<DeleteConnection>().deleteConnection(getLeftAtom(), getRightAtom(),getLeftAtomConnections(),getRightAtomConnections(), this.gameObject);
    }
   
}
