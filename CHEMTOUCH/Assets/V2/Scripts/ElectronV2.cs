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
                if (other.CompareTag("ConnectionPoint"))//Verifica se está a colidir com outro eletrão
                {
                    ElectronV2 otherElectron;
                    if (other.gameObject.TryGetComponent<ElectronV2>(out otherElectron))
                    {
                        AtomV2 otherAtom = otherElectron.atom;
                        if (otherAtom.isGrabed)//Verifica se o atomo oposto está agarrado (Isto pode corresponder a um atomo não agarrado)
                        {
                            if (!atom.isConnecting && !otherAtom.isConnecting)//Caso nenhum dos atomos esteja conectado
                            {
                                if (atom.molecule != null)
                                {
                                    if(otherAtom.molecule != null)//Ambos os atomos fazem parte de moléculas diferentes
                                    {
                                        if (!atom.molecule.isConnecting && !otherAtom.molecule.isConnecting)//As moléculas não estão a tentar ligar-se a outros elementos
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

                            else if (atom.isConnecting && otherAtom.isConnecting)//Caso os atomos já estejam ligados a algo (Alterar aqui)
                            {
                                if (atom.isMainConector)
                                {
                                    UpdateConnection(otherElectron);
                                    return;
                                }
                                /*else //- Código a implementar para as moléculas ciclicas
                                {
                                    StartConnection(atom, otherAtom, otherElectron); //- Verificar o processo de ligação e porquê que falha aqui
                                    return;
                                }*/
                            }
                           /* else{ //- Código a implementar para um atomo se ligar a múltiplas partes de uma molécula
                                    StartConnection(atom, otherAtom, otherElectron); //- Verificar o processo de ligação e porquê que falha aqui (e adicionar ligações de um atomo a múltiplos pontos de uma molécula)
                                    return;
                            }*/
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

    public void UpdateConnection( ElectronV2 otherElectron)//Atualiza a conexão caso a ligação envolva mais do que um par de atomos.
    {
        ConnectionV2 pendingConnection = atom.pendingConnection;
        if (pendingConnection.size < 3)
        {
            pendingConnection.AddElectronPair(this, otherElectron);
            connected = true;
            otherElectron.connected = true;
            otherElectron.gameObject.SetActive(false);
            otherElectron.gameObject.SetActive(false);// Possivel remoção no futuro
        }
    }
}
