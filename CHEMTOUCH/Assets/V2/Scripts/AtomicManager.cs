using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

[System.Serializable]
public class ConnectionEvent : UnityEvent<ConnectionV2>
{
}
[System.Serializable]
public class AtomEvent : UnityEvent<AtomV2>
{
}
[System.Serializable]
public class MoleculeEvent : UnityEvent<MoleculeV2>
{
}

[System.Serializable]
public class MoleculeToggleEvent : UnityEvent<MoleculeV2, bool>
{

}

[System.Serializable]
public class AtomToggleEvent : UnityEvent<AtomV2, bool>
{

}

[System.Serializable]
public class KeyInsertEvent : UnityEvent<AtomType, AtomType, MoleculeV2>
{

}

public class AtomicManager : MonoBehaviour
{
    [Serializable]
    public struct AtomProprieties
    {
        public AtomType type;
        public int connectionLimit;
        public Material material;
    }

    public Vector3 minimizeSize = new Vector3(0.5f, 0.5f, 0.5f);
    public Vector3 maximizeSize = new Vector3(1, 1, 1);
    public List<AtomProprieties> atomProprieties;
    public XRInteractionManager interactionManager;
    public GameObject moleculePrefab;
    public GameObject singleConnectionPrefab;
    public GameObject dupleConnectionPrefab;
    public GameObject triplrConnectionPrefab;
    public GameObject connectionPrefab;

    public Transform ObjParent;


    public bool isTutorial = false;
    public bool isComplete = false;
    public bool hasNarrative = false;
    public float firstMessageDelay = 5f;
    public int numberOfSteps = 7;
    public int tutorialStep = 0;
    public float tutorialStepDelay = 2f;
    public GameObject tutorialUI;
    public GameObject tutorialUIWarnings;
    public GameObject BlindFold;

    public float tutorialUIDelay = 0.1f;
    public float tutorialUINewLineDelay = 0.5f;
    public float tutorialWarningDuration = 5f;
    public string CurrentTutorialTask = null;
    public bool hasAdvanced = false;
    [TextArea(5, 10)]
    public List<string> tasks = new List<string>();
    public string[] warnings = new string[10];

    public ConnectionEvent OnCreateConnection = null;
    public ConnectionEvent OnDeleteConnection = null;
    public ConnectionEvent OnCancelConnection = null;

    public MoleculeToggleEvent OnMoleculeToggle = null;
    public AtomToggleEvent OnAtomToggle = null;

    public MoleculeEvent OnUnlockSucess = null;
    public MoleculeEvent OnUnlockFaill = null;
    public KeyInsertEvent OnKeyInsert = null;
    public UnityEvent[] TutorialStepEvents = new UnityEvent[7];

    public static readonly Dictionary<AtomType,int> connectionlimits = new Dictionary<AtomType, int>()
    {
        {AtomType.Hydrogen,1},
        {AtomType.Oxygen,2},
        {AtomType.Bromine,1},
        {AtomType.Carbon,4},
        {AtomType.Iodine,1},
        {AtomType.Empty,1},
        {AtomType.Chlorine,1},
        {AtomType.Nitrogen,3},
        {AtomType.Sulfur,1}
    };

    private Coroutine currentUItext = null;
    private Coroutine currentUIWarning = null;

    public void restart()
    {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("intro");
    }

    public void NormalMode()
    {
        SceneManager.LoadScene("V2");
    }

    public void HardMode()
    {
        SceneManager.LoadScene("V3");
    }

    public void SceneSand()
    {
        SceneManager.LoadScene("Sandbox");
    }

