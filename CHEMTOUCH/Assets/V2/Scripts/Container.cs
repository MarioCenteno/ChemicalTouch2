using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    public AtomicManager manager;
    public ContainerLock myLock;
    public GameObject Lid;
    [SerializeField]
    public List<IContainable> containedObjects = new List<IContainable>();
    public List<AtomV2> containedAtoms = new List<AtomV2>();
    public List<MoleculeV2> containedMolecules = new List<MoleculeV2>();
    public List<Containable> containables = new List<Containable>();
    public List<InkCanister> canisters =new List<InkCanister>();
    public List<InkBrush> brushes = new List<InkBrush>();

    private void Awake()
    {
        foreach (AtomV2 a in containedAtoms)
        {
            containedObjects.Add(a);
        }
        foreach (MoleculeV2 m in containedMolecules)
        {
            containedObjects.Add(m);
        }
        foreach (Containable co in containables)
        {
            containedObjects.Add(co);
        }
        foreach (InkCanister ca in canisters)
        {
            containedObjects.Add(ca);
        }
        foreach (InkBrush b in brushes)
        {
            containedObjects.Add(b);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach (AtomV2 a in containedAtoms)
        {
            containedObjects.Add(a);
        }
        foreach (MoleculeV2 m in containedMolecules)
        {
            containedObjects.Add(m);
        }
        foreach (Containable co in containables)
        {
            containedObjects.Add(co);
        }
        foreach (InkCanister ca in canisters)
        {
            containedObjects.Add(ca);
        }
        foreach (InkBrush b in brushes)
        {
            containedObjects.Add(b);
        }

        foreach (IContainable obj in containedObjects)
        {
            obj.Contain(this);
        }
    }

    public void OpenContainer()
    {
        Lid.SetActive(false);
        foreach (IContainable obj in containedObjects)
        {
            obj.Release();
        }
        
    }
}
