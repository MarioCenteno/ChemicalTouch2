using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectronV2 : MonoBehaviour
{
    public AtomV2 atom;
    public bool connected = false;
    private void OnTriggerEnter(Collider other)
    {
        if (atom.isGrabed)
        {
            if (!connected)
            {
                if (other.CompareTag("ConnectionPoint"))
                {
                    ElectronV2 otherElectron;
                    if (other.gameObject.TryGetComponent<ElectronV2>(out otherElectron))
                    {
                        AtomV2 otherAtom = otherElectron.atom;
                        if (otherAtom.isGrabed)
                        {
                            if (!atom.isConnecting && !otherAtom.isConnecting)
                            {
                                if (atom.molecule != null)
                                {
                                    if(otherAtom.molecule != null)
                                    {
                                        if (!atom.molecule.isConnecting && !otherAtom.molecule.isConnecting)
                                        {
                                            StartConnection(atom, otherAtom, otherElectron);
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        StartConnection(atom, otherAtom, otherElectron);
                                        return;
                                    }
                                }
                                else
                                {
                                    StartConnection(atom, otherAtom, otherElectron);
                                    return;
                                }
                                
                                
                            }

                            else if (atom.isConnecting && otherAtom.isConnecting)
                            {
                                if (atom.isMainConector)
                                {
                                    UpdateConnection(otherElectron);
                                    return;
                                }
                            }
                        }
                    }

                }
            }
            
        }
        
    }

    public void StartConnection(AtomV2 atom, AtomV2 otherAtom, ElectronV2 otherElectron)
    {
        GameObject connection = Instantiate(atom.manager.connectionPrefab);
        ConnectionV2 pendingConnection = connection.GetComponent<ConnectionV2>();
        connection.layer = LayerMask.NameToLayer("Molecule");
        pendingConnection.InitPendingConection(atom, otherAtom);
        pendingConnection.AddElectronPair(this, otherElectron);
        atom.StartPendingConnection(otherAtom, pendingConnection, true);
        otherAtom.StartPendingConnection(atom, pendingConnection, false);
        connected = true;
        otherElectron.connected = true;
    }

    public void UpdateConnection( ElectronV2 otherElectron)
    {
        ConnectionV2 pendingConnection = atom.pendingConnection;
        if (pendingConnection.size < 3)
        {
            pendingConnection.AddElectronPair(this, otherElectron);
            connected = true;
            otherElectron.connected = true;
            otherElectron.gameObject.SetActive(false);
            otherElectron.gameObject.SetActive(false);
        }
    }
}
