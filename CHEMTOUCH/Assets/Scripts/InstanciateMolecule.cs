

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class InstanciateMolecule : MonoBehaviour
{
    public Transform molleculeSpawn;
    public GameObject mollecule;
    public GameObject molleculePrefab;
    bool done = false;




    // Update is called once per frame

    void Update()
    {
        float Distance = Vector3.Distance(mollecule.transform.position, molleculeSpawn.position);
       if(Distance>0.1)
           {
          
            GameObject spawnMolecule = Instantiate(molleculePrefab, molleculeSpawn.position, molleculeSpawn.rotation, this.transform);
            mollecule = spawnMolecule;
            this.transform.parent.gameObject.SetActive(false);        
            }
    }
}
