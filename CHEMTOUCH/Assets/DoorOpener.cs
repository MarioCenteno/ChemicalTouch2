using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    public Vector3 openPos;
    public Vector3 closePos;

    public List<DoorLight> lights;
    public bool doorOpen = false;
    public bool openFinished = false;
    public float speed;

    private void Update()
    {
        if (!doorOpen)
        {
            bool isOpen = true;
            foreach (DoorLight dLight in lights)
            {
                if (!dLight.isLit)
                {
                    isOpen = false;
                    break;
                }
            }

            if (isOpen)
            {
                doorOpen = true;
                openFinished = false;
            }
        }

        else
        {
            if (!openFinished)
            {
                float step = (speed * Time.deltaTime);
                transform.position = Vector3.MoveTowards(transform.position, openPos, step);
                if(transform.position == openPos)
                {
                    openFinished = true;
                }
            }

        }

    }
}
