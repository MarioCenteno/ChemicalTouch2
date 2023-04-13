using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> tasks;
    public GameObject activeTask;
    public int activeIndex;
    public GameObject prevButton;
    public GameObject nextButton;
    void Start()
    {
        activeIndex = 0;
        activeTask = tasks[activeIndex];
        activeTask.SetActive(true);
        prevButton.SetActive(false);
        if(tasks.Count < 2)
        {
            nextButton.SetActive(false);
        }
    }

    // Update is called once per frame
    public void NextTask()
    {
        if(activeIndex < tasks.Count - 1)
        {
            activeIndex+=1;
            activeTask.SetActive(false);
            activeTask = tasks[activeIndex];
            activeTask.SetActive(true);
            prevButton.SetActive(true);
            if(activeIndex == tasks.Count - 1)
            {
                nextButton.SetActive(false);
            }
        }
    }

    public void PreviousTask()
    {
        if (activeIndex > 0)
        {
            activeIndex-=1;
            activeTask.SetActive(false);
            activeTask = tasks[activeIndex];
            activeTask.SetActive(true);
            nextButton.SetActive(true);
            if (activeIndex == 0)
            {
                prevButton.SetActive(false);
            }
        }
    }
}