    IEnumerator FirstMessageDelay()
    {
        yield return new WaitForSeconds(firstMessageDelay);

        currentUItext = StartCoroutine(ShowTutorialText(tasks[tutorialStep], tutorialUI));
        currentUIWarning = StartCoroutine(ShowTutorialText(" ", tutorialUIWarnings));
        TutorialStepEvents[0].Invoke();
    }
    IEnumerator FirstMessageDelayAndAdvance()
    {
        yield return new WaitForSeconds(firstMessageDelay);

        currentUItext = StartCoroutine(ShowTutorialText(tasks[tutorialStep], tutorialUI,advance: true));
        currentUIWarning = StartCoroutine(ShowTutorialText(" ", tutorialUIWarnings));
        TutorialStepEvents[0].Invoke();
    }
    private void Start()
    {
        if (isTutorial)
        {
            StartCoroutine(FirstMessageDelay());
        }
        if (hasNarrative)
        {
            BlindFold.SetActive(true);
            StartCoroutine(FirstMessageDelayAndAdvance());
        }
    }
    public void IncreaseStep()
    {
        if (isTutorial)
        {
            if (isComplete)
            {
                SceneManager.LoadScene("V2");
            }
            else
            {
                tutorialStep += 1;
                hasAdvanced = false;
                StopCoroutine(currentUItext);
                currentUItext = StartCoroutine(ShowTutorialText(tasks[tutorialStep], tutorialUI));
                TutorialStepEvents[tutorialStep].Invoke();
            }
        }
        else
        {
            if (isComplete)
            {
                tutorialUI.SetActive(false);
                BlindFold.SetActive(false);
            }
            else
            {
                tutorialStep += 1;
                hasAdvanced = false;
                StopCoroutine(currentUItext);
                currentUItext = StartCoroutine(ShowTutorialText(tasks[tutorialStep], tutorialUI, advance: true));
                TutorialStepEvents[tutorialStep].Invoke();
            }
        }
        
    }
    public void CompleteNarative()
    {
        isComplete = true;
    }
    IEnumerator ShowTutorialText(string task, GameObject TargetUI, bool isWarning = false, bool advance = false)
    {
        string temp = task.Replace("<br>", "\n");
        string[] tempLists = temp.Split('\n');
        foreach(string taskLine in tempLists)
        {
            for (int i = 1; i <= taskLine.Length; i++)
            {
                CurrentTutorialTask = taskLine.Substring(0, i);

                TargetUI.GetComponent<TMPro.TextMeshPro>().text = CurrentTutorialTask;                              
                yield return new WaitForSeconds(tutorialUIDelay);             
            }
            yield return new WaitForSeconds(tutorialUINewLineDelay);

        }


        if (isWarning)
        {
            yield return new WaitForSeconds(tutorialWarningDuration);

            TargetUI.GetComponent<TMPro.TextMeshPro>().text = "";
        }

        if (advance)
        {
            yield return new WaitForSeconds(tutorialStepDelay);
            IncreaseStep();
        }
    }

   

    public void CheckConnectionCreation(ConnectionV2 connection)
    {
        if (isTutorial && !hasAdvanced)
        {
            string warningString = null;
            bool advance = false;
            bool hasWarning = true;
            switch (tutorialStep)
            {
                case 0: 
                    warningString = "That's it!";
                    advance = true;
                    break;

                case 1:
                    switch (connection.size)
                    {
                        case 1:
                            warningString = "Try touching two pairs of electrons before letting go.";
                            connection.deleteFalg = true;
                            advance = false;
                            break;
                        case 2:
                            warningString = "Great!";
                            advance = true;
                            break;
                        case 3:
                            warningString = "That’s a triple connection, give it another shot.";
                            connection.deleteFalg = true;

                            advance = false;
                            break;
                        default:
                            warningString = "That's Not It, Try again!";
                            connection.deleteFalg = true;
                            advance = false;
                            break;
                    }
                    break;

                case 3:

                    if(connection.A1.type == connection.A2.type)
                    {
                        if(connection.A1.type == AtomType.Carbon)
                        {
                            warningString = "Only one carbon is needed";

                        }
                        else if (connection.A1.type == AtomType.Hydrogen)
                        {
                            warningString = "Not quite!";
                        }
                        advance = false;
                    }
                    else
                    {
                        if(connection.molecule.atoms.Count == 5)
                        {
                            int carbonCount = 0;
                            int hydrogenCount = 0;
                            foreach (AtomV2 atom in connection.molecule.atoms)
                            {
                                if (atom.type == AtomType.Carbon) carbonCount += 1;
                                else if (atom.type == AtomType.Hydrogen) hydrogenCount += 1;
                            }
                            if(carbonCount == 1 && hydrogenCount == 4)
                            {
                                warningString = "Great that is a CH4 molecule!";
                                advance = true;
                            }
                            else
                            {
                                warningString = "Not Quite!";
                                advance = false;
                            }

                        }
                        else
                        {
                            hasWarning = false;
                        }
                    }
                    break;

                default:
                    hasWarning = false;
                    advance = false;
                    break;
            }
            if (advance)
            {
                hasAdvanced = true;
            }
            if (hasWarning)
            {
                StopCoroutine(currentUIWarning);
                currentUIWarning = StartCoroutine(ShowTutorialText(warningString, tutorialUIWarnings, true, advance));

            }
            

        }
    }


    public void CheckConnectionDestruction(ConnectionV2 connection)
    {
        if (isTutorial && !hasAdvanced)
        {
            string warningString = null;
            bool advance = false;
            bool hasWarning = true;
            switch (tutorialStep)
            {
                case 2:
                    warningString = "That’s it";
                    advance = true;
                    break;

                default:
                    hasWarning = false;
                    advance = false;
                    break;
            }
            if (advance)
            {
                hasAdvanced = true;
            }
            if (hasWarning)
            {
                StopCoroutine(currentUIWarning);
                currentUIWarning = StartCoroutine(ShowTutorialText(warningString, tutorialUIWarnings, true, advance));

            }
        }
        
    }
    public void CheckConnectionCancel(ConnectionV2 connection)
    {

    }

