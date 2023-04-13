using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class XRSecondGrabInteractable : XRSimpleInteractable
{
    public XRTwoHandGrabInteractable mainInteractable;

    public override bool IsSelectableBy(XRBaseInteractor interactor)
    {
        bool alreadyGrabbing = !interactor.Equals(mainInteractable.selectingInteractor);
        return base.IsSelectableBy(interactor) && mainInteractable.isSelected && alreadyGrabbing;
    }
}
