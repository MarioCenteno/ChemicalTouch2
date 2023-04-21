using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLight : MonoBehaviour
{
    public MeshRenderer mesh;
    public Material lit;
    public Material litW;
    public Material unLit;

    public bool isLit = false;

    public void Lit()
    {
        mesh.material = lit;
        isLit = true;
    }

    public void LitW()
    {
        mesh.material = litW;
        isLit = true;
    }

    public void UnLit()
    {
        mesh.material = unLit;
        isLit = false;
    }
}
