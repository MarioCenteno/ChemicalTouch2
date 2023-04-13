using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dials;
using UnityEngine.Events;

public class ContainerLock : MonoBehaviour
{
    public AtomicManager manager = null;
    public UnityEvent OnUnlock = null;
    public UnityEvent OnInsert = null;
    public Container myContainer;
    public bool isLocked = true;
    public MoleculeV2 key = null;
    public AtomV2 aKey = null;
    public bool keyInserted = false;
    public AtomType lockType = AtomType.Hydrogen;

    public List<Pin> pins = new List<Pin>();

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("AtomManager").GetComponent<AtomicManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (isLocked)
        {
            if (key != null)
            {
                if (keyInserted)
                {
                    bool locked = false;
                    foreach (Pin pin in pins)
                    {
                        if (pin.needsLock)
                        {

                            if (!pin.pinState)
                            {
                                locked = true;
                                break;
                            }
                        }
                        

                        Debug.Log(pin.pinType);

                    }

                    if (!locked)
                    {
                        Debug.Log("yay");
                        isLocked = false;
                        manager.OnUnlockSucess.Invoke(key);
                        OnUnlock.Invoke();
                    }
                }

            }
        }
       
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isLocked)
        {
            if (key == null)
            {
                if (other.CompareTag("Atom"))
                {
                    Debug.Log("atom tag");
                    AtomV2 atom;
                    if (other.attachedRigidbody.gameObject.TryGetComponent<AtomV2>(out atom))
                    {
                        Debug.Log("isatom", atom);
                        if (atom.molecule != null)
                        {
                            Debug.Log("atom mol " + atom.molecule, atom);
                            if (atom.type == lockType)
                            {
                                Debug.Log("atom type " + atom.type, atom);
                                key = atom.molecule;
                                aKey = atom;
                                keyInserted = true;

                            }

                            manager.OnKeyInsert.Invoke(atom.type, lockType, atom.molecule);

                        }
                    }
                }
            }
        }
        
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (isLocked)
        {
            if (key != null)
            {
                if (other.CompareTag("Atom"))
                {

                    AtomV2 atom;
                    if (other.attachedRigidbody.gameObject.TryGetComponent<AtomV2>(out atom))
                    {
                        if (atom.molecule != null)
                        {
                            if (atom.type == lockType)
                            {
                                if (atom.molecule == key)
                                {
                                    if(atom == aKey)
                                    {
                                        key = null;
                                        aKey = null;
                                        keyInserted = false;
                                        manager.OnUnlockFaill.Invoke(atom.molecule);
                                    }
                                    

                                }
                            }
                        }
                    }
                }
            }
        }
        
    }
}
