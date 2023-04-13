using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SendTrash : MonoBehaviour
{
    public XRDirectInteractor l_hand;
    public XRDirectInteractor r_hand;

    private HistoryChange historyScript;
    private GameObject worldObjects;
    private GameObject trash;
    // Start is called before the first frame update
    private void Start()
    {
        trash = GameObject.Find("Trash");
        l_hand = GameObject.Find("LeftHandController").GetComponent<XRDirectInteractor>();
        r_hand = GameObject.Find("RightHandController").GetComponent<XRDirectInteractor>();
        worldObjects = GameObject.Find("WorldObjects");
        historyScript = GameObject.Find("GameManager").GetComponent<HistoryChange>();
    }

    // Update is called once per frame
    public void trashItem()
    {
        l_hand.enabled = false;
        r_hand.enabled = false;

        if (this.transform.parent != worldObjects.transform)
        {
            Transform parentObject = this.transform.parent;
            parentObject.transform.parent = trash.transform;
            parentObject.position = trash.transform.position;
        }
        else
        {
            this.transform.parent = trash.transform;
            this.transform.position = trash.transform.position;
        }
        //historyScript.addToHistory(worldObjects);

        l_hand.enabled = true;
        r_hand.enabled = true;
    }

    public void trashOnlyItem(GameObject go)
    {
        l_hand.enabled = false;
        r_hand.enabled = false;

        go.transform.parent = null;
        go.transform.parent = trash.transform;
        go.transform.position = trash.transform.position;
        //historyScript.addToHistory(worldObjects);

        l_hand.enabled = true;
        r_hand.enabled = true;
    }
}
