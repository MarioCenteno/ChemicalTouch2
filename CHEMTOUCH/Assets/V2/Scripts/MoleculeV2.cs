using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MoleculeV2 : MonoBehaviour, IContainable
{
    [SerializeField]
    public AtomicManager manager;
    [SerializeField]
    public XRGrabInteractable interactable;
    [SerializeField]
    public AtomV2 root;
    [SerializeField]
    public List<AtomV2> atoms;
    [SerializeField]
    public List<ConnectionV2> connections;

    [SerializeField]
    public SerializableDictionary<AtomV2, List<AtomV2>> adjacentAtoms = new SerializableDictionary<AtomV2, List<AtomV2>>();

    [SerializeField]    
    public SerializableDictionary<(AtomV2, AtomV2), ConnectionV2> connectionGraph = new SerializableDictionary<(AtomV2, AtomV2), ConnectionV2>();
    [SerializeField] 
    public bool needsUpdate;
    [SerializeField] 
    public Transform atomParent;
    [SerializeField] 
    public List<AtomV2> chiralCarbons;
    [SerializeField] 
    public bool isGrabed = false;
    [SerializeField] 
    public bool isConnecting = false;
    [SerializeField] 
    public bool minimized = false;
    [SerializeField]
    public static readonly Dictionary<(int, int, int, int), bool> IsClockwise = new Dictionary<(int, int, int, int), bool>()
        {
            {(4,1,3,2),true},
            {(4,3,2,1),true},
            {(4,2,1,3),true},
            {(1,4,2,3),true},
            {(3,4,1,2),true},
            {(2,4,3,1),true},
            {(1,3,4,2),true},
            {(3,2,4,1),true},
            {(2,1,4,3),true},
            {(1,2,3,4),true},
            {(3,1,2,4),true},
            {(2,3,1,4),true},
            {(4,1,2,3),false},
            {(4,3,1,2),false},
            {(4,2,3,1),false},
            {(1,4,3,2),false},
            {(3,4,2,1),false},
            {(2,4,1,3),false},
            {(1,2,4,3),false},
            {(3,1,4,2),false},
            {(2,3,4,1),false},
            {(1,3,2,4),false},
            {(3,2,1,4),false},
            {(2,1,3,4),false}
        };

    [SerializeField] 
    bool isContained = false;

    public bool modifyValues = false;
    public bool IsContained { get => isContained; set => isContained = value; }


    private void Awake()
    {
       
        manager = GameObject.FindGameObjectWithTag("AtomManager").GetComponent<AtomicManager>();
        
    }

    


    public void minimize()
    {
        minimized = true;
        foreach (AtomV2 atom in atoms)
        {
            atom.minimize();
        }
        foreach (ConnectionV2 connection in connections)
        {
            connection.minimize();
        }
        gameObject.transform.localScale = manager.minimizeSize;
    }
    public void maximize()
    {
        minimized = false;
        foreach (AtomV2 atom in atoms)
        {
            atom.maximize();
        }
        foreach (ConnectionV2 connection in connections)
        {
            connection.maximize();            
        }
        gameObject.transform.localScale = manager.maximizeSize;
    }

    public void Grabed()
    {
        isGrabed = true;
        if (!minimized)
        {
            foreach (AtomV2 atom in atoms)
            {
                atom.Grabed();
            }
        }
        
    }
    public void LetGo()
    {
        isGrabed = false;
        if (!minimized)
        {
            foreach (AtomV2 atom in atoms)
            {
                atom.LetGo();
            }
        }
        isConnecting = false;
    }


    
    public void InitMolecule()
    {
        adjacentAtoms = new SerializableDictionary<AtomV2, List<AtomV2>>();
        connectionGraph = new SerializableDictionary<(AtomV2, AtomV2), ConnectionV2>();
        manager = GameObject.FindGameObjectWithTag("AtomManager").GetComponent<AtomicManager>();
    }

    public void mergeInto(MoleculeV2 molecule)
    {
        foreach (AtomV2 atom in molecule.atoms)
        {
            addAtom(atom);
        }
        foreach (ConnectionV2 connection in molecule.connections)
        {
            addConnection(connection);
        }
    }

    public void findChiralCarbons()
    {
        Debug.Log("start chiral search");
        chiralCarbons = new List<AtomV2>();
        foreach (AtomV2 atom in atoms)
        {
            bool isChiral = false;
            if (atom.type == AtomType.Carbon)
            {
                if (atom.currentConections == 4)
                {
                    List<AtomV2> conectedAtoms = adjacentAtoms[atom];
                    int[] ocurences = numberOfAtomTypeOcurences(conectedAtoms);
                    if (ocurences[((int)AtomType.Hydrogen)] <= 1)
                    {
                        //TODO HASH sub trees to compare https://www.baeldung.com/cs/hashing-tree
                        //keep in mind cycles and as such add a visited atoms list in the recursive hash function
                        List<double> hashes = new List<double>();
                        isChiral = true;
                        foreach (AtomV2 child in adjacentAtoms[atom])
                        {
                            List<AtomV2> visited = new List<AtomV2>();
                            visited.Add(atom);
                            double sugGraphHash = GetSubGraphHash(atom, child, visited);
                            //Debug.Log(sugGraphHash + "hash", child.gameObject);
                            if (hashes.Contains(sugGraphHash))
                            {
                                isChiral = false;
                                break;
                            }
                            else
                            {
                                hashes.Add(sugGraphHash);
                            }
                        }
                        if (isChiral)
                        {
                            chiralCarbons.Add(atom);
                            Debug.Log("ISCHIRAL",atom.gameObject);
                            //atom.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.grey);
                        }
                        else
                        {
                            //Debug.Log("NOTCHIRAL");
                        }
                    }
                }
            }
        }
        Debug.Log("END Chiral Search");
    }

    public int[] numberOfAtomTypeOcurences(List<AtomV2> atomsToCheck)
    {
        int[] occurences = new int[Enum.GetNames(typeof(AtomType)).Length];
        IList<AtomV2> tempAtoms = atomsToCheck;
        foreach (AtomType type in (AtomType[])Enum.GetValues(typeof(AtomType)))
        {
            occurences[((int)type)] = tempAtoms.Where(a => a.type == type).Count();
        }
        return occurences;
    }

    public double GetSubGraphHash(AtomV2 root, AtomV2 subRoot, List<AtomV2> visited, string debugString = "", int depth = 0)
    {
        double SEED = 2;
        double MOD = 54321;
        double myHash = 0;
        myHash = ((double)subRoot.type) + (connectionGraph[(root, subRoot)].size - 1) * Math.Pow(SEED, depth) % MOD; // We need to consider connection type in hash
        List<AtomV2> ChildrenNotVisited = new List<AtomV2>();
        if (adjacentAtoms.ContainsKey(subRoot))
        {
            ChildrenNotVisited = adjacentAtoms[subRoot].Except(visited).ToList();
        }
        else
        {
            return myHash;
        }
        if (ChildrenNotVisited.Count() == 0)
        {
            //Debug.Log(debugString + "con-" + connectionGraph[(root, subRoot)].size + "atom-" + subRoot.type + ":" + myHash);
            return myHash;
        }
        else
        {
            visited.Add(subRoot);
            List<double> ChildrenHash = new List<double>();
            debugString += "\t";
            foreach (AtomV2 atom in ChildrenNotVisited)
            {
                ChildrenHash.Add(GetSubGraphHash(subRoot, atom, visited, debugString, depth + 1));
            }
            ChildrenHash.Sort();
            int i = 0;

            foreach (double childHash in ChildrenHash)
            {
                myHash = myHash + childHash * Math.Pow(SEED, i) % MOD;
                i++;
            }
            //Debug.Log(debugString + "con-" + connectionGraph[(root, subRoot)].size + "atom-" + subRoot.type + ":" + myHash);
            return myHash;
        }


    }

    public bool IsChiral(AtomV2 atom)
    {
        bool isChiral = false;
        List<AtomV2> connectedAtoms = adjacentAtoms[atom];

        foreach (AtomV2 connectedAtom in adjacentAtoms[atom])
        {

        }
        return isChiral;
    }
    public void addAtom(AtomV2 atom, bool isFirst = false)
    {
        atom.molecule = this;
        atoms.Add(atom);
        atom.parentTransform = atomParent;
        atom.needTransfromUpdate = true;

        gameObject.GetComponent<XRGrabInteractable>().colliders.Add(atom.myCollider);
        needsUpdate = true;
        if (isFirst)
        {
            root = atom;
            interactable.attachTransform = root.gameObject.transform;
        }
    }

    public void addConnection(ConnectionV2 connection)
    {
        connection.molecule = this;
        connection.SetParent(this);
        connections.Add(connection);
        addGraphConnection(connection.A1, connection.A2, connection);
        addGraphConnection(connection.A2, connection.A1, connection);
    }

    public void addGraphConnection(AtomV2 A1, AtomV2 A2, ConnectionV2 Con)
    {
        if (adjacentAtoms.ContainsKey(A1))
        {
            if (!adjacentAtoms[A1].Contains(A2))
            {
                adjacentAtoms[A1].Add(A2);
            }
        }
        else
        {
            adjacentAtoms.Add(A1, new List<AtomV2>());
            adjacentAtoms[A1].Add(A2);
        }
        if (connectionGraph.ContainsKey((A1, A2)))
        {
            connectionGraph[(A1, A2)] = Con;
        }
        else
        {
            connectionGraph.Add((A1, A2), Con);
        }
    }

    private void Start()
    {
        interactable.interactionManager.UnregisterInteractable(interactable);
        interactable.interactionManager.RegisterInteractable(interactable);
        manager = GameObject.FindGameObjectWithTag("AtomManager").GetComponent<AtomicManager>();
    }
    private void Update()
    {

        if (needsUpdate)
        {
            //interactable.interactionManager.UnregisterInteractable(interactable);
            //interactable.interactionManager.RegisterInteractable(interactable);
            atoms.RemoveAll(item => item == null);
            connections.RemoveAll(item => item == null);
            //findChiralCarbons();
            needsUpdate = false;
        }
    }

    public void RemoveAtom(AtomV2 atom)
    {
        atom.molecule = null;
        atoms.Remove(atom);
        adjacentAtoms.Remove(atom);
        atom.parentTransform = atom.manager.transform;
        atom.needTransfromUpdate = true;
        gameObject.GetComponent<XRGrabInteractable>().colliders.Remove(atom.myCollider);
        needsUpdate = true;
    }
    public void RemoveConnection(ConnectionV2 connection)
    {
        connection.molecule = null;
        

        connection.SetParent(null);
        connections.Remove(connection);
        RemoveGraphConnection(connection.A1, connection.A2);
        RemoveGraphConnection(connection.A2, connection.A1);
    }

    public void RemoveGraphConnection(AtomV2 A1, AtomV2 A2)
    {
        connectionGraph.Remove((A1, A2));
        if (adjacentAtoms.ContainsKey(A1))
        {
            adjacentAtoms[A1].Remove(A2);
        }
    }


    

    public bool BreakConnection(ConnectionV2 connection, AtomV2 a1, AtomV2 a2)
    {
        Debug.Log("BREAK START", connection);   
        connections.Remove(connection);
        connectionGraph.Remove((a1, a2));
        connectionGraph.Remove((a2, a1));
        adjacentAtoms[a1].Remove(a2);
        adjacentAtoms[a2].Remove(a1);
        Dictionary<(AtomV2, AtomV2), ConnectionV2> subgraph1 = new Dictionary<(AtomV2, AtomV2), ConnectionV2>();
        Dictionary<(AtomV2, AtomV2), ConnectionV2> subgraph2 = new Dictionary<(AtomV2, AtomV2), ConnectionV2>();
        List<AtomV2> atomsSub1 = new List<AtomV2>();
        List<AtomV2> atomsSub2 = new List<AtomV2>();
        //Dictionary<AtomV2, List<AtomV2>> adjancencyListsSub1 = new Dictionary<AtomV2, List<AtomV2>>();
        //Dictionary<AtomV2, List<AtomV2>> adjancencyListsSub2 = new Dictionary<AtomV2, List<AtomV2>>();
        List<AtomV2> visited = new List<AtomV2>();
        atomsSub1.Add(a1);
        GetSubGraph(a1, visited, subgraph1, atomsSub1);
        if (visited.Contains(a2)) // Was cyclic connection
        {
            Debug.Log("Cyclic");
            Debug.Log("BREAK END", connection);
            return true;
        }
        else //split molecule
        {
            //Debug.Log("spliting");
            atomsSub2.Add(a2);
            GetSubGraph(a2, visited, subgraph2, atomsSub2);
            List<AtomV2> remainingAtoms = atoms.Except(visited).ToList();
            if (remainingAtoms.Count != 0)
            {
                Debug.Log("something wrong is not right");
                connections.Add(connection);
                connectionGraph.Add((a1, a2),connection);
                connectionGraph.Add((a2, a1),connection);
                adjacentAtoms[a1].Add(a2);
                adjacentAtoms[a2].Add(a1);
                return false;
            }
            if (atomsSub1.Contains(root))
            {
                Debug.Log("from sub2");
                //make sub2 new molecule
                RemoveSubGraph(subgraph2,atomsSub2);
                CreateMoleculeFromSubGraph(a2,subgraph2,atomsSub2);
            }
            else if(atomsSub2.Contains(root))
            {
                Debug.Log("from sub1");
                //make sub1 new molecule
                RemoveSubGraph(subgraph1, atomsSub1);
                CreateMoleculeFromSubGraph(a1, subgraph1, atomsSub1);
            }
            Debug.Log("BREAK END", connection);
            return true;
        }
        
    }
    public void CreateMoleculeFromSubGraph(AtomV2 newRoot, Dictionary<(AtomV2, AtomV2), ConnectionV2> newSubGraph, List<AtomV2> atomsInSub)
    {
        MoleculeV2 newMolecule = Instantiate(root.manager.moleculePrefab, gameObject.transform.position, Quaternion.identity).GetComponent<MoleculeV2>();
        newMolecule.InitMolecule();
        newMolecule.transform.position = newRoot.gameObject.transform.position;
        newMolecule.transform.rotation = newRoot.gameObject.transform.rotation;
        newMolecule.interactable.attachTransform = newRoot.gameObject.transform;
        atomsInSub.Remove(newRoot);
        newMolecule.addAtom(newRoot,true);
        foreach (AtomV2 atom in atomsInSub.Except(new List<AtomV2>()))
        {
            newMolecule.addAtom(atom);
        }
        foreach (ConnectionV2 connection in newSubGraph.Values.Except(new List<ConnectionV2>()))
        {
            newMolecule.addConnection(connection);
        }
        newMolecule.needsUpdate = true;
        newMolecule.interactable.interactionManager.UnregisterInteractable(newMolecule.interactable);
        newMolecule.interactable.interactionManager.RegisterInteractable(newMolecule.interactable);
        newMolecule.atoms.RemoveAll(item => item == null);
        newMolecule.connections.RemoveAll(item => item == null);
        //Debug.Log("I:", gameObject);
        //Debug.Log("created:", newMolecule.gameObject);
        newMolecule.LetGo();
    }
    public void UpdateGraphs(Dictionary<(AtomV2, AtomV2), ConnectionV2> newSubGraph, List<AtomV2> atomsInSub)
    {
        atoms = atomsInSub;
        connections = newSubGraph.Values.Except(new List<ConnectionV2>()).ToList();
    }


    public void RemoveSubGraph(Dictionary<(AtomV2, AtomV2), ConnectionV2> subGraphToRemove, List<AtomV2> atomsInSub)
    {
        foreach(ConnectionV2 connection in subGraphToRemove.Values.Except(new List<ConnectionV2>()))
        {
            RemoveConnection(connection);
        }
        foreach(AtomV2 atom in atomsInSub.Except(new List<AtomV2>()))
        {
            RemoveAtom(atom);
        }
        atoms.RemoveAll(item => item == null);
        connections.RemoveAll(item => item == null);
        bool wasGrabbed = false;
        XRBaseInteractor grabbingInteractor = null;
        needsUpdate = true;
        if(isGrabed)
        {
            wasGrabbed = true;
            grabbingInteractor = interactable.selectingInteractor;
            interactable.interactionManager.SelectCancel(grabbingInteractor, interactable);
        }
        else
        {
            //LetGo();
        }
        interactable.interactionManager.UnregisterInteractable(interactable);
        interactable.interactionManager.RegisterInteractable(interactable);
        if (wasGrabbed)
        {
            interactable.interactionManager.SelectEnter(grabbingInteractor, interactable);
        }
    }
    public void GetSubGraph(AtomV2 root, List<AtomV2> visited, Dictionary<(AtomV2, AtomV2), ConnectionV2> newSubGraph,List<AtomV2> atomsInSub)
    {
        visited.Add(root);
        List<AtomV2> ChildrenNotVisited = adjacentAtoms[root].Except(visited).ToList();
        if (ChildrenNotVisited.Count() == 0)
        {
            return;
        }
        foreach (AtomV2 atom in ChildrenNotVisited)
        {
            atomsInSub.Add(atom);
            if(connectionGraph.ContainsKey((root,atom)))
                newSubGraph.Add((root, atom), connectionGraph[(root, atom)]);
            if (connectionGraph.ContainsKey((atom, root)))
                newSubGraph.Add((atom, root), connectionGraph[(atom, root)]);
            GetSubGraph(atom, visited, newSubGraph,atomsInSub);
        }
    }

    public void Contain(Container cont)
    {
        if (minimized)
        {
            if (!isGrabed)
            {
                if (!IsContained)
                {
                    IsContained = true;
                    foreach (AtomV2 atom in atoms)
                    {
                        atom.IsContained = true;
                    }

                    interactable.interactionLayerMask = LayerMask.GetMask("Nothing");

                    gameObject.transform.SetParent(cont.transform);
                }
            }
            
        }
      
    }

    public void Release()
    {
        IsContained = false;
        foreach (AtomV2 atom in atoms)
        {
            atom.IsContained = true;
        }
        interactable.interactionLayerMask = ~0;
        gameObject.transform.SetParent(root.manager.ObjParent);

    }

    /** /
private void TryAddAjacencyListElement(Dictionary<AtomV2, List<AtomV2>> adjancencyList,AtomV2 key, AtomV2 element)
{
   if (adjancencyList.ContainsKey(key)) 
   {
       if (!adjacentAtoms[key].Contains(element))
       {
           adjancencyList[key].Add(element);
       }         
   }
   else
   {
       adjancencyList.Add(key, new List<AtomV2>());
       adjancencyList[key].Add(element);
   }
}
/**/
}
