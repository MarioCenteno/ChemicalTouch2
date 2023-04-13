using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[System.Serializable]
public enum AtomType
{
    Hydrogen,
    Carbon,
    Oxygen,
    Nitrogen,
    Chlorine,
    Sulfur,
    Iodine,
    Bromine,
    Empty,
}
public class AtomV2 : MonoBehaviour, IContainable
{
    [SerializeField]
    public XRGrabInteractable Interactable;
    [SerializeField] 
    public AtomicManager manager;
    [SerializeField] 
    public MoleculeV2 molecule = null;
    [SerializeField] 
    public List<ConnectionV2> connections;
    [SerializeField] 
    public int maxConnections = 4;
    [SerializeField] 
    public int currentConections = 0;
    [SerializeField] 
    public ConnectionV2 pendingConnection = null;
    [SerializeField] 
    public AtomV2 connectingAtom = null;
    [SerializeField] 
    public bool isConnecting = false;
    [SerializeField] 
    public bool isGrabed = false;
    [SerializeField] 
    public bool isMainConector = false;
    [SerializeField] 
    public int weight;
    [SerializeField] 
    public AtomType type;
    [SerializeField] 
    public Transform parentTransform;
    [SerializeField] 
    public bool needTransfromUpdate = false;
    [SerializeField] 
    public Material lineMat;
    [SerializeField] 
    public List<ElectronV2> electrons;
    [SerializeField] 
    public Collider myCollider;
    [SerializeField] 
    public bool minimized = false;
    [SerializeField] 
    public MeshRenderer MyMesh;
    [SerializeField] 
    public bool isContained = false;
    public bool IsContained { get => isContained; set => isContained = value; }

