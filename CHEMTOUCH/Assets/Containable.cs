using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Containable : MonoBehaviour, IContainable
{
    public XRBaseInteractable interactable;
    public bool isContained;
    public AtomicManager manager;
    public bool IsContained { get => isContained; set => isContained = value; }

    public void Contain(Container cont)
    {
        if (!IsContained)
        {
            IsContained = true;
            interactable.interactionLayerMask = LayerMask.GetMask("Nothing");
            gameObject.transform.SetParent(cont.transform);
        }
    }

    public void Release()
    {
        IsContained = false;
        interactable.interactionLayerMask = ~0;
        gameObject.transform.SetParent(manager.ObjParent);
    }
}
