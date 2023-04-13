using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class MollecularConnection : MonoBehaviour
{

    public XRDirectInteractor l_hand;
    public XRDirectInteractor r_hand;
    public Text l_UI_connection;
    public Text r_UI_connection;

    public GameObject TrashCan;

    public GameObject connectionPrefab;
    //public GameObject DoubleConnection_prefab;
    //public GameObject TripleConnection_prefab;

    public GameObject molleculePrefab;
    public GameObject atomPrefab;

    private GameObject l_atom;
    private GameObject r_atom;
    private float Distance;
    private bool isconnecting;
    private bool isdisconnecting;
    private bool setupHands;
    private int l_n_connections;
    private int r_n_connections;
    private GameObject ParentMollecule;
    private GameObject l_atom_p;
    private GameObject r_atom_p;

    private GameObject l_atom_electrons;
    private int TimeRemaining;
    private GameObject r_atom_electrons;
    private GameObject l_atom_connections;
    private GameObject r_atom_connections;
    private GameObject connection;
    private GameObject connection2;
    private GameObject connection3;
    private bool hasconnection;
    private bool leftatomhand;
    private bool rightatomhand;
    private bool molleculeintanciated;
    private GameObject simpleconnection;
    // Start is called before the first frame update
    void Start()
    {
        isconnecting = false;
        isdisconnecting = false;
        l_n_connections = 0;
        r_n_connections = 0;
        setupHands = false;
        hasconnection = false;
        leftatomhand = false;
        rightatomhand = false;
        molleculeintanciated = false;
        simpleconnection =null;
        ParentMollecule = null;

        TimeRemaining = 1000;
}

    // Update is called once per frame
    void Update()
    {
        DestroyTime();

        if (l_hand.selectTarget != null && r_hand.selectTarget != null && l_hand.selectTarget.tag == "Grabable" && r_hand.selectTarget.tag == "Grabable")
        {
            if (!setupHands)
            {
                
                l_UI_connection.gameObject.SetActive(true);
                r_UI_connection.gameObject.SetActive(true);
                l_atom = l_hand.selectTarget.gameObject;
                r_atom = r_hand.selectTarget.gameObject;

                r_atom_electrons = r_atom.transform.Find("eletrons").gameObject;
                l_atom_electrons =l_atom.transform.Find("eletrons").gameObject;
                if(r_atom_electrons.transform.childCount>1)
                    r_atom_connections = r_atom.transform.Find("connections").gameObject;
                if (l_atom_electrons.transform.childCount>1)
                    l_atom_connections = l_atom.transform.Find("connections").gameObject;
               

                setupHands = true;
            }
            Distance = Vector3.Distance(l_atom.transform.position, r_atom.transform.position);
            if (Distance < 0.15 && !isconnecting && l_n_connections<l_atom_electrons.transform.childCount && r_n_connections< r_atom_electrons.transform.childCount)
            {
                isconnecting = true;
                l_n_connections++;
                r_n_connections++;
                l_UI_connection.text = l_n_connections.ToString();
                if (l_n_connections == l_atom_electrons.transform.childCount)
                    l_UI_connection.color = Color.red;
                r_UI_connection.text = r_n_connections.ToString();
                if (r_n_connections == r_atom_electrons.transform.childCount)
                    r_UI_connection.color = Color.red;
            }
            else if (Distance > 1.3 && !isdisconnecting)
            {
                isdisconnecting = true;
                l_n_connections = 0;
                r_n_connections = 0;
                l_UI_connection.color = Color.white;
                r_UI_connection.color = Color.white;
                l_UI_connection.text = l_n_connections.ToString();
                r_UI_connection.text = l_n_connections.ToString();
                hasconnection = false;
            }
            else if (Distance > 0.3 && Distance < 1.0 && (isconnecting==true || isdisconnecting==true))
            {
                isconnecting = false;
                isdisconnecting = false;
                

            }



        }
        else if (((l_hand.selectTarget != null && l_hand.selectTarget.tag == "Grabable") || (r_hand.selectTarget != null && r_hand.selectTarget.tag == "Grabable")) && (l_n_connections > 0 && r_n_connections > 0))
        {
            /* if (!r_atom.name.Contains("Mollecule") && !l_atom.name.Contains("Mollecule"))
             {*/
                 if (!molleculeintanciated)
                 {
                     ParentMollecule = Instantiate(molleculePrefab, (l_hand.transform.position+r_hand.transform.position)/2, Quaternion.identity);

            //    GameObject l_atom_m = l_atom;
            //    GameObject r_atom_m = r_atom;

                ParentMollecule.GetComponent<XRGrabInteractable>().colliders.Add(r_atom.GetComponent<SphereCollider>());
                ParentMollecule.GetComponent<XRGrabInteractable>().colliders.Add(l_atom.GetComponent<SphereCollider>());
               
                l_atom.transform.parent = ParentMollecule.transform;
                r_atom.transform.parent = ParentMollecule.transform;
                
                GameObject NPM = ParentMollecule;
               

                Instantiate(NPM,(l_hand.transform.position+r_hand.transform.position)/2, Quaternion.identity);
                molleculeintanciated = true;
                //simpleconnection = Instantiate(connectionPrefab, NPM.transform.position, Quaternion.LookRotation(NPM.transform.GetChild(0).position - NPM.transform.GetChild(1).position),NPM.transform);

                ParentMollecule.transform.parent = TrashCan.transform;
                ParentMollecule.transform.position = TrashCan.transform.position;
                l_atom.transform.parent = TrashCan.transform;
                 l_atom.transform.position = TrashCan.transform.position;
                 r_atom.transform.parent = TrashCan.transform;
                 r_atom.transform.position = TrashCan.transform.position;

            }







            /*else if (r_atom.name.Contains("Mollecute") && !l_atom.name.Contains("Mollecule"))
                l_atom.transform.parent = r_atom.transform;
            else if (!r_atom.name.Contains("Mollecute") && l_atom.name.Contains("Mollecule"))
                r_atom.transform.parent = l_atom.transform;
            else if (r_atom.name.Contains("Mollecute") && l_atom.name.Contains("Mollecule"))
            {
                if (r_atom.transform.childCount > l_atom.transform.childCount)
                {
                    for (int i = 0; i < l_atom.transform.childCount; i++)
                        l_atom.transform.GetChild(i).parent = r_atom.transform;
                    Destroy(l_atom);
                }
                else
                {
                    for (int i = 0; i < r_atom.transform.childCount; i++)
                        r_atom.transform.GetChild(i).parent = l_atom.transform;
                    Destroy(r_atom);
                }
            }*/
            /*if (l_hand.selectTarget != null)
                ParentMollecule.GetComponent<XRGrabInteractable>().attachTransform = l_hand.selectTarget.transform;
            else if (r_hand.selectTarget !=null)
                ParentMollecule.GetComponent<XRGrabInteractable>().attachTransform = r_hand.selectTarget.transform;
            */
            /*if (l_hand.selectTarget != null)
            {

                hasconnection = false;

                if (l_hand.selectTarget.gameObject == l_atom)
                {
                    if (!hasconnection)
                    {

                        if (l_atom.transform.childCount<3)
                        {
                            simpleconnection = Instantiate(connectionPrefab,
                            l_atom_electrons.transform.GetChild(0).transform.position,
                            Quaternion.LookRotation(l_atom_electrons.transform.GetChild(0).position
                            - l_atom.transform.position), l_atom.transform);
                        }
                        l_atom.transform.parent = null;
                        simpleconnection.name = "l_atom_connection";
                        r_atom.transform.parent = l_atom.transform;
                        //leftatomhand = true;
                        //rightatomhand = false;
                        r_atom.transform.position = simpleconnection.transform.GetChild(0).position;
                        //r_atom.transform.GetChild(0).transform.localPosition = l_atom.transform.localPosition;
                        //r_atom_electrons.transform.GetChild(0).position = l_atom_electrons.transform.GetChild(0).position;
                        //Vector3 currentrotation = (r_atom_electrons.transform.GetChild(0).position - r_atom.transform.position).normalized;
                        //Vector3 newRotation = Vector3.RotateTowards(currentrotation,l_atom.transform.position,10*Time.deltaTime,0.0f);
                        //r_atom.transform.rotation = Quaternion.LookRotation(newRotation);
                        FixedJoint joint = simpleconnection.transform.GetChild(1).GetComponent<FixedJoint>();
                        joint.connectedBody = r_atom.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Rigidbody>();
                        //Debug.Log(joint.connectedBody.name);
                        Destroy(r_atom.transform.GetChild(2).gameObject);

                        hasconnection = true;
                    }








                }
                else if (l_hand.selectTarget.gameObject == r_atom)
                {
                    if (!hasconnection)
                    {
                        if (r_atom.transform.childCount<3) {
                            simpleconnection = Instantiate(connectionPrefab,
                            r_atom_electrons.transform.GetChild(0).transform.position,
                            Quaternion.LookRotation(r_atom_electrons.transform.GetChild(0).position
                            - r_atom.transform.position), r_atom.transform);
                            simpleconnection.name = "r_atom_connection";
                        }

                        r_atom.transform.parent = null;
                        l_atom.transform.parent = r_atom.transform;
                        //leftatomhand = false;
                        //rightatomhand = true;
                        l_atom.transform.position = simpleconnection.transform.GetChild(0).position;
                        //l_atom.transform.GetChild(0).transform.localPosition = r_atom.transform.localPosition;
                        //r_atom_electrons.transform.GetChild(0).position = l_atom_electrons.transform.GetChild(0).position;
                        //Vector3 currentrotation = l_atom_electrons.transform.GetChild(0).position - l_atom.transform.position;
                        //Vector3 newRotation = Vector3.RotateTowards(currentrotation, r_atom.transform.position, 1000, 0.0f);
                        //l_atom.transform.rotation = Quaternion.LookRotation(newRotation); 
                        FixedJoint joint = simpleconnection.transform.GetChild(1).GetComponent<FixedJoint>();
                        joint.connectedBody = l_atom.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Rigidbody>();
                        Destroy(l_atom.transform.GetChild(2).gameObject);
                        hasconnection = true;
                    }



                }


                    /*
                    FixedJoint joint = simpleconnection.GetComponent<FixedJoint>();
                    if (joint != null)
                    {
                        joint.connectedBody = r_atom.GetComponent<Rigidbody>();
                        hasconnection = true;
                    }
                    else
                    {
                        joint = new FixedJoint();
                        joint.transform.position = simpleconnection.transform.GetChild(0).position;
                        joint.connectedBody = r_atom.GetComponent<Rigidbody>();
                        hasconnection = true;
                    }



                }

            else
            {
                if (!hasconnection)
                {

                    var simpleconnection = Instantiate(connectionPrefab,
                    r_atom_electrons.transform.GetChild(0).position,
                    Quaternion.LookRotation(r_atom_electrons.transform.GetChild(0).position
                    - r_atom.transform.position), r_atom.transform);

                    l_atom.transform.parent = r_atom.transform;
                    l_atom.transform.position = simpleconnection.transform.GetChild(0).position;
                    l_atom.transform.rotation = Quaternion.LookRotation(r_atom_electrons.transform.GetChild(0).position
                    - l_atom_electrons.transform.GetChild(0).position);
                    hasconnection = true;
                }
            }*/
        }

        else
        {
            if (setupHands)
            {
               
                l_UI_connection.color = Color.white;
                r_UI_connection.color = Color.white;
                l_UI_connection.text = l_n_connections.ToString();
                r_UI_connection.text = r_n_connections.ToString();
                l_UI_connection.gameObject.SetActive(false);
                r_UI_connection.gameObject.SetActive(false);
                setupHands = false;
                hasconnection = false;
                //ParentMollecule = null;
            }
            return;

        }

    }


    public void DestroyTime()
    {
        if (TimeRemaining > 0)
        {
            TimeRemaining -= 1;
        }
        else
        {
            for (int i=0; i<TrashCan.transform.childCount;i++)
            {
                    Destroy(TrashCan.transform.GetChild(i).gameObject);
            }
            TimeRemaining = 1000;
            Debug.Log("Cleanup");
        }
    }
}


