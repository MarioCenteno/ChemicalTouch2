using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmitAnswer : MonoBehaviour
{
    [SerializeField] GameObject WrongAnswer;
    [SerializeField] GameObject RightAnswer;
    [SerializeField] GameObject WorldObjects;

    [SerializeField] GameObject[] Challenges;
    private GameObject trash;

    private int counter;

    void Start()
    {
        counter = 0;
        WorldObjects = GameObject.Find("WorldObjects");
        trash = GameObject.Find("Trash");
    }


    public void increaseLevel(int level)
    {
        Challenges[level].SetActive(false);
        Challenges[level+1].SetActive(true);
    }
    public void submit(bool check,int level)
    {
        if (check == false)
        {
            WrongAnswer.SetActive(true);
            StartCoroutine(setActiveTemporary(WrongAnswer));
            return;
        }
        else
        {
            RightAnswer.SetActive(true);
            StartCoroutine(setActiveTemporary(RightAnswer));
            clearWorld();
            increaseLevel(level);
        }


        /*
        switch (counter)
        {
            case 1:
                WrongAnswer.SetActive(true);
                StartCoroutine(setActiveTemporary(WrongAnswer));
                return;
            case 2:
                RightAnswer.SetActive(true);
                StartCoroutine(setActiveTemporary(RightAnswer));
                clearWorld();
                Challenge1.SetActive(false);
                Challenge2.SetActive(true);
                return;
            case 3:
                RightAnswer.SetActive(true);
                StartCoroutine(setActiveTemporary(RightAnswer));
                clearWorld();
                Challenge2.SetActive(false);
                StartCoroutine(setActiveAfter(Challenge3));
     
                return;
            case 4:
                RightAnswer.SetActive(true);
                StartCoroutine(setActiveTemporary(RightAnswer));
                clearWorld();
                Challenge3.SetActive(false);
                Challenge4.SetActive(true);
                return;
            case 5:
                RightAnswer.SetActive(true);
                StartCoroutine(setActiveTemporary(RightAnswer));
                clearWorld();
                Challenge4.SetActive(false);
                CompleteScreen.SetActive(true);
                return;
            case 6:
                this.gameObject.SetActive(false);
                return;
        }*/
    }

    IEnumerator setActiveTemporary(GameObject ob)
    {
        yield return new WaitForSeconds(1.5f);
        ob.SetActive(false);

    }

    IEnumerator setActiveAfter(GameObject ob)
    {
        yield return new WaitForSeconds(1.5f);
        ob.SetActive(true);

    }



    public void clearWorld()
    {
        foreach(Transform go in WorldObjects.transform)
        {
            go.parent=trash.transform;
            
        }
    }
}
