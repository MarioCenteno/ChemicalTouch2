using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class AttatchPointCreator : MonoBehaviour
{
    private void Awake()
    {
        CreateAttatch();
    }

    private void CreateAttatch()
    {
        if(TryGetComponent(out XRGrabInteractable interactable))
        {
            GameObject attachObject = new GameObject("Attach");
            attachObject.transform.SetParent(transform);
            attachObject.transform.localPosition = Vector3.zero;
            attachObject.transform.rotation = Quaternion.identity;

            interactable.attachTransform = attachObject.transform;
        }
    }

}
