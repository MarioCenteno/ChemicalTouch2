using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class TutorialHandToggle : MonoBehaviour
{
    // Start is called before the first frame update
    public InputDeviceCharacteristics controllerCharacteristics;
    public GameObject HandTeleport;
    private InputDevice targetDevice;
    public XRDirectInteractor hand;
    public GameObject HandInteractor;
    private GameObject MolecularSelector;
    private GameObject Buttons;
    private GameObject molleculePoint;
    //[SerializeField] Transform handTransform;

    private bool spawner;
    private bool buttons;

    public GameObject[] Screens;
    public bool changeScreenTime;
    //[SerializeField] Transform handTransform;

    private int screenCount;

    void Start()
    {
        changeScreenTime = true;
        screenCount = 0;
        spawner = false;
        buttons = false;
        if (this.transform.Find("Molecular Spawner").gameObject != null)
            MolecularSelector = this.transform.Find("Molecular Spawner").gameObject;
        if (this.transform.Find("ButtonsSelector").gameObject != null)
            Buttons = this.transform.Find("ButtonsSelector").gameObject;
    }

    void TryInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);


        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!targetDevice.isValid)
        {
            TryInitialize();
        }
        bool isprimaryButtonPressed = false;
        bool isgripButtonPressed = false;
        bool istriggerButtonPressed = false;
        if (targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out isprimaryButtonPressed) && isprimaryButtonPressed)
        {
            
            changeScreenForward();
        }
        else
            HandTeleport.SetActive(false);
        if (targetDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out isprimaryButtonPressed) && isprimaryButtonPressed)
        {

            changeScreenBack();
        }
        if (targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out isgripButtonPressed) && isgripButtonPressed && hand.selectTarget != null && tagIsGrabable(hand.selectTarget.tag))
        {
            HandInteractor.SetActive(false);
            if (Buttons != null && MolecularSelector != null)
            {
                Buttons.SetActive(false);
                MolecularSelector.SetActive(false);
            }
        }
        else if (targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out isgripButtonPressed) && isgripButtonPressed)
            check();
        else
        {
            StartCoroutine(setHandInteractorON());
        }
        //Debug.Log(handTransform.localEulerAngles.z);
        if (this.gameObject.transform.localEulerAngles.z < 240 && this.gameObject.transform.localEulerAngles.z > 0 && !spawner)
        {

            activateSpawner();
        }
        else if (this.gameObject.transform.localEulerAngles.z < 360 && this.gameObject.transform.localEulerAngles.z > 240 && !buttons && hand.selectTarget == null)
        {

            activateButtons();
        }

        /*if (targetDevice.TryGetFeatureValue(CommonUsages.triggerButton, out istriggerButtonPressed) && istriggerButtonPressed && hand.selectTarget == null)
        {
            MolecularSelector.SetActive(true);
            HandInteractor.SetActive(false);
        }
        else
        {
            MolecularSelector.SetActive(false);
            HandInteractor.SetActive(true);
        }*/

    }
    IEnumerator setHandInteractorON()
    {
        yield return new WaitForSeconds(0.2f);
        HandInteractor.SetActive(true);

    }

    bool tagIsGrabable(string tag)
    {
        if (tag == "Atom" || tag == "Mollecule" || tag == "Gun" || tag == "Spawnable")
            return true;
        return false;
    }
    public void check()
    {
        RaycastHit hit;
        if (Physics.Raycast(hand.gameObject.transform.position, transform.forward, out hit, 0.01f))
        {
            if (hit.transform.gameObject.tag == "Atom" && hit.transform.parent != null)
            {

                hand.enabled = false;
                molleculePoint = hit.transform.parent.Find("ConnectionPoint").gameObject;
                molleculePoint.transform.position = hit.transform.position;
                hit.transform.parent.GetComponent<XRGrabInteractable>().attachTransform = hit.transform;
                hand.enabled = true;
            }

        }
    }

    public void changeScreenForward()
    {
        if (changeScreenTime && screenCount < 35)
        {
            this.Screens[screenCount].SetActive(false);
            screenCount++;
            this.Screens[screenCount].SetActive(true);
            changeScreenTime = false;
            StartCoroutine(setChangeTrue());
        }
        else if(screenCount>=35)
            SceneManager.LoadScene("ChallengeScene");
    }
    public void changeScreenBack()
    {
        if (changeScreenTime && screenCount>0)
        {
            this.Screens[screenCount].SetActive(false);
            screenCount--;
            this.Screens[screenCount].SetActive(true);
            changeScreenTime = false;
            StartCoroutine(setChangeTrue());
        }
    }
    IEnumerator setChangeTrue()
    {
        yield return new WaitForSeconds(1.0f);
        changeScreenTime = true;

    }

    public void activateButtons()
    {
        buttons = true;
        spawner = false;
        if (Buttons != null && MolecularSelector != null)
        {
            Buttons.SetActive(true);
            MolecularSelector.SetActive(false);
        }
    }

    public void activateSpawner()
    {
        buttons = false;
        spawner = true;
        if (Buttons != null && MolecularSelector != null)
        {
            Buttons.SetActive(false);
            MolecularSelector.SetActive(true);
        }

    }
}
