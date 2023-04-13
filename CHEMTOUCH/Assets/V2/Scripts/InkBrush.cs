using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class InkBrush : MonoBehaviour, IContainable
{
    public XRBaseInteractable interactable;
    public bool isContained;

    public AtomType atomColor;
    public XRSocketInteractor socket;
    public InkCanister canister;
    public bool hasCanister;
    public AtomV2 atomToPaint;
    public MeshRenderer myMesh;
    public AtomicManager manager;


    public bool IsContained { get => isContained; set => isContained = value; }

    public void Contain(Container cont)
    {
        if (!IsContained)
        {
            IsContained = true;
            interactable.interactionLayerMask = LayerMask.GetMask("Nothing");
            socket.interactionLayerMask = LayerMask.GetMask("Nothing");
            gameObject.transform.SetParent(cont.transform);
        }
    }

    public void Release()
    {
        IsContained = false;
        interactable.interactionLayerMask = ~0;
        socket.interactionLayerMask = LayerMask.GetMask("Ink");
        gameObject.transform.SetParent(manager.ObjParent);
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Atom"))
        {
            AtomV2 a;
            if(collision.gameObject.TryGetComponent<AtomV2>(out a))
            {
                atomToPaint = a;
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Atom"))
        {
            atomToPaint = null;
        }
    }

    public void addCanister()
    {
        if (socket.selectTarget)
        {
            InkCanister ink;
            if (socket.selectTarget.gameObject.TryGetComponent<InkCanister>(out ink))
            {
                canister = ink;

                atomColor = canister.atomColor;
                myMesh.material = manager.atomProprieties[((int)atomColor)].material;
                hasCanister = true;
            }
        }
        
    }

    public void removeCanister()
    {
        canister = null;
        atomColor = AtomType.Empty;
        myMesh.material = manager.atomProprieties[((int)atomColor)].material;
        hasCanister = false;
    }

    public void TryPaint()
    {
        Debug.Log("tp " + atomColor +" a "+ atomToPaint);
        if (hasCanister)
        {
            if (atomToPaint)
            {
                Debug.Log("panting " + atomToPaint);
                if (!atomToPaint.minimized)
                {
                    if (atomToPaint.currentConections <= manager.atomProprieties[((int)atomColor)].connectionLimit)
                    {
                        if(atomToPaint.type == AtomType.Empty)
                        {
                            Debug.Log("atomToPaint was empty");
                            
                            atomToPaint.type = atomColor;
                            atomToPaint.MyMesh.material = manager.atomProprieties[((int)atomColor)].material;
                            canister.atomColor = AtomType.Empty;
                            atomColor = AtomType.Empty;
                            canister.mesh.material = manager.atomProprieties[((int)AtomType.Empty)].material;
                            myMesh.material = manager.atomProprieties[((int)atomColor)].material;
                            atomToPaint.HideElectrons();
                        }

                        else
                        {
                            Debug.Log("atomToPaint was not empty");
                            if (atomColor == AtomType.Empty)
                            {
                                Debug.Log("but i was");
                                canister.atomColor = atomToPaint.type;
                                atomColor = atomToPaint.type;
                                canister.mesh.material = manager.atomProprieties[((int)atomToPaint.type)].material;
                                atomToPaint.type = AtomType.Empty;
                                atomToPaint.MyMesh.material = manager.atomProprieties[((int)AtomType.Empty)].material;
                                myMesh.material = manager.atomProprieties[((int)atomColor)].material;
                                atomToPaint.HideElectrons();
                            }
                        }
                    
                    }
                }
            }
        }
        
        
    }
}
