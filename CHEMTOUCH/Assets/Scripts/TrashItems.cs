using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashItems : MonoBehaviour
{

    public bool trash;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
       if(trash)
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                Destroy(this.transform.GetChild(i).gameObject);
            }
            trash = false;
        }
    }
}
