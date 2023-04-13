using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class TutorialContinue : MonoBehaviour
{
    // Start is called before the first frame update
    public InputDeviceCharacteristics controllerCharacteristics;
    public GameObject HandTeleport;
    private InputDevice targetDevice;
    public XRDirectInteractor hand;
    public GameObject HandInteractor;
    public GameObject[] Screens;
    //[SerializeField] Transform handTransform;

    private int screenCount;

    void Start()
    {
        screenCount = 0;
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
        if (targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out isprimaryButtonPressed))
        {
            Debug.Log("hiyytty");
            changeScreen();
        }
        else
        {
            Debug.Log("hawffwafwafawy");
        
        }
        //HandTeleport.SetActive(true);
    }

    public void changeScreen()
    {
        this.Screens[screenCount].SetActive(false);
        screenCount++;
        this.Screens[screenCount].SetActive(true);
    }
    
   
}
