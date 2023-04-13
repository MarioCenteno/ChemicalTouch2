using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{
    public ContainerLock myLock;
    public AtomType pinType = AtomType.Hydrogen;
    public AtomV2 aPin = null;
    public bool pinState = false;
    public bool needsLock = true;
    private void OnTriggerEnter(Collider other)
    {
        if (myLock.key != null)
        {
            if (other.CompareTag("Atom"))
            {
               
                AtomV2 atom;
                if (other.attachedRigidbody.gameObject.TryGetComponent<AtomV2>(out atom))
                {
                    if (atom.molecule == myLock.key)
                    {
                        if (atom.molecule.minimized)
                        {

                            if (!pinState)
                            {
                                if (atom.type == pinType)
                                {
                                    aPin = atom;
                                    Debug.Log("atom type " + atom.type, atom);
                                    pinState = true;
                                }                              
                            }
                            
                        }

                    }
                }
                

            }
        }
       
    }

    private void OnTriggerStay(Collider other)
    {
        if (myLock.key != null)
        {
            if (other.CompareTag("Atom"))
            {

                AtomV2 atom;
                if (other.attachedRigidbody.gameObject.TryGetComponent<AtomV2>(out atom))
                {
                    if (atom.molecule == myLock.key)
                    {
                        if (atom.molecule.minimized)
                        {

                            if (!pinState)
                            {
                                if (atom.type == pinType)
                                {
                                    aPin = atom;
                                    Debug.Log("atom type " + atom.type, atom);
                                    pinState = true;
                                }
                            }

                        }

                    }
                }


            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if(myLock.key != null)
        {
            if (other.CompareTag("Atom"))
            {
                
                AtomV2 atom;
                if (other.attachedRigidbody.gameObject.TryGetComponent<AtomV2>(out atom))
                {
                    if (atom.molecule == myLock.key || !needsLock)
                    {
                        if (atom.molecule.minimized|| !needsLock)
                        {
                            if (pinState)
                            {
                                if (atom.type == pinType)
                                {
                                    if(atom == aPin)
                                    {
                                        aPin = null;
                                        pinState = false;

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
