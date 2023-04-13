using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementMenu : MonoBehaviour
{
    // Start is called before the first frame update





    //Verificar se vale a pena fazer este como serialized
    public List<GameObject> elements;

    public AtomV2 currentAtom;

    public GameObject atom;


    public GameObject activeElement;

    void Start()
    {
        activeElement = elements[0];
        activeElement.SetActive(true);
        atom = (GameObject)activeElement.GetComponent<ElementBox>().atom;


    }

    // Update is called once per frame
    public void ChangeElement(int element)
    {

            activeElement.SetActive(false);
            activeElement = elements[element];
            activeElement.SetActive(true);
            atom = (GameObject)activeElement.GetComponent<ElementBox>().atom;


    }

    public GameObject GetAtom()
    {
        return atom;
    }

 // Adicionar possível variável de retorno do tipo de atomo a devolver



//Ver classe AtomV2
}
