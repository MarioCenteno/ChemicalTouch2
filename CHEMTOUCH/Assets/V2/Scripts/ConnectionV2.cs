using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;

[Serializable]
public struct ElectronPair
{
    [SerializeField]
    public ElectronV2 E1;
    [SerializeField] 
    public ElectronV2 E2;
    [SerializeField] 
    public GameObject ConectionObj;
}

public class ConnectionV2 : MonoBehaviour
{
    [SerializeField] 
    public bool isRemoving = false;
    [SerializeField] 
    public AtomicManager manager;
    [SerializeField] 
    public LineRenderer line;
    [SerializeField] 
    public LineRenderer line1;
    [SerializeField] 
    public LineRenderer line2;
    [SerializeField] 
    public LineRenderer line3;
    [SerializeField] 
    public LineRenderer line4;
    /**/
    [SerializeField] 
    public LineRenderer singleCon;
    [SerializeField] 
    public LineRenderer doubleCon;
    [SerializeField] 
    public LineRenderer tripleCon;
    /**/

    [SerializeField] 
    public MoleculeV2 molecule;
    [SerializeField] 
    public AtomV2 A1 = null;
    [SerializeField] 
    public List<ElectronV2> atom1Electrons;
    [SerializeField] 
    public Vector3 directionAtom1;
    [SerializeField] 
    public AtomV2 A2 = null;
    [SerializeField] 
    public List<ElectronV2> atom2Electrons;
    [SerializeField] 
    public List<ElectronPair> electronPairs;
    [SerializeField] 
    public Vector3 directionAtom2;
    [SerializeField] 
    public GameObject connectionMeshObj;
    [SerializeField] 
    public int size = 0;
    [SerializeField] 
    public bool isPending;
    [SerializeField] 
    public MeshCollider colider;
    [SerializeField] 
    public bool minimized = false;

    public bool deleteFalg = false;

    private void Awake()
    {
        size = 0;
        atom1Electrons = new List<ElectronV2>();
        atom2Electrons = new List<ElectronV2>();
        electronPairs = new List<ElectronPair>();
        colider = gameObject.GetComponent<MeshCollider>();
    }


    public void minimize()
    {
        minimized = true;
    }
    public void maximize()
    {
        minimized = false;
    }

