using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomicCreation : MonoBehaviour
{
    public bool m_Started = false;
    public LayerMask m_LayerMask;
    public float posMod = 1;
    public float radiusMod = 1;


    public ElementMenu menu;

    public GameObject atom;

    void Start()
    {


    }

    public void CreateAtom()
    {


        /*
        Vector3 botom = gameObject.transform.position - new Vector3(0, transform.localScale.y * posMod, 0);
        Vector3 top = gameObject.transform.position + new Vector3(0, transform.localScale.y * posMod, 0);
        Collider[] hitColliders = Physics.OverlapCapsule(botom, top, transform.localScale.x * 0.5f * radiusMod, m_LayerMask);*/
        //Check when there is a new collider coming into contact with the box

        // 1 - Verificação de Ecrã pressionado - Não é necessário - ele recolhe automáticamente o tipo da molecula a partir da ligação ao quadro no unity


        //Verificar se é mais fácil clonar os atomos establecido em vez de criar completamente novos.

        // 2 - Criação da Molecula - Verificar se isto cria um objeto tridimensional ou é necessário que isto seja um game object
        atom = (GameObject)menu.atom;

        Vector3 pos = new Vector3((float)-0.844, (float)2, (float)0.821);

        Instantiate(atom, pos, Quaternion.identity);

  
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (m_Started)
        {
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Vector3 botom = gameObject.transform.position - new Vector3(0, transform.localScale.y * posMod, 0);
            Vector3 top = gameObject.transform.position + new Vector3(0, transform.localScale.y * posMod, 0);
            float radius = transform.localScale.x * 0.5f * radiusMod;
            Gizmos.DrawWireSphere(botom, radius);
            Gizmos.DrawWireSphere(top, radius);
        }
        

    }
}
