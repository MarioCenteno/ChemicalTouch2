using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Dials
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] public Transform linkedDial;
        [SerializeField] private int snapRotationAmount = 25;
        [SerializeField] private float angleTolerance;
        [SerializeField] private GameObject rightHandModel;
        [SerializeField] private GameObject leftHandModel;
        [SerializeField] bool useDummyHands;

        private XRBaseInteractor interactor;
        private float startAngle;
        private bool requiresStartAngle = true;
        private bool shouldGetHandRotation = false;

        private XRGrabInteractable grabInteractor => GetComponent<XRGrabInteractable>();

        private void OnEnable()
        {
            grabInteractor.onSelectEntered.AddListener(GrabbedBy);
            grabInteractor.onSelectExited.AddListener(GrabEnd);
        }

        private void OnDisable()
        {
            grabInteractor.onSelectEntered.RemoveListener(GrabbedBy);
            grabInteractor.onSelectExited.RemoveListener(GrabEnd);
        }

        private void GrabEnd(XRBaseInteractor args0)
        {
            shouldGetHandRotation = false;
            requiresStartAngle = true;
            //HandModelVisibility(false); //for dummy hand models
        }

        private void GrabbedBy(XRBaseInteractor args0)
        {
            interactor = GetComponent<XRGrabInteractable>().selectingInteractor;
            interactor.GetComponent<XRDirectInteractor>().hideControllerOnSelect = true;

            shouldGetHandRotation = true;
            startAngle = 0f;

            //HandModelVisibility(true); //for dummy hand models
        }
        

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (shouldGetHandRotation)
            {
                var rotationAngle = GetInteractorRotation();
                GetRotationDistance(rotationAngle);
            }
        }

        public float GetInteractorRotation() => interactor.GetComponent<Transform>().eulerAngles.z;
    
        private void GetRotationDistance(float currentAngle)
        {
            if (!requiresStartAngle)
            {
                var angleDistance = Mathf.Abs(startAngle - currentAngle);
                if( angleDistance > 270f)
                {
                    float angleCheck;

                    if (startAngle < currentAngle)
                    {
                        angleCheck = CheckAngle(currentAngle, startAngle);
                        if(angleCheck < angleTolerance)
                        {
                            return;
                        }

                        else
                        {
                            RotateDialClockwise();
                            startAngle = currentAngle;
                        }
                    }

                    else if(startAngle > currentAngle)
                    {
                        angleCheck = CheckAngle(currentAngle, startAngle);
                        if (angleCheck < angleTolerance)
                        {
                            return;
                        }

                        else
                        {
                            RotateDialAntiClockwise();
                            startAngle = currentAngle;
                        }
                    }
                }
            }
            else
            {
                requiresStartAngle = false;
                startAngle = currentAngle;
            }
        }

        private float CheckAngle(float currentAngle, float startAngle) => (360f - currentAngle) + startAngle;

        private void RotateDialClockwise()
        {
            linkedDial.localEulerAngles = new Vector3(
                    linkedDial.localEulerAngles.x,
                    linkedDial.localEulerAngles.y + snapRotationAmount,
                    linkedDial.localEulerAngles.z 
                );
            if(TryGetComponent<IDial>(out IDial dial))
            {
                dial.DialChanged(linkedDial.localEulerAngles.z);
            }
        }

        private void RotateDialAntiClockwise()
        {
            linkedDial.localEulerAngles = new Vector3(
                   linkedDial.localEulerAngles.x,
                   linkedDial.localEulerAngles.y - snapRotationAmount,
                   linkedDial.localEulerAngles.z 
               );
            if (TryGetComponent<IDial>(out IDial dial))
            {
                dial.DialChanged(linkedDial.localEulerAngles.z);
            }
        }
    }
}

