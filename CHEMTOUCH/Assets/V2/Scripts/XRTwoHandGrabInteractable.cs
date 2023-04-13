using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRTwoHandGrabInteractable : XRGrabInteractable
{

    public XRSimpleInteractable secondGrabPoint;
    public XRBaseInteractor secondInteractor;
    // Start is called before the first frame update
    void Start()
    {
        secondGrabPoint.onSelectEntered.AddListener(OnSecondGrab);
        secondGrabPoint.onSelectExited.AddListener(OnSecondRelease);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if(secondInteractor && selectingInteractor)
        {

        }
        base.ProcessInteractable(updatePhase);
    }

    public void OnSecondGrab(XRBaseInteractor interactor)
    {
        Debug.Log("2g");
        secondInteractor = interactor;

    }

    public void OnSecondRelease(XRBaseInteractor interactor)
    {
        Debug.Log("2r");
        secondInteractor = null;
    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        Debug.Log("1g");
        base.OnSelectEntered(interactor);
    }

    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        Debug.Log("1r");
        base.OnSelectExited(interactor);
        secondInteractor = null;
    }

    public override bool IsSelectableBy(XRBaseInteractor interactor)
    {
        bool alreadyGrabbed = selectingInteractor && !interactor.Equals(selectingInteractor);
        return base.IsSelectableBy(interactor) && !alreadyGrabbed;
    }
}
