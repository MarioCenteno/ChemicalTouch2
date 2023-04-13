using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Questions_manager : MonoBehaviour
{
    [SerializeField] string[] Questions;
    [SerializeField] string[] OptionsA;
    [SerializeField] string[] OptionsB;
    [SerializeField] string[] OptionsC;
    [SerializeField] string[] OptionsD;
    [SerializeField] int[] Answers;
    [SerializeField] int index;
    Text question;
    Text optionA;
    Text optionB;
    Text optionC;
    Text optionD;



    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        question = GameObject.Find("Canvas").transform.GetChild(0).GetComponent<Text>();
        question.text = Questions[index];

        optionA = GameObject.Find("Canvas").transform.GetChild(1).GetComponent<Text>();
        optionA.text = OptionsA[index];

        optionB = GameObject.Find("Canvas").transform.GetChild(2).GetComponent<Text>();
        optionB.text = OptionsB[index];

        optionC = GameObject.Find("Canvas").transform.GetChild(3).GetComponent<Text>();
        optionC.text = OptionsC[index];

        optionD = GameObject.Find("Canvas").transform.GetChild(4).GetComponent<Text>();
        optionD.text = OptionsD[index];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void next()
    {
        index++;
        question.text = Questions[index];
        optionA.text = OptionsA[index];
        optionB.text = OptionsB[index];
        optionC.text = OptionsC[index];
        optionD.text = OptionsD[index];
    }

    public bool getBool(int option)
    {
        return option == Answers[index];
    }
}
