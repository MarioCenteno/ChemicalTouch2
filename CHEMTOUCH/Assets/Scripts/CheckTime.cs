using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class CheckTime : MonoBehaviour
{
    bool changed;
    private StreamWriter file;
    public float timeOfLevel;
    public int level;
    private string fname;
    private string path;
    private int numtries;

    // Start is called before the first frame update
    void Start()
    {
        fname = System.DateTime.Now.ToString("HH-mm-ss") + ".txt";
        path = Path.Combine(Application.persistentDataPath, fname);
        file = new StreamWriter(path);
        timeOfLevel = 0;
        level = 1;
        changed = false;
        numtries = 0;
    }


    // Update is called once per frame
    void Update()
    {
        if (changed)
        {
            file.WriteLine("Desafio " + level + ": " + timeOfLevel + " Tentativas:"+numtries);
            level++;
            timeOfLevel = 0;
            numtries = 0;
            changed = false;
            file.Flush();
        }
        timeOfLevel += Time.deltaTime;
    }

    public void hasChanged(int numbertry)
    {
        changed = true;
        numtries = numbertry;
    }
}
