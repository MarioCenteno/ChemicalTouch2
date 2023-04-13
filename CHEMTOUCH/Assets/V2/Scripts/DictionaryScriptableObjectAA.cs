using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dictionary Storage AA", menuName = "Data Objects/Dictionary Storage Object AA")]
public class DictionaryScriptableObjectAA : ScriptableObject
{
    [SerializeField]
    List<AtomV2> keys = new List<AtomV2>();
    [SerializeField]
    List<List<AtomV2>> values = new List<List<AtomV2>>();

    public List<AtomV2> Keys { get => keys; set => keys = value; }
    public List<List<AtomV2>> Values { get => values; set => values = value; }
}