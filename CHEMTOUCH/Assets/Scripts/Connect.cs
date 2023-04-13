using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class Connect : MonoBehaviour
{
    public float MaxDistance;
    public bool isMollecule;
    public GameObject hitObject;
    public bool ishitting;
    public int numConnection;
    public int hitnumConnection;
    public int currentConnections;
    [SerializeField] Atom atomChars;
    [SerializeField] Outline outline;


    private bool isconnecting;
    public GameObject closestConnection;



    // Start is called before the first frame update

    private void Start()
    {
        isMollecule = false;
        ishitting = false;
        isconnecting = false;
        numConnection = 0;
        hitObject = null;
        closestConnection = null;
       
    }

    // Update is called once per frame
    void Update()
    {
        if (outline.enabled == true) { outline.enabled = false; }
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, rayDirection(), out hit, MaxDistance))
        {

            /*      if (hit.transform.parent != null && hit.transform.parent.name.Contains("Mollecule"))
                  {

                      isMollecule = true;
                  }
                  if (hit.transform.tag == "Atom")
                  {
                      connectingAtoms(this.transform,hit.transform);

                  }*/
            if (!isconnecting && hit.transform.tag == "Atom")
            {
                if (hit.transform.parent != null && hit.transform.parent.name.Contains("Mollecule"))
                    isMollecule = true;
                isconnecting = true;
                hitObject = hit.transform.gameObject;
            }
        }
        if (hitObject != null)
        {
            connectingAtoms(this.transform, hitObject.transform);
            
        }
        getconnection();

    }

    Vector3 rayDirection()
    {
        return new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
    }

    void connectingAtoms(Transform atom, Transform hitAtom)
    {
        float Distance = Vector3.Distance(atom.position, hitAtom.position);
        if (Distance < 0.2 && !ishitting && hitAtom.gameObject.GetComponent<Connect>().getmaxnumeletroes()>0)
        {
            ishitting = true;
            if (this.atomChars.maxeletroes > numConnection && hitAtom.gameObject.GetComponent<Connect>().getmaxnumeletroes() > hitAtom.gameObject.GetComponent<Connect>().numConnection && numConnection<3)
            {

                hitAtom.gameObject.GetComponent<Connect>().numConnection++;
                hitAtom.gameObject.GetComponent<Connect>().currentConnections++;
                currentConnections++;
                hitnumConnection++;
                numConnection++;
                highlightConnections(numConnection, hitAtom);

            }

        }

        else if (Distance > 0.2 && Distance < 0.3)
        {
            ishitting = false;
        }
        else if (Distance > 0.3)
        {
            hitObject = null;
            isconnecting = false;
            ishitting = false;
            numConnection = 0;
            hitAtom.gameObject.GetComponent<Connect>().numConnection -= hitnumConnection;
            hitAtom.gameObject.GetComponent<Connect>().currentConnections = 0;
            currentConnections = 0;
            hitnumConnection = 0;
            closestConnection = null;
            foreach (Transform child in hitAtom)
            {
                child.gameObject.SetActive(false);
            }

        }
    }

    void highlightConnections(int numConnection, Transform hitAtom)
    {
        foreach (Transform child in hitAtom)
        {
            child.gameObject.SetActive(false);
        }
        switch (numConnection)
        {
            case 1:
                hitAtom.GetChild(0).gameObject.SetActive(true);
                return;
            case 2:
                hitAtom.GetChild(1).gameObject.SetActive(true);
                return;
            case 3:
                hitAtom.GetChild(2).gameObject.SetActive(true);
                return;
        }
   
    }


    public void getconnection(){
        if (numConnection == 1)
        {

            foreach (GameObject connect in hitObject.GetComponent<Atom>().simples)
            {

                if (Vector3.Distance(this.transform.position, connect.transform.GetChild(0).position) < 0.06f && connect.activeInHierarchy)
                {
                    hitObject.GetComponent<Connect>().closestConnection = connect;
                    highlightLigacao(connect, hitObject.GetComponent<Atom>().simples);

                }
            }
        }
        else if (numConnection == 2)
        {

            foreach (GameObject connect in hitObject.GetComponent<Atom>().dupla)
            {

                if (Vector3.Distance(this.transform.position, connect.transform.GetChild(0).position) < 0.06f && connect.activeInHierarchy)
                {
                    
                    hitObject.GetComponent<Connect>().closestConnection = connect;
                    highlightLigacao(connect, hitObject.GetComponent<Atom>().dupla);

                    //Debug.Log("CLOSEST CONNECT ---> " + closestConnection.name);
                }
            }
        }
        else if (numConnection == 3)
        {
            foreach (GameObject connect in hitObject.GetComponent<Atom>().tripla)
            {
                if (Vector3.Distance(this.transform.position, connect.transform.GetChild(0).position) < 0.06f && connect.activeInHierarchy)
                {
                   
                    hitObject.GetComponent<Connect>().closestConnection = connect;
                    highlightLigacao(connect, hitObject.GetComponent<Atom>().tripla);
                }
            }
        }
        else return;
    }
    public int getmaxnumeletroes()
    {
        return atomChars.maxeletroes;
    }

    public void highlightLigacao(GameObject connection, GameObject[] others)
    {
        foreach(GameObject c in others)
        {
            c.GetComponent<Outline>().enabled = false;
        }
        connection.GetComponent<Outline>().enabled = true;
    }

    public void resetVariables()
    {
        isMollecule = false;
        ishitting = false;
        isconnecting = false;
        numConnection = 0;
        hitObject = null;
        closestConnection = null;
    }
}
