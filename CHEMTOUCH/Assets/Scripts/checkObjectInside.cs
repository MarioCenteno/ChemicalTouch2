using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkObjectInside : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Material rightAnswer;
    [SerializeField] Material WrongAnswer;
    [SerializeField] Material defaultMaterial;
    [SerializeField] GameObject cylinder;
    [SerializeField] SubmitAnswer billboard;
    [SerializeField] GameObject[] Answers;
    [SerializeField] int level;
    [SerializeField] GameObject GunGO;
    [SerializeField] GameObject GunText;
    [SerializeField] GameObject SubmitText;
    [SerializeField] GameObject SubmitAnswerGO;
    [SerializeField] CheckTime checkTime;
    private int numtries=0;
    // Update is called once per frame


    public void checkIfMolleculeInsideCorrect(GameObject mollecule)
    {
        //Debug.Log(mollecule.transform.position);
        //Debug.Log(mollecule.tag);
        if (cylinder.GetComponent<Collider>().bounds.Contains(mollecule.transform.position) && mollecule.tag == "Mollecule" && mollecule.activeInHierarchy)
        {
            if (checkCorrect(mollecule))
            {
                cylinder.GetComponent<MeshRenderer>().material = rightAnswer;
                mollecule.transform.position = new Vector3(0, 0, -100);
                billboard.submit(true, level);
                level++;
                checkTime.hasChanged(numtries+1);
                numtries = 0;
            }
            else
            {
                cylinder.GetComponent<MeshRenderer>().material = WrongAnswer;
                billboard.submit(false, level);
                numtries++;
            }
            StartCoroutine(setOriginalMaterial(cylinder));
        }
        if (level >= 6)
        {
            GunGO.SetActive(true);
            GunText.SetActive(true);
            SubmitText.SetActive(false);
            SubmitAnswerGO.SetActive(false);
        }
    }

    IEnumerator setOriginalMaterial(GameObject ob)
    {
        yield return new WaitForSeconds(1.5f);
        ob.GetComponent<MeshRenderer>().material = defaultMaterial;

    }

    public bool checkCorrect(GameObject mollecule)
    {
        int atomCount = 0;
        int correctCount = 0;
        GameObject molleculeCheck = mollecule;
        foreach(Transform obj in molleculeCheck.transform)
        {
            if (obj.tag == "Atom")
            {
                atomCount++;
                Atom atom1 = obj.GetComponent<Atom>();
                foreach(Transform obj2 in Answers[level].transform)
                {

                    if (obj2.tag == "Atom")
                    {

                        //Debug.Log("YO");
                        Atom atom2 = obj2.GetComponent<Atom>();
                       // Debug.Log("Atom2Type____>"+atom2.GetAtomType());
   
                        if (atom2.GetAtomType() == atom1.GetAtomType() && atom2.numConnections == atom1.numConnections &&
                            atom2.numeletroes == atom1.numeletroes && haveSameConnectingAtoms(atom1.connectingAtoms, atom2.connectingAtoms)) 
                        {   
                            correctCount++; 
                            obj2.tag = "AtomChecked"; 
                            break; 
                        }                           
                    }
                    
                }
            }
        }
        foreach (Transform obj2 in Answers[level].transform)
        {

            if (obj2.tag == "AtomChecked")
            {
                obj2.tag = "Atom";
            }
        }
        if (atomCount == correctCount)
        {
            return true;
        }
        else
        {
            Debug.Log(correctCount);
            Debug.Log(atomCount);
            
            return false;
        }
    }

    public bool haveSameConnectingAtoms(List<Atom.connectionAtom> dictionary1, List<Atom.connectionAtom> dictionary2)
    {
        bool answer = false;
        int dict1Checked = 0;
        int dict2Checked = 0;
        List<Atom.connectionAtom> dict1 =dictionary1;
        List<Atom.connectionAtom> dict2 = dictionary2;
        Debug.Log("Dictionary 1 size: "+dict1.Count);
        Debug.Log("Dictionary 2 size: " + dict2.Count);

        for (int i=0; i < dict1.Count; i++)
        {
            dict1Checked++;
            for(int j=0; j<dict2.Count; j++)
            {
                if (dict1[i].Type == dict2[j].Type && !dict1[i].Checked && !dict2[j].Checked)
                {
                    dict1[i].changeChecked();
                    dict2[j].changeChecked();
                    dict2Checked++;
                    break;
                }
            }
        }
        if (dict1Checked==dict2Checked) { answer=true;}
        else
            answer=false;
        return answer;
    }

}
