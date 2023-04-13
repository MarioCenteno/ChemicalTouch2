using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimizer : MonoBehaviour
{

    bool m_Started;
    public LayerMask m_LayerMask;
    public Transform resultSpawn;
    List<MoleculeV2> collidingMolecules;
    List<AtomV2> collidingAtoms;

    void Start()
    {
        collidingAtoms = new List<AtomV2>();
        collidingMolecules = new List<MoleculeV2>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody)
        {
            if (other.attachedRigidbody.gameObject.CompareTag("Mollecule"))
            {
                Debug.Log("hereTM");
            }
            if (other.attachedRigidbody.gameObject.CompareTag("Atom"))
            {
                Debug.Log("hereTA");
            }
        }   
    }

    public void minimize()
    {
        /**/
        print("here");
        Vector3 botom = gameObject.transform.position - new Vector3(0, transform.localScale.y, 0);
        Vector3 top = gameObject.transform.position + new Vector3(0, transform.localScale.y, 0);
        Collider[] hitColliders = Physics.OverlapCapsule(botom, top, transform.localScale.x / 2, m_LayerMask);
        int i = 0;
        //Check when there is a new collider coming into contact with the box
        collidingMolecules = new List<MoleculeV2>();
        collidingAtoms = new List<AtomV2>();
        foreach (Collider colider in hitColliders)
        {
            if (colider.attachedRigidbody)
            {
                MoleculeV2 mol;
                AtomV2 atom;
                if (colider.attachedRigidbody.CompareTag("Mollecule"))
                {
                    
                    if (colider.attachedRigidbody.gameObject.TryGetComponent<MoleculeV2>(out mol))
                    {
                        if (!mol.isGrabed)
                        {
                            if (!mol.minimized)
                            {
                                if (!collidingMolecules.Contains(mol))
                                {
                                    collidingMolecules.Add(mol);
                                }
                            }
                        }
                    }
                }
                else if (colider.attachedRigidbody.CompareTag("Atom"))
                {
                    if (colider.attachedRigidbody.gameObject.TryGetComponent<AtomV2>(out atom))
                    {
                        if (atom.molecule)
                        {
                            mol = atom.molecule;
                            if (!mol.isGrabed)
                            {
                                if (!mol.minimized)
                                {
                                    if (!collidingMolecules.Contains(mol))
                                    {
                                        collidingMolecules.Add(mol);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!atom.isGrabed)
                            {
                                if (!atom.minimized)
                                {
                                    if (!collidingAtoms.Contains(atom))
                                    {
                                        collidingAtoms.Add(atom);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
        }
        Debug.Log(collidingAtoms);
        Debug.Log(collidingMolecules);
        float offset = -0.05f;//ofsetamount
        foreach (MoleculeV2 mol in collidingMolecules)
        {
            Debug.Log(mol);
            mol.minimize();
            mol.transform.position = new Vector3(resultSpawn.position.x, resultSpawn.position.y + offset, resultSpawn.position.z);
            offset += 0.05f;
        }
        offset = -0.05f;
        foreach (AtomV2 atom in collidingAtoms)
        {
            Debug.Log(atom);
            atom.minimize();
            atom.transform.position = new Vector3(resultSpawn.position.x, resultSpawn.position.y + offset, resultSpawn.position.z);
            offset += 0.05f;
        }
        /**/
    }
}
