using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionDetector : MonoBehaviour
{

    [SerializeField] Material rightAnswer;
    [SerializeField] Material wrongAnswer;
    [SerializeField] Material defaultMaterial;
    [SerializeField] SubmitAnswer billboard;
    [SerializeField] bool isRight;
    [SerializeField] int level;
    [SerializeField] CheckTime checkTime;
    private int numtries=0;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Destroy(collision.gameObject);
            if (isRight)
            {
                billboard.submit(true, level);
                checkTime.hasChanged(numtries+1);
                numtries = 0;
            }

            else
            {
                this.GetComponent<MeshRenderer>().material = wrongAnswer;
                billboard.submit(false, level);
                StartCoroutine(setOriginalMaterial(this.gameObject));
                numtries++;
            }
        }
        
    }

    IEnumerator setOriginalMaterial(GameObject ob)
    {
        yield return new WaitForSeconds(1.5f);
        ob.GetComponent<MeshRenderer>().material = defaultMaterial;

    }
}
