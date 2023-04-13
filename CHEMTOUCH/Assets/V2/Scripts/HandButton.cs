using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class HandButton : XRBaseInteractable
{
    public UnityEvent OnPress = null;
    private bool previousPress = false;


    private float yMin = 0.0f;
    private float yMax = 0.0f;
    private float previousHandHeight = 0.0f;
    private XRBaseInteractor hoverInteractor = null;

    protected override void Awake()
    {
        base.Awake();
        onHoverEntered.AddListener(StartPress);
        onHoverExited.AddListener(EndPress);
    }

    protected override void OnDestroy()
    {
        onHoverEntered.RemoveListener(StartPress);
        onHoverExited.RemoveListener(EndPress);
    }

    private void StartPress(XRBaseInteractor interactor)
    {
        hoverInteractor = interactor;
        previousHandHeight = GetLocalyPosition(interactor.transform.position);
    }
    private void EndPress(XRBaseInteractor interactor)
    {
        hoverInteractor = null;
        previousHandHeight = 0.0f;
        previousPress = false;
        SetYPosition(yMax);
    }

    private void Start()
    {
        SetMinMax();
    }

    private void SetMinMax()
    {
        Collider collider = gameObject.GetComponent<Collider>();
        yMin = transform.localPosition.y - (collider.bounds.size.y*0.5f);
        yMax = transform.localPosition.y;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (hoverInteractor)
        {
            float newHandHeight = GetLocalyPosition(hoverInteractor.transform.position);
            float handDiference = previousHandHeight - newHandHeight;
            previousHandHeight = newHandHeight;
            float newPosition = transform.localPosition.y - handDiference;
            SetYPosition(newPosition);

            CheckPress();
        }
    }

    private float GetLocalyPosition(Vector3 positon)
    {

        Vector3 localPosition = transform.root.InverseTransformPoint(positon);
        return localPosition.y;
    }

    private void SetYPosition(float position)
    {
        Vector3 newPosition = transform.localPosition;
        newPosition.y = Mathf.Clamp(position, yMin, yMax);
        transform.localPosition = newPosition;
    }

    private void CheckPress()
    {
        bool inPosition = InPosition();
        if (inPosition && inPosition != previousPress)
        {
            Debug.Log("yay");
            OnPress.Invoke();
        }

        previousPress = inPosition;
    }

    private bool InPosition()
    {
        float inRange = Mathf.Clamp(transform.localPosition.y, yMin, yMin + 0.01f);
        return transform.localPosition.y == inRange;
    }

}
