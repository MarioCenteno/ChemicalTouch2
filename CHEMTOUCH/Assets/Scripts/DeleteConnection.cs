using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteConnection : MonoBehaviour
{

    [SerializeField] GameObject trash;
    private GameObject WorldObject;

    private void Start()
    {
        WorldObject = GameObject.Find("WorldObjects");
    }

    // Start is called before the first frame update
    public void deleteConnection(GameObject atom1, GameObject atom2, GameObject connectionAtom1, GameObject connectionAtom2, GameObject connection)
    {
        
        Atom atom1script = atom1.GetComponent<Atom>();
        Atom atom2script = atom2.GetComponent<Atom>();
        GameObject mollecule1 = atom1.transform.parent.gameObject;
        //GameObject mollecule2 = atom2.transform.parent.gameObject;


        if (atom1script.numConnections == 1)
        {
            atom1script.activateAllConnections();
            atom1script.numConnections=0;
            atom1script.numeletroes = atom1script.maxeletroes;
            atom1.transform.parent = WorldObject.transform;

        }
        else
        {
            atom1script.activateConnections(connectionAtom1, getThisNumConnections(connection));
            atom1script.numConnections--;
            atom1script.numeletroes+= getThisNumConnections(connection);
        }
            
        if (atom2script.numConnections == 1)
        {
            atom2script.activateAllConnections();
            atom2script.numConnections = 0;
            atom2script.numeletroes = atom2script.maxeletroes;
            atom2.transform.parent = WorldObject.transform;
        }
        else
        {
            atom2script.activateConnections(connectionAtom2, getThisNumConnections(connection));
            atom2script.numConnections--;
            atom2script.numeletroes+= getThisNumConnections(connection);
        }
        connection.transform.parent = null;
        connection.transform.parent = trash.transform;
        connection.transform.position = trash.transform.position;
        if (mollecule1.transform.childCount == 1)
        {
            mollecule1.transform.parent = null;
            mollecule1.transform.parent = trash.transform;
            mollecule1.transform.position = trash.transform.position;
        }
        atom1.GetComponent<Connect>().resetVariables();
        atom2.GetComponent<Connect>().resetVariables();
        atom1script.removeConnectingAtoms(GetConnectionNumber(connectionAtom1.name));
        atom2script.removeConnectingAtoms(GetConnectionNumber(connectionAtom2.name));
    } 


    public int getThisNumConnections(GameObject connectionName)
    {
        if (connectionName.name.Contains("Simple"))
        {
            return 1;
        }
        else if (connectionName.name.Contains("double"))
        {
            return 2;
        }
        else return 3;
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
