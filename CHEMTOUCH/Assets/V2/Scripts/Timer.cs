using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Considerar mover este código para dentro de Atomic Manager <<<< !!!!!
public class Timer : MonoBehaviour
{

    float currentTime = 0f;
    float countdownTime = 0f;
    float startingTime = 90f;

    [SerializeField] Text timerText;
    [SerializeField] Text countdownText;

    // Start is called before the first frame update
    void Start()
    {
        countdownTime = startingTime;
        timerText.color = Color.white;
        countdownText.color = Color.yellow;
    }

    // Update is called once per frame
    void Update()
    {
        
        float minutes = Mathf.FloorToInt(countdownTime / 60);
        float seconds = Mathf.FloorToInt(countdownTime % 60);
        if (minutes < 10 && seconds < 10)
        {
            countdownText.text = "Time Left: 0" + minutes.ToString() + ":0" + seconds.ToString();
        }
        else
        {
            if (minutes < 10)
            {
                countdownText.text = "Time Left: 0" + minutes.ToString() + ":" + seconds.ToString();
            }
            else
            {
                if (seconds < 10)
                {
                    countdownText.text = "Time Left: " + minutes.ToString() + ":0" + seconds.ToString();
                }
                else
                {
                    countdownText.text = "Time Left: " + minutes.ToString() + ":" + seconds.ToString();
                }
            }

        }
        countdownTime -= 1 * Time.deltaTime;
        if (countdownTime <= 0)
        {
            countdownTime = 0;
            countdownText.color = Color.red;
            countdownText.text = "Time Left: 00:00\r\n" + "Time Up!";
        }
        float minutes2 = Mathf.FloorToInt(currentTime / 60);
        float seconds2 = Mathf.FloorToInt(currentTime % 60);
        if (minutes2 < 10 && seconds2 < 10)
        {
            timerText.text = "Time: 0" + minutes2.ToString() + ":0" + seconds2.ToString();
        }
        else
        {
            if (minutes2 < 10)
            {
                timerText.text = "Time: 0" + minutes2.ToString() + ":" + seconds2.ToString();
            }
            else
            {
                if (seconds2 < 10)
                {
                    timerText.text = "Time: " + minutes2.ToString() + ":0" + seconds2.ToString();
                }
                else
                {
                    timerText.text = "Time: " + minutes2.ToString() + ":" + seconds2.ToString();
                }
            }

        }
        currentTime += 1 * Time.deltaTime;
    }
}
