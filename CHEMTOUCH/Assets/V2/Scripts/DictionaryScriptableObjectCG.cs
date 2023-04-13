using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dictionary Storage CG", menuName = "Data Objects/Dictionary Storage Object CG")]
public class DictionaryScriptableObjectCG : ScriptableObject
{
    [SerializeField]
    List<(AtomV2, AtomV2)> keys = new List<(AtomV2, AtomV2)>();
    [SerializeField]
    List<ConnectionV2> values = new List<ConnectionV2>();

    public List<(AtomV2, AtomV2)> Keys { get => keys; set => keys = value; }
    public List<ConnectionV2> Values { get => values; set => values = value; }
}