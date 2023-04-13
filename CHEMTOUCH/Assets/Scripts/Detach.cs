using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Detach : MonoBehaviour
{
   
    public Connect connectScript;
    private GameObject worldObjects;
    [SerializeField] GameObject molleculePrefab;
    [SerializeField] GameObject simplePrefab;
    [SerializeField] GameObject doublePrefab;
    [SerializeField] GameObject triplePrefab;
    private GameObject MollecularSpanwer;
    [SerializeField] Atom atomCharacteristics;


    private HistoryChange historyScript;

    private GameObject atomOne=null;



    // Start is called before the first frame update



    private void Start()
    {

        worldObjects = GameObject.Find("WorldObjects");
        historyScript = GameObject.Find("GameManager").GetComponent<HistoryChange>();
        MollecularSpanwer = GameObject.Find("XR Rig").transform.GetChild(0).Find("LeftHandController").Find("Molecular Spawner").gameObject;
    }

    // Update is called once per frame
    public void onDetach()
    {

        if (connectScript.ishitting && connectScript.hitObject.GetComponent<Atom>().numeletroes>0 && connectScript.hitObject.GetComponent<Connect>().closestConnection!=null)
        {
            //r_hand.enabled = false;
            //l_hand.enabled = false;
            if (connectScript.isMollecule == true)
            {
                this.gameObject.transform.parent = connectScript.hitObject.transform.parent;
                this.gameObject.GetComponent<XRGrabInteractable>().interactionLayerMask = 0;
                connectAtoms(this.gameObject, connectScript.hitObject, connectScript.hitObject.transform.parent);
                this.gameObject.GetComponent<Atom>().numConnections++;
                connectScript.hitObject.GetComponent<Atom>().numConnections++;

            }
            else if (this.gameObject.transform.parent != worldObjects.transform)
            {
                connectScript.hitObject.transform.parent = this.gameObject.transform.parent;

                connectScript.hitObject.GetComponent<XRGrabInteractable>().interactionLayerMask = 0;
                connectAtoms(connectScript.hitObject, this.gameObject, this.gameObject.transform.parent);
                this.gameObject.GetComponent<Atom>().numConnections++;
                connectScript.hitObject.GetComponent<Atom>().numConnections++;
            }
            else
            {
                GameObject mollecule = Instantiate(molleculePrefab, this.gameObject.transform.position, Quaternion.identity);
                this.gameObject.transform.parent = mollecule.transform;
                connectScript.hitObject.transform.parent = mollecule.transform;
                mollecule.transform.parent = worldObjects.transform;
                connectAtoms(this.gameObject, connectScript.hitObject, this.connectScript.hitObject.transform.parent);
                this.gameObject.GetComponent<Atom>().numConnections++;
                connectScript.hitObject.GetComponent<Atom>().numConnections++;
                this.gameObject.GetComponent<XRGrabInteractable>().interactionLayerMask = 0;
                connectScript.hitObject.GetComponent<XRGrabInteractable>().interactionLayerMask = 0;

            }
            deactivatemyconnections(this.GetComponent<Connect>().numConnection, this.connectScript.currentConnections);
            deactivateotherconnections(connectScript.hitObject.GetComponent<Connect>().closestConnection, connectScript.hitObject.GetComponent<Connect>().currentConnections,false);
            this.connectScript.currentConnections = 0;
            connectScript.hitObject.GetComponent<Connect>().currentConnections = 0;
            //r_hand.enabled = true;
           // l_hand.enabled = true;

        }
        StartCoroutine(ActivateSpawner());
       
        //historyScript.addToHistory(worldObjects);


    }

    IEnumerator ActivateSpawner()
    {
        yield return new WaitForSeconds(0.2f);
        MollecularSpanwer.SetActive(true);

    }


    public void connectAtoms(GameObject atom1, GameObject atom2, Transform parent)
    {
        int connectionsWanted = this.connectScript.numConnection;
        if (connectionsWanted == 0)
            return;
        else if (connectionsWanted == 1)
        {
            if (this.gameObject.GetComponent<Atom>().maxeletroes == 1)
            {
                int atoms2remaining = atom2.GetComponent<Atom>().maxeletroes - atom2.GetComponent<Atom>().numeletroes;
                GameObject connection = Instantiate(simplePrefab, atom2.GetComponent<Connect>().closestConnection.transform.position, Quaternion.LookRotation(atom2.GetComponent<Connect>().closestConnection.transform.position - atom2.transform.position), parent);
                connection.GetComponent<ConnectionProperties>().setLeftAtom(atom1);
                connection.GetComponent<ConnectionProperties>().setRightAtom(atom2);
                connection.GetComponent<ConnectionProperties>().setLeftAtomConnections(atom1.GetComponent<Atom>().simples[0]);
                connection.GetComponent<ConnectionProperties>().setRightAtomConnections(atom2.GetComponent<Connect>().closestConnection);
                atom1.GetComponent<Atom>().addConnectingAtoms(atom2.GetComponent<Atom>().GetAtomType(), 1);
                atom2.GetComponent<Atom>().addConnectingAtoms(atom1.GetComponent<Atom>().GetAtomType(), GetConnectionNumber(atom2.GetComponent<Connect>().closestConnection.name));
                atom1.transform.position = connection.transform.GetChild(0).position;
                


            }
            else
            {
                int atoms1remaining = atom1.GetComponent<Atom>().maxeletroes - atom1.GetComponent<Atom>().numeletroes;
                int atoms2remaining = atom2.GetComponent<Atom>().maxeletroes - atom2.GetComponent<Atom>().numeletroes;
                GameObject connection = Instantiate(simplePrefab, atom2.GetComponent<Connect>().closestConnection.transform.position, Quaternion.LookRotation(atom2.GetComponent<Connect>().closestConnection.transform.position - atom2.transform.position), parent);
                connection.GetComponent<ConnectionProperties>().setLeftAtom(atom1);
                connection.GetComponent<ConnectionProperties>().setRightAtom(atom2);
                connection.GetComponent<ConnectionProperties>().setLeftAtomConnections(atom1.GetComponent<Atom>().simples[0]);
                connection.GetComponent<ConnectionProperties>().setRightAtomConnections(atom2.GetComponent<Connect>().closestConnection);
                atom1.GetComponent<Atom>().addConnectingAtoms(atom2.GetComponent<Atom>().GetAtomType(), 1);
                atom2.GetComponent<Atom>().addConnectingAtoms(atom1.GetComponent<Atom>().GetAtomType(), GetConnectionNumber(atom2.GetComponent<Connect>().closestConnection.name));
                atom1.transform.position = connection.transform.GetChild(0).position;
                atom1.transform.rotation = Quaternion.identity;
                Vector3 direction1 = (atom1.transform.GetChild(0).GetChild(atoms1remaining).position - atom1.transform.position).normalized;
                float angle = Vector3.Angle(direction1, atom1.transform.forward);
                atom1.transform.LookAt(atom2.transform, atom1.transform.up);
                atom1.transform.Rotate(angle, 0, 0);
                
            }
            this.gameObject.GetComponent<Atom>().numeletroes--;
            connectScript.hitObject.GetComponent<Atom>().numeletroes--;

        }
        else if (connectionsWanted == 2)
        {
            if (this.gameObject.GetComponent<Atom>().maxeletroes == 2)
            { 
                int atoms2remaining = atom2.GetComponent<Atom>().maxeletroes - atom2.GetComponent<Atom>().numeletroes;
                GameObject connection = Instantiate(doublePrefab, atom2.GetComponent<Connect>().closestConnection.transform.position, Quaternion.LookRotation(atom2.GetComponent<Connect>().closestConnection.transform.position - atom2.transform.position), parent);
                connection.GetComponent<ConnectionProperties>().setLeftAtom(atom1);
                connection.GetComponent<ConnectionProperties>().setRightAtom(atom2);
                connection.GetComponent<ConnectionProperties>().setLeftAtomConnections(atom1.GetComponent<Atom>().dupla[0]);
                connection.GetComponent<ConnectionProperties>().setRightAtomConnections(atom2.GetComponent<Connect>().closestConnection);
                atom1.GetComponent<Atom>().addConnectingAtoms(atom2.GetComponent<Atom>().GetAtomType(), 1);
                atom2.GetComponent<Atom>().addConnectingAtoms(atom1.GetComponent<Atom>().GetAtomType(), GetConnectionNumber(atom2.GetComponent<Connect>().closestConnection.name));
                atom1.transform.position = connection.transform.GetChild(0).position;
            }
            else
            {
                int atoms2remaining = atom2.GetComponent<Atom>().maxeletroes - atom2.GetComponent<Atom>().numeletroes;
                GameObject connection = Instantiate(doublePrefab, atom2.GetComponent<Connect>().closestConnection.transform.position, Quaternion.LookRotation(atom2.GetComponent<Connect>().closestConnection.transform.position - atom2.transform.position), parent);
                connection.GetComponent<ConnectionProperties>().setLeftAtom(atom1);
                connection.GetComponent<ConnectionProperties>().setRightAtom(atom2);
                connection.GetComponent<ConnectionProperties>().setLeftAtomConnections(atom1.GetComponent<Atom>().dupla[0]);
                connection.GetComponent<ConnectionProperties>().setRightAtomConnections(atom2.GetComponent<Connect>().closestConnection);
                atom1.GetComponent<Atom>().addConnectingAtoms(atom2.GetComponent<Atom>().GetAtomType(), 1);
                atom2.GetComponent<Atom>().addConnectingAtoms(atom1.GetComponent<Atom>().GetAtomType(), GetConnectionNumber(atom2.GetComponent<Connect>().closestConnection.name));
                atom1.transform.position = connection.transform.GetChild(0).position;
                atom1.transform.LookAt(atom2.transform, atom1.transform.up);
                //atom1.transform.Rotate(Vector3 Axis,angle);
                
            }
            this.gameObject.GetComponent<Atom>().numeletroes -= connectionsWanted;
            connectScript.hitObject.GetComponent<Atom>().numeletroes -= connectionsWanted;

        }
        else if (connectionsWanted == 3)
        {
            if (this.gameObject.GetComponent<Atom>().maxeletroes == 3)
            {
                int atoms2remaining = atom2.GetComponent<Atom>().maxeletroes - atom2.GetComponent<Atom>().numeletroes;
                GameObject connection = Instantiate(triplePrefab, atom2.GetComponent<Connect>().closestConnection.transform.position, Quaternion.LookRotation(atom2.GetComponent<Connect>().closestConnection.transform.position - atom2.transform.position), parent);
                connection.GetComponent<ConnectionProperties>().setLeftAtom(atom1);
                connection.GetComponent<ConnectionProperties>().setRightAtom(atom2);
                connection.GetComponent<ConnectionProperties>().setLeftAtomConnections(atom1.GetComponent<Atom>().tripla[0]);
                connection.GetComponent<ConnectionProperties>().setRightAtomConnections(atom2.GetComponent<Connect>().closestConnection);
                atom1.GetComponent<Atom>().addConnectingAtoms(atom2.GetComponent<Atom>().GetAtomType(), 1);
                atom2.GetComponent<Atom>().addConnectingAtoms(atom1.GetComponent<Atom>().GetAtomType(), GetConnectionNumber(atom2.GetComponent<Connect>().closestConnection.name));
                atom1.transform.position = connection.transform.GetChild(0).position;
            }
            else
            {
                int atoms2remaining = atom2.GetComponent<Atom>().maxeletroes - atom2.GetComponent<Atom>().numeletroes;
                GameObject connection = Instantiate(triplePrefab, atom2.GetComponent<Connect>().closestConnection.transform.position, Quaternion.LookRotation(atom2.GetComponent<Connect>().closestConnection.transform.position - atom2.transform.position), parent);
                connection.GetComponent<ConnectionProperties>().setLeftAtom(atom1);
               
                connection.GetComponent<ConnectionProperties>().setRightAtom(atom2);
                connection.GetComponent<ConnectionProperties>().setLeftAtomConnections(atom1.GetComponent<Atom>().tripla[0]);
                connection.GetComponent<ConnectionProperties>().setRightAtomConnections(atom2.GetComponent<Connect>().closestConnection);
                atom1.GetComponent<Atom>().addConnectingAtoms(atom2.GetComponent<Atom>().GetAtomType(), 1);
                atom2.GetComponent<Atom>().addConnectingAtoms(atom1.GetComponent<Atom>().GetAtomType(), GetConnectionNumber(atom2.GetComponent<Connect>().closestConnection.name));
                atom1.transform.position = connection.transform.GetChild(0).position;
                atom1.transform.LookAt(atom2.transform, atom1.transform.up);
                //atom1.transform.Rotate(Vector3 Axis,angle);
                
            }
            this.gameObject.GetComponent<Atom>().numeletroes -= connectionsWanted;
            connectScript.hitObject.GetComponent<Atom>().numeletroes -= connectionsWanted;
        }
        foreach (Transform child in atom2.transform)
        {
            child.gameObject.SetActive(false);
        }
        //parent.transform.GetChild(0).position = atom1.transform.position;
        //parent.GetComponent<XRGrabInteractable>().attachTransform = atom1.transform;
    }


    public void deactivatemyconnections(int numConnection, int connectionsWanted)
    {
        switch (numConnection)
        {
            case 1:
                this.GetComponent<Atom>().simples[0].SetActive(false);
                foreach (GameObject c in this.GetComponent<Atom>().dupla)
                    if (c.name.Contains("1")) c.SetActive(false);
                foreach (GameObject c in this.GetComponent<Atom>().tripla)
                    c.SetActive(false);
                if (this.GetComponent<Atom>().tripla.Length > 2)
                {
                    this.GetComponent<Atom>().tripla[2].SetActive(true);
                }
                return;
            case 2:
                this.GetComponent<Atom>().simples[0].SetActive(false);
                this.GetComponent<Atom>().simples[1].SetActive(false);
                foreach (GameObject c in this.GetComponent<Atom>().dupla) {
                    if (c.name.Contains("1")) c.SetActive(false);
                    if (c.name.Contains("2")) c.SetActive(false);
                }
                foreach (GameObject c in this.GetComponent<Atom>().tripla)
                    c.SetActive(false);
                return;
            case 3:
                foreach (GameObject c in this.GetComponent<Atom>().simples)
                {
                    c.SetActive(false);
                }
                if (this.GetComponent<Atom>().simples.Length > 4)
                {
                        this.GetComponent<Atom>().simples[4].SetActive(true);
                }
                foreach (GameObject c in this.GetComponent<Atom>().dupla)
                {
                    c.SetActive(false);
                }
                return;
            case 4:
                return;
        }

    }
    public void deactivateotherconnections(GameObject connection, int numConnection, bool active)
    {
         switch (numConnection)
        {
            case 1:
                if (connection.name.Contains("1"))
                {
                    foreach (GameObject c in connectScript.hitObject.GetComponent<Atom>().dupla)
                    {
                        if (c.name.Contains("1")) c.SetActive(active);
                    }
                    foreach (GameObject c in connectScript.hitObject.GetComponent<Atom>().tripla)
                    {
                        if (c.name.Contains("1")) c.SetActive(active);
                    }

                }
                if (connection.name.Contains("2"))
                {
                    foreach (GameObject c in connectScript.hitObject.GetComponent<Atom>().dupla)
                    {
                        if (c.name.Contains("2")) c.SetActive(active);
                    }
                    foreach (GameObject c in connectScript.hitObject.GetComponent<Atom>().tripla)
                    {
                        if (c.name.Contains("2")) c.SetActive(active);
                    }
                }
                if (connection.name.Contains("3"))
                {
                    foreach (GameObject c in connectScript.hitObject.GetComponent<Atom>().dupla)
                    {
                        if (c.name.Contains("3")) c.SetActive(active);
                    }
                    foreach (GameObject c in connectScript.hitObject.GetComponent<Atom>().tripla)
                    {
                        if (c.name.Contains("3")) c.SetActive(active);
                    }
                }
                if (connection.name.Contains("4"))
                {
             
                    foreach (GameObject c in connectScript.hitObject.GetComponent<Atom>().dupla)
                    {
                        if (c.name.Contains("4")) c.SetActive(active);
                    }
                    foreach (GameObject c in connectScript.hitObject.GetComponent<Atom>().tripla)
                    {
                        if (c.name.Contains("4")) c.SetActive(active);
                    }
                }
                connection.SetActive(active);
                return;
            case 2:
                if (connection.name.Contains("1"))
                {
                    connectScript.hitObject.GetComponent<Atom>().simples[0].SetActive(active);
                    foreach(GameObject c in connectScript.hitObject.GetComponent<Atom>().dupla)
                    {
                        if (c.name.Contains("1")) c.SetActive(active);
                    }
                    foreach (GameObject c in connectScript.hitObject.GetComponent<Atom>().tripla)
                    {
                        if (c.name.Contains("1")) c.SetActive(active);
                    }

                }
                if (connection.name.Contains("2"))
                {
                    connectScript.hitObject.GetComponent<Atom>().simples[1].SetActive(active);
                    foreach (GameObject c in connectScript.hitObject.GetComponent<Atom>().dupla)
                    {
                        if (c.name.Contains("2")) c.SetActive(active);
                    }
                    foreach (GameObject c in connectScript.hitObject.GetComponent<Atom>().tripla)
                    {
                        if (c.name.Contains("2")) c.SetActive(active);
                    }
                }
                if (connection.name.Contains("3"))
                {
                    connectScript.hitObject.GetComponent<Atom>().simples[2].SetActive(active);
                    foreach (GameObject c in connectScript.hitObject.GetComponent<Atom>().dupla)
                    {
                        if (c.name.Contains("3")) c.SetActive(active);
                    }
                    foreach (GameObject c in connectScript.hitObject.GetComponent<Atom>().tripla)
                    {
                        if (c.name.Contains("3")) c.SetActive(active);
                    }
                }
                if (connection.name.Contains("4"))
                {
                    connectScript.hitObject.GetComponent<Atom>().simples[3].SetActive(active);
                    foreach (GameObject c in connectScript.hitObject.GetComponent<Atom>().dupla)
                    {
                        if (c.name.Contains("4")) c.SetActive(active);
                    }
                    foreach (GameObject c in connectScript.hitObject.GetComponent<Atom>().tripla)
                    {
                        if (c.name.Contains("4")) c.SetActive(active);
                    }
                }
                connection.SetActive(active);
                return;
            case 3:
                foreach (GameObject c in connectScript.hitObject.GetComponent<Atom>().simples)
                {
                    c.SetActive(active);
                }
                if (connectScript.hitObject.GetComponent<Atom>().simples.Length > 4)
                {
                    if (connection.name.Contains("1"))
                        connectScript.hitObject.GetComponent<Atom>().simples[4].SetActive(true);
                    else
                        connectScript.hitObject.GetComponent<Atom>().simples[5].SetActive(true);
                }
                foreach (GameObject c in connectScript.hitObject.GetComponent<Atom>().dupla)
                {
                    c.SetActive(active);
                }
                connection.SetActive(active);
                return;
            default:
                return;
        }
            
    }

    public int GetConnectionNumber(string connection)
    {
        if (connection.Contains("1"))
        {
            return 1;
        }
        if (connection.Contains("2"))
        {
            return 2;
        }
        if (connection.Contains("3"))
        {
            return 3;
        }
        if (connection.Contains("4"))
        {
            return 4;
        }
        return 0;
    }


}