    public void CheckMoleculeToggle(MoleculeV2 molecule, bool minimized)
    {
        if (isTutorial && !hasAdvanced)
        {
            string warningString = null;
            bool advance = false;
            bool hasWarning = true;
            switch (tutorialStep)
            {
                case 4:
                    if (minimized)
                    {
                        if (molecule.atoms.Count >= 5)
                        {
                            int carbonCount = 0;
                            int hydrogenCount = 0;
                            foreach (AtomV2 atom in molecule.atoms)
                            {
                                if (atom.type == AtomType.Carbon) carbonCount += 1;
                                else if (atom.type == AtomType.Hydrogen) hydrogenCount += 1;
                            }
                            if (carbonCount == 1 && hydrogenCount == 4)
                            {
                                warningString = "Great that is a stabilized CH4 molecule!";
                                advance = true;
                            }
                            else
                            {
                                warningString = "Seems like you stabilized the wrong molecule";
                                advance = false;
                            }
                        }
                    }
                    else
                    {

                    }
                    break;
                default:
                    hasWarning = false;
                    advance = false;
                    break;
            }
            if (advance)
            {
                hasAdvanced = true;
            }
            if (hasWarning)
            {
                StopCoroutine(currentUIWarning);
                currentUIWarning = StartCoroutine(ShowTutorialText(warningString, tutorialUIWarnings, true, advance));

            }

        }
    }

    public void CheckAttomToggle(AtomV2 atom, bool minimized)
    {
        if (isTutorial)
        {

        }
    }

    public void CheckUnlockSucess(MoleculeV2 molecule)
    {
        if (isTutorial && !hasAdvanced)
        {
            string warningString = null;
            bool advance = false;
            bool hasWarning = true;
            switch (tutorialStep)
            {
                case 5:
                    warningString = "That's it!\nYou unlokcked the box, and can now use the items it contained!";
                    advance = true;
                    break;

                case 6:
                    int carbonCount = 0;
                    int hydrogenCount = 0;
                    int oxygenCount = 0;
                    int chlorineCount = 0;
                    foreach (AtomV2 atom in molecule.atoms)
                    {
                        if (atom.type == AtomType.Carbon) carbonCount += 1;
                        else if (atom.type == AtomType.Hydrogen) hydrogenCount += 1;
                        else if (atom.type == AtomType.Chlorine) chlorineCount += 1;
                        else if (atom.type == AtomType.Oxygen) oxygenCount += 1;

                    }
                    if (carbonCount == 2 && hydrogenCount >= 1 && chlorineCount == 1 && oxygenCount == 1)
                    {
                        warningString = "Great job! \n This conludes the intruduction to our facilities. \n Good luck on your reshearch.";
                        isComplete = true;
                        advance = true;
                    }
                    break;
                    
                default:
                    advance = false;
                    hasWarning = false;
                    break;
            }
            if (advance)
            {
                hasAdvanced = true;
            }
            if (hasWarning)
            {
                StopCoroutine(currentUIWarning);
                currentUIWarning = StartCoroutine(ShowTutorialText(warningString, tutorialUIWarnings, true, advance));

            }
        }
    }
    public void CheckUnlockFail(MoleculeV2 molecule)
    {
        if (isTutorial && !hasAdvanced)
        {
            string warningString = null;
            bool advance = false;
            bool hasWarning = true;
            switch (tutorialStep)
            {
                case 5:
                    if (molecule.atoms.Count >= 5)
                    {
                        int carbonCount = 0;
                        int hydrogenCount = 0;
                        foreach (AtomV2 atom in molecule.atoms)
                        {
                            if (atom.type == AtomType.Carbon) carbonCount += 1;
                            else if (atom.type == AtomType.Hydrogen) hydrogenCount += 1;
                        }
                        if (carbonCount == 1 && hydrogenCount == 4)
                        {                           
                            warningString = "Try giving the key molecule a turn.";
                        }
                        else
                        {
                            warningString = "It looks like that's not the right key.";                          
                        }
                        advance = false;

                    }
                    break;
                default:
                    advance = false;
                    hasWarning = false;
                    break;
            }
            if (advance)
            {
                hasAdvanced = true;
            }
            if (hasWarning)
            {
                StopCoroutine(currentUIWarning);
                currentUIWarning = StartCoroutine(ShowTutorialText(warningString, tutorialUIWarnings, true, advance));

            }
        }
    }

    public void CheckKeyInsert(AtomType t1, AtomType t2, MoleculeV2 key)
    {
        if (isTutorial)
        {

        }
    }


}
