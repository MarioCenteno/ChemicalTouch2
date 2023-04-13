using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class updateTrash : MonoBehaviour
{
    private GameObject trash;

    private void Start()
    {
        trash = GameObject.Find("Trash");
    }
    // Start is called before the first frame update
    public void trashItems()
    {
        trash.GetComponent<TrashItems>().trash = true;
    }
}
