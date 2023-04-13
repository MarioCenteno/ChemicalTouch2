using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeButton : MonoBehaviour
{
    [SerializeField] Material offMaterial;
    [SerializeField] Material onMaterial;

    [SerializeField] Sprite offSprite;
    [SerializeField] Sprite onSprite;


    // Start is called before the first frame update
    public void changeButtonColours()
    {
        if (this.transform.GetChild(0).GetComponent<Image>().sprite.name.Equals("moveOff"))
        {
            this.transform.GetChild(0).GetComponent<Image>().sprite = onSprite;
            this.transform.GetChild(1).GetComponent<MeshRenderer>().material = onMaterial;
        }
        else if (this.transform.GetChild(0).GetComponent<Image>().sprite.name.Equals("moveOn"))
        {
            this.transform.GetChild(0).GetComponent<Image>().sprite = offSprite;
            this.transform.GetChild(1).GetComponent<MeshRenderer>().material = offMaterial;
        }

    }
}