    private void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("AtomManager").GetComponent<AtomicManager>();
        Interactable = gameObject.GetComponent<XRGrabInteractable>();

    }

    private void Start()
    {
        Interactable = gameObject.GetComponent<XRGrabInteractable>();
        manager = GameObject.FindGameObjectWithTag("AtomManager").GetComponent<AtomicManager>();
        MyMesh.material = manager.atomProprieties[((int)type)].material;
        foreach (ElectronV2 electron in electrons)
        {
            electron.gameObject.SetActive(false);
        }
        if (minimized)
        {
            if (molecule == null)
            {
                gameObject.transform.localScale = manager.minimizeSize;
            }
            
        }
        else
        {
            if (molecule == null)
            {
                gameObject.transform.localScale = manager.maximizeSize;
            }
            if (isGrabed)
            {
                for (int i = 0; i != manager.atomProprieties[(int)type].connectionLimit; i++)
                {
                    if (i >= electrons.Count) break;
                    if (currentConections >= manager.atomProprieties[(int)type].connectionLimit) break;

                    if (!electrons[i].connected)
                        electrons[i].gameObject.SetActive(true);
                }
            }
            
        }
        
        
    }

    private void Update()
    {
        if (needTransfromUpdate)
        {
            gameObject.transform.parent = parentTransform;
            needTransfromUpdate = false;
        }
        foreach (ElectronV2 electron in electrons)
        {
            if (electron.connected)
                electron.gameObject.SetActive(false);
        }

        if (isConnecting)
        {
            if (isMainConector)
            {
                if ((gameObject.transform.position - connectingAtom.transform.position).magnitude <= 0.5)
                {
                    pendingConnection.line4.startColor = Color.red;
                    pendingConnection.line4.endColor = Color.red;
                }
                if ((gameObject.transform.position - connectingAtom.transform.position).magnitude > 0.5)
                {
                    pendingConnection.line4.startColor = Color.blue;
                    pendingConnection.line4.endColor = Color.blue;
                }
                if ((gameObject.transform.position - connectingAtom.transform.position).magnitude > 1)
                {
                    manager.OnCancelConnection.Invoke(pendingConnection);
                    ConnectionV2 temp = pendingConnection;
                    connectingAtom.removePendingConnection();
                    removePendingConnection();
                    temp.freeElectrons();
                    Destroy(temp.gameObject);
                }
            }
        }
    }

    public void minimize()
    {
        minimized = true;
        if(molecule == null)
        {
            gameObject.transform.localScale = manager.minimizeSize;
        }
        foreach (ElectronV2 electron in electrons)
        {
            electron.gameObject.SetActive(false);
        }
    }
    public void maximize()
    {
        minimized = false;
        if (molecule == null)
        {
            gameObject.transform.localScale = manager.maximizeSize;
        }
        if(isGrabed)
        {
            for (int i = 0; i != manager.atomProprieties[(int)type].connectionLimit; i++)
            {
                if (i >= electrons.Count) break;
                if (currentConections >= manager.atomProprieties[(int)type].connectionLimit) break;
                if (!electrons[i].connected)
                    electrons[i].gameObject.SetActive(true);
            }
        }
        
    }


    public void HideElectrons()
    {
        foreach (ElectronV2 electron in electrons)
        {
            electron.gameObject.SetActive(false);
        }
        if (isGrabed)
        {
            for (int i = 0; i != manager.atomProprieties[(int)type].connectionLimit; i++)
            {
                
                if (i >= electrons.Count) break;
                if (currentConections >= manager.atomProprieties[(int)type].connectionLimit) break;
                if (!electrons[i].connected)
                    electrons[i].gameObject.SetActive(true);
            }
        }
    }

    public void StartPendingConnection(AtomV2 connectingAtom, ConnectionV2 pendingConnection, bool isMain)
    {
        this.connectingAtom = connectingAtom;
        this.pendingConnection = pendingConnection;
        isMainConector = isMain;
        isConnecting = true;
    }

    public void Grabed()
    {
        isGrabed = true;
        if (!minimized)
        {
            for (int i = 0; i != manager.atomProprieties[(int)type].connectionLimit; i++)
            {
                if (i >= electrons.Count) break;
                if (currentConections >= manager.atomProprieties[(int)type].connectionLimit) break;

                if (!electrons[i].connected)
                    electrons[i].gameObject.SetActive(true);
            }
        }
        
    }
    public void LetGo()
    {
        isGrabed = false;
        if(!minimized){
            if (isConnecting)
            {
                pendingConnection.CompleteConnection(this, connectingAtom);
                /**/
                if (molecule != null)
                {
                    if (connectingAtom.molecule != null)
                    {
                        MoleculeV2 oldMolecule = molecule;
                        connectingAtom.molecule.mergeInto(oldMolecule);
                        Destroy(oldMolecule.gameObject);
                    }
                    else
                    {
                        molecule.addAtom(connectingAtom);
                    }
                }
                else
                {
                    if (connectingAtom.molecule != null)
                    {
                        connectingAtom.molecule.addAtom(this);
                    }
                    else
                    {
                        MoleculeV2 newMolecule = Instantiate(manager.moleculePrefab, gameObject.transform.position, Quaternion.identity).GetComponent<MoleculeV2>();
                        newMolecule.InitMolecule();
                        //newMolecule.transform.position = connectingAtom.gameObject.transform.position;
                        //newMolecule.transform.rotation = connectingAtom.gameObject.transform.rotation;
                        newMolecule.addAtom(connectingAtom, true);
                        newMolecule.addAtom(this);
                        newMolecule.interactable.attachTransform = connectingAtom.gameObject.transform;


                    }
                }
                XRInteractionManager int_manager = manager.interactionManager;
                XRGrabInteractable interactable = connectingAtom.gameObject.GetComponent<XRGrabInteractable>();
                XRBaseInteractor interactor = interactable.selectingInteractor;

                interactable.interactionLayerMask = LayerMask.GetMask("Nothing");
                gameObject.GetComponent<XRGrabInteractable>().interactionLayerMask = LayerMask.GetMask("Nothing");
                /**/
                molecule.addConnection(pendingConnection);
                molecule.needsUpdate = true;



                XRGrabInteractable mol_interactable = molecule.interactable;
                mol_interactable.interactionManager.UnregisterInteractable(mol_interactable);
                mol_interactable.interactionManager.RegisterInteractable(mol_interactable);
                manager.OnCreateConnection.Invoke(pendingConnection);

                connectingAtom.removePendingConnection();
                removePendingConnection();
                try
                {
                    int_manager.SelectCancel(interactor, interactable);
                }
                catch
                {
                    //Debug.Log("woops1");
                }

                //Debug.Log("here after select cancel");
                try
                {
                    int_manager.SelectEnter(interactor, mol_interactable);
                }
                catch
                {
                    //Debug.Log("woops2");
                }
                //Debug.Log("here after select enter");
                /**/

            }
            foreach (ElectronV2 electron in electrons)
            {
                //if (electron.connected)
                electron.gameObject.SetActive(false);
            }
            needTransfromUpdate = true;
        }       
        
    }
    public void removePendingConnection()
    {
        pendingConnection = null;
        isConnecting = false;
        if (molecule != null)
        {
            molecule.isConnecting = false;
        }
        isMainConector = false;
        connectingAtom = null;
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
                    Interactable.interactionLayerMask = LayerMask.GetMask("Nothing");
                    gameObject.transform.SetParent(cont.transform);

                }
            }
        }
    }

    public void Release()
    {
        IsContained = false;
        Interactable.interactionLayerMask = ~0;
        gameObject.transform.SetParent(manager.ObjParent);
    }
}

