using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomicToggle : MonoBehaviour
{
    public bool m_Started = false;
    public LayerMask m_LayerMask;
    public float posMod = 1;
    public float radiusMod = 1;
    List<MoleculeV2> collidingMolecules;
    List<AtomV2> collidingAtoms;
    public AtomicManager manager = null;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("AtomManager").GetComponent<AtomicManager>();
        collidingAtoms = new List<AtomV2>();
        collidingMolecules = new List<MoleculeV2>();
    }
    /** /
    public void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody)
        {
            if (other.attachedRigidbody.gameObject.CompareTag("Mollecule"))
            {
                //Debug.Log("hereTM");
            }
            if (other.attachedRigidbody.gameObject.CompareTag("Atom"))
            {
                //Debug.Log("hereTA");
            }
        }
    }
    /**/

    public void ToggleAtoms()
    {
        /**/
        Vector3 botom = gameObject.transform.position - new Vector3(0, transform.localScale.y * posMod, 0);
        Vector3 top = gameObject.transform.position + new Vector3(0, transform.localScale.y * posMod, 0);
        Collider[] hitColliders = Physics.OverlapCapsule(botom, top, transform.localScale.x * 0.5f * radiusMod, m_LayerMask);
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
                            
                            if (!collidingMolecules.Contains(mol))
                            {
                                collidingMolecules.Add(mol);
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
                                
                                if (!collidingMolecules.Contains(mol))
                                {
                                    collidingMolecules.Add(mol);
                                }
                               
                            }
                        }
                        else
                        {
                            if (!atom.isGrabed)
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
        //Debug.Log(collidingAtoms);
        //Debug.Log(collidingMolecules);

        foreach (MoleculeV2 mol in collidingMolecules)
        {
            //Debug.Log(mol);
            if (mol.minimized)
            {
                mol.maximize();
                manager.OnMoleculeToggle.Invoke(mol, false);
            }
            else
            {
                mol.minimize();
                manager.OnMoleculeToggle.Invoke(mol, true);
            }

        }
        foreach (AtomV2 atom in collidingAtoms)
        {

            //Debug.Log(atom);
            if (atom.minimized)
            {
                atom.maximize();
                manager.OnAtomToggle.Invoke(atom, false);

            }
            else
            {
                atom.minimize();
                manager.OnAtomToggle.Invoke(atom, true);
            }
        }
        /**/
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (m_Started)
        {
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Vector3 botom = gameObject.transform.position - new Vector3(0, transform.localScale.y * posMod, 0);
            Vector3 top = gameObject.transform.position + new Vector3(0, transform.localScale.y * posMod, 0);
            float radius = transform.localScale.x * 0.5f * radiusMod;
            Gizmos.DrawWireSphere(botom, radius);
            Gizmos.DrawWireSphere(top, radius);
        }
        

    }
}
