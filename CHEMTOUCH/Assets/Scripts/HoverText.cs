using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class HoverText : MonoBehaviour
{
    private TextMeshProUGUI text;
    [SerializeField] string atomName;
    // Start is called before the first frame update
    void Start()
    {
        text = this.transform.parent.transform.parent.Find("AtomTypeText").GetChild(0).GetComponent<TextMeshProUGUI>();    
    }

    // Update is called once per frame


    public void enterhover()
    {
        text.text = atomName;
    }

    public void exithover()
    {
        text.text = "";
    }
}
