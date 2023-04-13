using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkObjects : MonoBehaviour
{
    private GameObject GameManager;
    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("GameManager").gameObject;
    }

    // Update is called once per frame
    public void checkObjectsInside()
    {
        GameManager.GetComponent<checkObjectInside>().checkIfMolleculeInsideCorrect(this.gameObject);
    }
}