    public void freeElectrons()
    {
        foreach (ElectronV2 electron in atom1Electrons)
        {
            electron.connected = false;
            electron.gameObject.SetActive(true);
        }
        foreach (ElectronV2 electron in atom2Electrons)
        {
            electron.connected = false;
            electron.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (deleteFalg)
        {
            RemoveConnection();
        }
        /** /
        line.material = A1.lineMat;
        if (line.positionCount < 2) line.positionCount = 2;
        line.SetPosition(0, A1.transform.position);
        line.SetPosition(1, A2.transform.position);
        /**/
        directionAtom1 = new Vector3(0, 0, 0);
        foreach (ElectronV2 electron in atom1Electrons)
        {
            directionAtom1 += electron.transform.position - A1.transform.position;
        }
        directionAtom1 = A1.gameObject.transform.position + directionAtom1;
        //directionAtom1 /= atom1Electrons.Count;
        directionAtom2 = new Vector3(0, 0, 0);
        foreach (ElectronV2 electron in atom2Electrons)
        {
            directionAtom2 += electron.transform.position - A2.transform.position;
        }
        directionAtom2 = A2.gameObject.transform.position + directionAtom2;
        //directionAtom2 /= atom2Electrons.Count;
        /*LINE mode* /
        if (!isPending)
        {
            if (size >= 1)
            {
                singleCon.material = A1.lineMat;
                if (singleCon.positionCount < 2) singleCon.positionCount = 2;
                singleCon.SetPosition(0, electronPairs[0].E1.transform.position);
                singleCon.SetPosition(1, electronPairs[0].E2.transform.position);
            }

            if (size >= 2)
            {
                singleCon.material = A1.lineMat;
                if (singleCon.positionCount < 2) singleCon.positionCount = 2;
                singleCon.SetPosition(0, directionAtom1 + (electronPairs[0].E1.transform.position - directionAtom1).normalized * 0.05f);
                singleCon.SetPosition(1, directionAtom2 + (electronPairs[0].E2.transform.position - directionAtom2).normalized * 0.05f);

                doubleCon.material = A1.lineMat;
                if (doubleCon.positionCount < 2) doubleCon.positionCount = 2;
                doubleCon.SetPosition(0, directionAtom1 + (electronPairs[1].E1.transform.position - directionAtom1).normalized * 0.05f);
                doubleCon.SetPosition(1, directionAtom2 + (electronPairs[1].E2.transform.position - directionAtom2).normalized * 0.05f);
            }

            if (size >= 3)
            {
                tripleCon.material = A1.lineMat;
                if (tripleCon.positionCount < 2) tripleCon.positionCount = 2;
                tripleCon.SetPosition(0, directionAtom1 + (electronPairs[2].E1.transform.position - directionAtom1).normalized * 0.05f);
                tripleCon.SetPosition(1, directionAtom2 + (electronPairs[2].E2.transform.position - directionAtom2).normalized * 0.05f);
            }
        }
        /**/

        if (isPending)
        {


            /*DEBUG*/
            if (size >= 1)
            {
                line1.material = A1.lineMat;
                if (line1.positionCount < 2) line1.positionCount = 2;
                line1.SetPosition(0, electronPairs[0].E1.transform.position);
                line1.SetPosition(1, electronPairs[0].E2.transform.position);
            }

            if (size >= 2)
            {
                line2.material = A1.lineMat;
                if (line2.positionCount < 2) line2.positionCount = 2;
                line2.SetPosition(0, electronPairs[1].E1.transform.position);
                line2.SetPosition(1, electronPairs[1].E2.transform.position);
            }

            if (size >= 3)
            {
                line3.material = A1.lineMat;
                if (line3.positionCount < 2) line3.positionCount = 2;
                line3.SetPosition(0, electronPairs[2].E1.transform.position);
                line3.SetPosition(1, electronPairs[2].E2.transform.position);
            }

            line4.material = A1.lineMat;
            if (line4.positionCount < 2) line4.positionCount = 2;
            line4.SetPosition(0, directionAtom1);
            line4.SetPosition(1, directionAtom2);
            /**/
        }





    }

    public void instantiateRenderers()
    {
        GameObject temp = new GameObject("r");
        temp.transform.SetParent(gameObject.transform);
        temp.layer = gameObject.layer;
        line = temp.AddComponent<LineRenderer>();
        line.positionCount = 0;
        line.startColor = Color.red;
        line.endColor = Color.red;
        line.startWidth = 0.01f;
        line.endWidth = 0.01f;

        temp = new GameObject("r1");
        temp.transform.SetParent(gameObject.transform);
        temp.layer = gameObject.layer;
        line1 = temp.AddComponent<LineRenderer>();
        line1.positionCount = 0;
        line1.startColor = Color.green;
        line1.endColor = Color.green;
        line1.startWidth = 0.01f;
        line1.endWidth = 0.01f;

        temp = new GameObject("r2");
        temp.transform.SetParent(gameObject.transform);
        temp.layer = gameObject.layer;

        line2 = temp.AddComponent<LineRenderer>();
        line2.positionCount = 0;
        line2.startColor = Color.green;
        line2.endColor = Color.green;
        line2.startWidth = 0.01f;
        line2.endWidth = 0.01f;

        temp = new GameObject("r3");
        temp.transform.SetParent(gameObject.transform);
        temp.layer = gameObject.layer;

        line3 = temp.AddComponent<LineRenderer>();
        line3.positionCount = 0;
        line3.startColor = Color.green;
        line3.endColor = Color.green;
        line3.startWidth = 0.01f;
        line3.endWidth = 0.01f;

        line4 = gameObject.AddComponent<LineRenderer>();
        line4.positionCount = 0;
        line4.startColor = Color.red;
        line4.endColor = Color.red;
        line4.startWidth = 0.01f;
        line4.endWidth = 0.01f;
        /**/
        temp = new GameObject("rAv1");
        temp.transform.SetParent(gameObject.transform);
        temp.layer = gameObject.layer;

        singleCon = temp.AddComponent<LineRenderer>();
        singleCon.positionCount = 0;
        singleCon.startColor = Color.white;
        singleCon.endColor = Color.white;
        singleCon.startWidth = 0.02f;
        singleCon.endWidth = 0.02f;

        temp = new GameObject("rAv2");
        temp.transform.SetParent(gameObject.transform);
        temp.layer = gameObject.layer;

        doubleCon = temp.AddComponent<LineRenderer>();
        doubleCon.positionCount = 0;
        doubleCon.startColor = Color.white;
        doubleCon.endColor = Color.white;
        doubleCon.startWidth = 0.02f;
        doubleCon.endWidth = 0.02f;

        temp = new GameObject("rAv3");
        temp.transform.SetParent(gameObject.transform);
        temp.layer = gameObject.layer;

        tripleCon = temp.AddComponent<LineRenderer>();
        tripleCon.positionCount = 0;
        tripleCon.startColor = Color.white;
        tripleCon.endColor = Color.white;
        tripleCon.startWidth = 0.02f;
        tripleCon.endWidth = 0.02f;
        /**/
    }

    public void InitPendingConection(AtomV2 atom1, AtomV2 atom2)
    {
        A1 = atom1;
        A2 = atom2;
        isPending = true;
        manager = atom1.manager;
        size = 0;
        instantiateRenderers();
    }
    public void AddElectronPair(ElectronV2 E1, ElectronV2 E2)
    {
        size++;

        ElectronPair pair;
        pair.E1 = E1;
        pair.E2 = E2;
        pair.ConectionObj = null;
        electronPairs.Add(pair);

        atom1Electrons.Add(E1);

        atom2Electrons.Add(E2);

    }

    public void updateConnectionDirections()
    {
        directionAtom1 = new Vector3(0, 0, 0);
        foreach (ElectronV2 electron in atom1Electrons)
        {
            directionAtom1 += electron.transform.position - A1.transform.position;
        }
        directionAtom1 = A1.gameObject.transform.position + directionAtom1;
        //directionAtom1 /= atom1Electrons.Count;
        directionAtom2 = new Vector3(0, 0, 0);
        foreach (ElectronV2 electron in atom2Electrons)
        {
            directionAtom2 += electron.transform.position - A2.transform.position;
        }
        directionAtom2 = A2.gameObject.transform.position + directionAtom2;

    }
    public void CompleteConnection(AtomV2 atomToMove, AtomV2 atomToStay)
    {
        if (isPending)
        {
            line.positionCount = 0;
            line1.positionCount = 0;
            line2.positionCount = 0;
            line3.positionCount = 0;
            line4.positionCount = 0;

            A1.connections.Add(this);
            A2.connections.Add(this);
            //A2.removePendingConnection();
            //A1.removePendingConnection();
            isPending = false;
            A1.currentConections += size;
            A2.currentConections += size;


            /**/
            Vector3 stayPos;
            Vector3 movePos;
            updateConnectionDirections();
            if (atomToStay == A1)
            {
                stayPos = directionAtom1;
            }
            else
            {
                stayPos = directionAtom2;
            }
            /**/
            /**/
            switch (size)
            {
                case 1:
                    connectionMeshObj = Instantiate(manager.singleConnectionPrefab, gameObject.transform.position, Quaternion.identity);
                    break;
                case 2:
                    connectionMeshObj = Instantiate(manager.dupleConnectionPrefab, gameObject.transform.position, Quaternion.identity);
                    break;
                case 3:
                    connectionMeshObj = Instantiate(manager.triplrConnectionPrefab, gameObject.transform.position, Quaternion.identity);
                    break;
                default:
                    connectionMeshObj = Instantiate(manager.singleConnectionPrefab, gameObject.transform.position, Quaternion.identity);
                    break;
            }
            ConnectionProprieties con_proprieties;
            float distance = 0.2f;
            if(connectionMeshObj.TryGetComponent<ConnectionProprieties>(out con_proprieties))
            {
                distance = (con_proprieties.PointA.position - con_proprieties.PointB.position).magnitude; 
            }
            connectionMeshObj.transform.SetParent(gameObject.transform);
            gameObject.transform.position = atomToStay.transform.position;
            connectionMeshObj.transform.rotation = Quaternion.FromToRotation(connectionMeshObj.transform.up, (stayPos - connectionMeshObj.transform.position).normalized);
            Vector3 dirStay = (stayPos - atomToStay.transform.position).normalized;
            Vector3 goalPos = (atomToStay.transform.position + dirStay * distance);
            Vector3 offset = goalPos - atomToMove.transform.position;

            /**/
            if (atomToMove.molecule != null)
            {
                //Debug.Log("here");
                atomToMove.molecule.transform.position += offset;
                updateConnectionDirections();
                /**/
                if (atomToMove == A1)
                {
                    movePos = directionAtom1;
                }
                else
                {
                    movePos = directionAtom2;
                }
                Vector3 dirMove = (movePos - atomToMove.transform.position).normalized;
                float angle = Vector3.Angle(dirMove, -dirStay);
                Vector3 axis = Vector3.Cross(dirMove, -dirStay);
                atomToMove.molecule.transform.RotateAround(atomToMove.transform.position, axis, angle);
            }
            else
            {
                atomToMove.transform.position += offset;
                updateConnectionDirections();
                if (atomToMove == A1)
                {
                    movePos = directionAtom1;
                }
                else
                {
                    movePos = directionAtom2;
                }
                Vector3 dirMove = (movePos - atomToMove.transform.position).normalized;
                Quaternion rot = Quaternion.FromToRotation(dirMove, -dirStay);
                atomToMove.transform.rotation = rot * atomToMove.transform.rotation;
            }

            updateConnectionDirections();
        }
        



        /** /

        atomToMove.transform.position = atomToStay.transform.position - (directionVector.normalized * 0.2f);

        /** /
        if (atomToStay.molecule != null)
        {
            MoleculeV2 mol = atomToStay.molecule;
            ParentConstraint parentConstraint = mol.gameObject.GetComponent<ParentConstraint>();
            parentConstraint.weight = 0f;//Master weight
            parentConstraint.constraintActive = false;
        }
        if (atomToMove.molecule != null)
        {
            MoleculeV2 mol = atomToMove.molecule;
            ParentConstraint parentConstraint = mol.gameObject.GetComponent<ParentConstraint>();
            parentConstraint.weight = 0f;//Master weight
            parentConstraint.constraintActive = false;
        }
        /**/
    }
    public void SetParent(MoleculeV2 molecule)
    {
        if(molecule == null)
        {
            gameObject.transform.SetParent(manager.transform);
        }
        else
        {
            gameObject.transform.SetParent(molecule.gameObject.transform);
        }        
    }

    public void RemoveConnection()
    {
        manager.OnDeleteConnection.Invoke(this);
        //Debug.Log(" // ", A1);
        //Debug.Log(" // ", A2);
        if(molecule.BreakConnection(this, A1, A2))
        {
            freeElectrons();
            //Debug.Log("removing: ", this.gameObject);
            //Debug.Log(" from: ", molecule.gameObject);
            A1.currentConections -= size;
            A2.currentConections -= size;
            A1.connections.Remove(this);
            A2.connections.Remove(this);
            A1.connections.RemoveAll(item => item == null);//limpar null elements para evitar bugs
            A2.connections.RemoveAll(item => item == null);
            Destroy(gameObject);//delay test
        }
        else
        {
            freeElectrons();
            //Debug.Log("removing: ", this.gameObject);
            //Debug.Log(" from: ", molecule.gameObject);
            A1.currentConections -= size;
            A2.currentConections -= size;
            A1.connections.Remove(this);
            A2.connections.Remove(this);
            A1.connections.RemoveAll(item => item == null);//limpar null elements para evitar bugs
            A2.connections.RemoveAll(item => item == null);
            Debug.Log("FAILED TO REMOVE",gameObject);
            Debug.Break();
        }
        
       
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (!minimized)
        {
            if (other.CompareTag("Eraser"))
            {
                EraseConnection eraser = other.gameObject.GetComponent<EraseConnection>();
                if (eraser != null)
                {
                    if (eraser.erasing)
                    {
                        if (!isRemoving)
                        {
                            isRemoving = true;
                            //Debug.Log("erasing");
                            RemoveConnection();
                        }
                        else
                        {
                            //Debug.Break();
                        }

                    }
                }

            }
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!minimized)
        {
            if (other.CompareTag("Eraser"))
            {
                // Debug.Log("start touching");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!minimized)
        {
            if (other.CompareTag("Eraser"))
            {
                // Debug.Log("stop touching");
            }
        }
    }
}
