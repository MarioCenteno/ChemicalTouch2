using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atom : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public int maxeletroes;
    [SerializeField] public int numeletroes;
    [SerializeField] public GameObject[] simples;
    [SerializeField] public GameObject[] dupla;
    [SerializeField] public GameObject[] tripla;
    public int numConnections;
   
    [SerializeField] string Type;


    [System.Serializable] public struct connectionAtom
    {
        public int position;
        public string Type;
        public bool Checked;
       

        public connectionAtom(int v, string atomName)
        {
            this.position = v;
            this.Type = atomName;
            this.Checked= false;
        }

        public void changeChecked()
        {
            this.Checked = !this.Checked;
        }
    }


    [SerializeField] public List<connectionAtom> connectingAtoms;

        private void Start()
    {
         connectingAtoms = new List<connectionAtom>();
    }


    public bool isSolo()
    {
        return numConnections == 1;
    }

    public void activateAllConnections()
    {
        foreach(GameObject s in simples)
        {
            if (!s.name.Contains("5") && !s.name.Contains("6"))
                s.SetActive(true);
            else
                s.SetActive(false);
        }
        foreach (GameObject d in dupla)
        {
            d.SetActive(true);
        }
        foreach (GameObject t in tripla)
        {
            t.SetActive(true);
        }
    }

    public void activateConnections(GameObject connection, int numConnectionsAtom)
    {
                
                if (connection.name.Contains("1"))
                {
                    this.simples[0].SetActive(true);
                    foreach (GameObject con in dupla)
                    {
                    if (checkSimpleOfDouble(con))
                            con.SetActive(true);
                    }
                    foreach (GameObject con in tripla)
                    {
                        if (con.name.Contains("1"))
                            con.SetActive(true);
                    }
                }
                if (connection.name.Contains("2"))
                {
                    //Debug.Log("SSSSSRRRA");
                    this.simples[1].SetActive(true);
                    foreach (GameObject con in dupla)
                    {
                        if (checkSimpleOfDouble(con))
                            con.SetActive(true);
                    }
                    foreach (GameObject con in tripla)
                    {
                        if (con.name.Contains("2"))
                            con.SetActive(true);
                    }
                }
                if (connection.name.Contains("3"))
                {
                    this.simples[2].SetActive(true);
                    foreach (GameObject con in dupla)
                    {
                        if (checkSimpleOfDouble(con))
                            con.SetActive(true);
                    }
                    foreach (GameObject con in tripla)
                    {
                        if (con.name.Contains("3"))
                            con.SetActive(true);
                    }
                }
                if (connection.name.Contains("4"))
                {
                    this.simples[3].SetActive(true);
                    foreach (GameObject con in dupla)
                    {
                        if (checkSimpleOfDouble(con))
                            con.SetActive(true);
                    }
                    foreach (GameObject con in tripla)
                    {
                        if (con.name.Contains("4"))
                            con.SetActive(true);
                    }
                }
    }
    

    public bool checkSimpleOfDouble(GameObject connection)
    {
        if (connection.name.Contains("1") && connection.name.Contains("2"))
            return simples[0].activeSelf && simples[1].activeSelf;
        else if (connection.name.Contains("1") && connection.name.Contains("3"))
            return simples[0].activeSelf && simples[2].activeSelf;
        else if (connection.name.Contains("1") && connection.name.Contains("4"))
            return simples[0].activeSelf && simples[3].activeSelf;
        else if (connection.name.Contains("2") && connection.name.Contains("3"))
            return simples[1].activeSelf && simples[2].activeSelf;
        else if (connection.name.Contains("2") && connection.name.Contains("4"))
            return simples[1].activeSelf && simples[3].activeSelf;
        else if (connection.name.Contains("3") && connection.name.Contains("4"))
            return simples[2].activeSelf && simples[3].activeSelf;
        return false;
    }

    public void addConnectingAtoms(string atomName, int position)
    {
        connectingAtoms.Add(new connectionAtom(position-1,atomName));

    }

    public void removeConnectingAtoms(int position)
    {
    }
    public string GetAtomType()
    {
        return Type;
    }

    public int getDictionaryCount()
    {
        return connectingAtoms.Count;
    }
}
