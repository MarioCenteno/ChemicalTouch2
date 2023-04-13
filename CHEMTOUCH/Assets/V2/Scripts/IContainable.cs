using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
[SerializeField]
public interface IContainable
{
    [SerializeField]
    bool IsContained { get; set; }
    [SerializeField]
    void Contain(Container cont);
    [SerializeField]
    void Release();

}
