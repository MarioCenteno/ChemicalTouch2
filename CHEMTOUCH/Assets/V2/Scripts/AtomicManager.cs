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
    //public Vector3 maximizeSize = new Vector3(1.3f, 1.3f, 1.3f);
    public List<AtomProprieties> atomProprieties;
    public XRInteractionManager interactionManager;
    public GameObject moleculePrefab;
    public GameObject singleConnectionPrefab;
    public GameObject dupleConnectionPrefab;
    public GameObject triplrConnectionPrefab;
    public GameObject connectionPrefab;

    public Transform ObjParent;

    public bool hasTimer = false;
    public bool hasEnding = false;

    public bool gameEnded = false;

    public bool PT = false;


    public bool isTutorial = false;
    public bool isComplete = false;
    public bool hasNarrative = false;
    public float firstMessageDelay = 5f;
    public int numberOfSteps = 7;
    public int tutorialStep = 0;
    public float tutorialStepDelay = 2f;
    public GameObject tutorialUI;
    public GameObject tutorialUIWarnings;
    public GameObject timerUI;
    public GameObject timerUICountdown;
    public GameObject BlindFold;

    public float tutorialUIDelay = 0.1f;
    public float tutorialUINewLineDelay = 0.5f;
    public float tutorialWarningDuration = 5f;
    public string CurrentTutorialTask = null;
    public bool hasAdvanced = false;
    [TextArea(5, 10)]
    public List<string> tasks = new List<string>();
    [TextArea(5, 10)]
    public List<string> tasks2 = new List<string>();
    public string[] warnings = new string[10];

    float currentTime = -7f;
    float countdownTime = 0f;
    //float startingTime = 368f;
    float startingTime = 308f;//218f;

    public List<DoorOpener> doors;

    public ConnectionEvent OnCreateConnection = null;
    public ConnectionEvent OnDeleteConnection = null;
    public ConnectionEvent OnCancelConnection = null;

    public MoleculeToggleEvent OnMoleculeToggle = null;
    public AtomToggleEvent OnAtomToggle = null;

    public MoleculeEvent OnUnlockSucess = null;
    public MoleculeEvent OnUnlockFaill = null;
    public KeyInsertEvent OnKeyInsert = null;
    public UnityEvent[] TutorialStepEvents = new UnityEvent[7];

    public GameObject light1;
    public GameObject light2;
    public GameObject light3;
    public GameObject light4;

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
        {AtomType.Sulfur,2}
    };

    private Coroutine currentUItext = null;
    private Coroutine currentUIWarning = null;
    private Coroutine currentUItimer = null;
    private Coroutine currentUICountdown = null;


    public void restart()
    {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("Menu3");
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("intro");
    }

    public void TutorialPT()
    {
        SceneManager.LoadScene("introPT");
    }

    public void NormalMode()
    {
        SceneManager.LoadScene("V2");
    }

    public void NormalModePT()
    {
        SceneManager.LoadScene("V2PT");
    }

    public void HardMode()
    {
        SceneManager.LoadScene("V3");
    }

    public void HardModePT()
    {
        SceneManager.LoadScene("V3PT");
    }

    public void SceneSand()
    {
        SceneManager.LoadScene("Sandbox");
    }

    public void SceneSandPT()
    {
        SceneManager.LoadScene("SandboxPT");
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

    IEnumerator FinalMessage()
    {
        yield return new WaitForSeconds(firstMessageDelay);

        currentUItext = StartCoroutine(ShowTutorialText(tasks2[tutorialStep], tutorialUI, advance: false));
    }



    private void Start()
    {
        if (isTutorial)
        {
            StartCoroutine(FirstMessageDelay());
        }
        if (hasTimer)
        {
            countdownTime = startingTime;
            timerUICountdown.GetComponent<TMPro.TextMeshPro>().color = Color.yellow;
        }
        if (hasNarrative)
        {
            BlindFold.SetActive(true);
            if (timerUI != null && timerUICountdown != null) {
                timerUI.SetActive(false);
                timerUICountdown.SetActive(false);
            }
            StartCoroutine(FirstMessageDelayAndAdvance());
            currentUIWarning = StartCoroutine(ShowTutorialText(" ", tutorialUIWarnings));
        }
    }
    public void IncreaseStep()
    {
        if (isTutorial)
        {
            if (isComplete)
            {
                /*if (PT)
                    SceneManager.LoadScene("V2PT");
                else
                    SceneManager.LoadScene("V2");*/
                SceneManager.LoadScene("Menu3");
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
                //tutorialUI.SetActive(false);

                BlindFold.SetActive(false);
                tutorialUI.GetComponent<TMPro.TextMeshPro>().text = "";
                if (hasTimer)
                {
                    timerUI.SetActive(true);
                    timerUICountdown.SetActive(true);
                }
            }
            else
            {
                if (!gameEnded)
                {
                    tutorialStep += 1;
                    hasAdvanced = false;
                    StopCoroutine(currentUItext);
                    currentUItext = StartCoroutine(ShowTutorialText(tasks[tutorialStep], tutorialUI, advance: true));
                    TutorialStepEvents[tutorialStep].Invoke();
                }
                else
                {
                    tutorialStep += 1;
                    hasAdvanced = false;
                    StopCoroutine(currentUItext);
                    if (tutorialStep == tasks2.Count)
                    {
                        SceneManager.LoadScene("Menu3");
                    }
                    currentUItext = StartCoroutine(ShowTutorialText(tasks2[tutorialStep], tutorialUI, advance: false));

                }

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
            yield return new WaitForSeconds(tutorialUINewLineDelay*4);

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
        if (gameEnded){
            yield return new WaitForSeconds(tutorialStepDelay);
            IncreaseStep();
        }
    }

    IEnumerator ShowBriefText(GameObject TargetUI)
    {

        if (PT)
            TargetUI.GetComponent<TMPro.TextMeshPro>().text = "Tempo Bonus!\n+30 segundos";
        else
            TargetUI.GetComponent<TMPro.TextMeshPro>().text = "Bonus Time!\n+30 seconds";
        
        
        yield return new WaitForSeconds(tutorialUIDelay);
        yield return new WaitForSeconds(tutorialUINewLineDelay);

        yield return new WaitForSeconds(tutorialWarningDuration);
        TargetUI.GetComponent<TMPro.TextMeshPro>().text = "";
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
                    if(PT)
                        warningString = "É isso mesmo!";
                    else
                        warningString = "That's it!";
                    advance = true;
                    break;

                case 1:
                    switch (connection.size)
                    {
                        case 1:
                            if (PT)
                                warningString = "Tenta tocar 2 pares de eletrões antes de largares os atomos";
                            else
                                warningString = "Try touching two pairs of electrons before letting go.";
                            connection.deleteFalg = true;
                            advance = false;
                            break;
                        case 2:
                            if (PT)
                                warningString = "Boa!";
                            else
                                warningString = "Great!";
                            advance = true;
                            break;
                        case 3:
                            if (PT)
                                warningString = "Isso é uma conexão tripla, tenta outra vez.";
                            else
                                warningString = "That’s a triple connection, give it another shot.";
                            connection.deleteFalg = true;

                            advance = false;
                            break;
                        default:
                            if (PT)
                                warningString = "Não é isso, tenta outra vez!";
                            else
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
                            if (PT)
                                warningString = "Apenas um atomo de carbono é necessário.";
                            else
                                warningString = "Only one carbon is needed";

                        }
                        else if (connection.A1.type == AtomType.Hydrogen)
                        {
                            if (PT)
                                warningString = "Não é isso!";
                            else
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
                                if (PT)
                                    warningString = "Boa! Isso é uma molécula de CH4!";
                                else
                                    warningString = "Great! That is a CH4 molecule!";
                                advance = true;
                            }
                            else
                            {
                                if (PT)
                                    warningString = "Não é isso!";
                                else
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
                    if (PT)
                        warningString = "É isso mesmo!";
                    else
                        warningString = "That’s it!";
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
                                if (PT)
                                    warningString = "Boa! Isso é uma molécula CH4 estabilizada!";
                                else
                                    warningString = "Great! That is a stabilized CH4 molecule!";
                                advance = true;
                            }
                            else
                            {
                                if (PT)
                                    warningString = "Parece que estabilizaste a molécula errada...";
                                else
                                    warningString = "Seems like you stabilized the wrong molecule...";
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
                    if (PT)
                        warningString = "É isso mesmo!\nAbriste a caixa, e agora podes utilizar os objetos que estão dentro dela!";
                    else
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
                        if (PT)
                            warningString = "Bom trabalho!\n Isto conclui a introdução às nossas instalações. \n Bom trabalho na tua pesquisa.";
                        else
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
        if (!isTutorial)
        {
            countdownTime += 30;
            if (countdownTime > 60)
            {
                timerUICountdown.GetComponent<TMPro.TextMeshPro>().color = Color.yellow;
            }
            currentUItext = StartCoroutine(ShowBriefText(tutorialUI));
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
                            if (PT)
                                warningString = "Tenta rodar a molécula.";
                            else
                                warningString = "Try giving the key molecule a turn.";
                        }
                        else
                        {
                            if (PT)
                                warningString = "Parece que isso não é a chave correta.";
                            else
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

    // Update is called once per frame
    void Update()
    {
        if (!gameEnded && countdownTime!=0 &&( (hasNarrative && isComplete) ||!hasNarrative) && hasTimer) {
            float minutes = Mathf.FloorToInt(countdownTime / 60);
            float seconds = Mathf.FloorToInt(countdownTime % 60);
            if (PT)
            {
                if (minutes < 10 && seconds < 10)
                {
                    timerUICountdown.GetComponent<TMPro.TextMeshPro>().text = "Tempo Restante: 0" + minutes.ToString() + ":0" + seconds.ToString();
                }
                else
                {
                    if (minutes < 10)
                    {
                        timerUICountdown.GetComponent<TMPro.TextMeshPro>().text = "Tempo Restante: 0" + minutes.ToString() + ":" + seconds.ToString();
                    }
                    else
                    {
                        if (seconds < 10)
                        {
                            timerUICountdown.GetComponent<TMPro.TextMeshPro>().text = "Tempo Restante: " + minutes.ToString() + ":0" + seconds.ToString();
                        }
                        else
                        {
                            timerUICountdown.GetComponent<TMPro.TextMeshPro>().text = "Tempo Restante: " + minutes.ToString() + ":" + seconds.ToString();
                        }
                    }
                }
            }
            else
            {
                if (minutes < 10 && seconds < 10)
                {
                    timerUICountdown.GetComponent<TMPro.TextMeshPro>().text = "Time Left: 0" + minutes.ToString() + ":0" + seconds.ToString();
                }
                else
                {
                    if (minutes < 10)
                    {
                        timerUICountdown.GetComponent<TMPro.TextMeshPro>().text = "Time Left: 0" + minutes.ToString() + ":" + seconds.ToString();
                    }
                    else
                    {
                        if (seconds < 10)
                        {
                            timerUICountdown.GetComponent<TMPro.TextMeshPro>().text = "Time Left: " + minutes.ToString() + ":0" + seconds.ToString();
                        }
                        else
                        {
                            timerUICountdown.GetComponent<TMPro.TextMeshPro>().text = "Time Left: " + minutes.ToString() + ":" + seconds.ToString();
                        }
                    }

                }
            }
            countdownTime -= 1 * Time.deltaTime;
            if (countdownTime <= 0)
            {
                countdownTime = 0;
                if (PT)
                    SceneManager.LoadScene("GameOverPT");
                else
                    SceneManager.LoadScene("GameOver");
            }

            float minutes2 = Mathf.FloorToInt(currentTime / 60);
            float seconds2 = Mathf.FloorToInt(currentTime % 60);
            if (PT)
            {
                if (minutes2 < 10 && seconds2 < 10)
                {
                    timerUI.GetComponent<TMPro.TextMeshPro>().text = "Tempo: 0" + minutes2.ToString() + ":0" + seconds2.ToString();
                }
                else
                {
                    if (minutes2 < 10)
                    {
                        timerUI.GetComponent<TMPro.TextMeshPro>().text = "Tempo: 0" + minutes2.ToString() + ":" + seconds2.ToString();
                    }
                    else
                    {
                        if (seconds2 < 10)
                        {
                            timerUI.GetComponent<TMPro.TextMeshPro>().text = "Tempo: " + minutes2.ToString() + ":0" + seconds2.ToString();
                        }
                        else
                        {
                            timerUI.GetComponent<TMPro.TextMeshPro>().text = "Tempo: " + minutes2.ToString() + ":" + seconds2.ToString();
                        }
                    }

                }
            }
            else
            {
                if (minutes2 < 10 && seconds2 < 10)
                {
                    timerUI.GetComponent<TMPro.TextMeshPro>().text = "Time: 0" + minutes2.ToString() + ":0" + seconds2.ToString();
                }
                else
                {
                    if (minutes2 < 10)
                    {
                        timerUI.GetComponent<TMPro.TextMeshPro>().text = "Time: 0" + minutes2.ToString() + ":" + seconds2.ToString();
                    }
                    else
                    {
                        if (seconds2 < 10)
                        {
                            timerUI.GetComponent<TMPro.TextMeshPro>().text = "Time: " + minutes2.ToString() + ":0" + seconds2.ToString();
                        }
                        else
                        {
                            timerUI.GetComponent<TMPro.TextMeshPro>().text = "Time: " + minutes2.ToString() + ":" + seconds2.ToString();
                        }
                    }

                }
            }
            currentTime += 1 * Time.deltaTime;
        }
        if (hasTimer)
        {
            if (countdownTime <= 60)
            {
                light1.SetActive(false);
                light2.SetActive(false);
                light3.SetActive(false);
                light4.SetActive(false);
                timerUICountdown.GetComponent<TMPro.TextMeshPro>().color = Color.red;
                if (PT)
                    timerUICountdown.GetComponent<TMPro.TextMeshPro>().text += "\nDepressa!";
                else
                    timerUICountdown.GetComponent<TMPro.TextMeshPro>().text += "\nHurry Up!";
            }
            else
            {
                light1.SetActive(true);
                light2.SetActive(true);
                light3.SetActive(true);
                light4.SetActive(true);
            }
        }
        if (!gameEnded && hasEnding)
        {
                bool hasEnded = true;
                foreach (DoorOpener door in doors)
                {
                    if (!door.openFinished)
                    {
                        hasEnded = false;
                        break;
                    }
                }
                if (hasEnded)
                {
                    gameEnded = true;
                    BlindFold.SetActive(true);
                    //tutorialUI.SetActive(true);
                    if (hasTimer)
                    {
                        timerUI.SetActive(false);
                        timerUICountdown.SetActive(false);

                        //Aplicar sistema de classificação aqui

                        //Ranks:
                        // S - 3:40
                        // A - 4:20
                        // B - 5:00
                        // C - 6:00

                        string s = "";

                        float minutes = Mathf.FloorToInt(currentTime / 60);
                        float seconds = Mathf.FloorToInt(currentTime % 60);


                        if (currentTime < 220)
                        {
                            s = " S";
                            if (PT)
                                tasks2[3] = "Alcançaste a melhor classificação possível! Fantástico! Agora descansa.";
                            else
                                tasks2[3] = "You've achived the best possible rank! Awesome! Now go take a rest.";
                        }
                        if (220 < currentTime && currentTime < 280)
                        {
                            s = "n A";
                        }
                        if (280 < currentTime && currentTime < 320)
                        {
                            s = " B";
                        }
                        if (320 < currentTime)
                        {
                            s = " C";
                        }
                        if (seconds < 10)
                        {
                            if (PT) {
                                //tasks2[2] = "Demoraste <color=yellow>" + minutes + ":0" + seconds + "</color> e como tal serás recompensado com a classificação<color=blue>" + s + "</color>.";
                                tasks2[2] = "Demoraste " + minutes + ":0" + seconds + " e como tal serás recompensado com a classificação" + s + ".";
                            }
                            else{ 
                                //tasks2[2] = "You took <color=yellow>" + minutes + ":0" + seconds + "</color> to finish and as such you'll be awarded with a<color=blue>" + s + "</color> rank.";
                                tasks2[2] = "You took " + minutes + ":0" + seconds + " to finish and as such you'll be awarded with a" + s + " rank.";
                            }
                        }
                        else {
                            if (PT) {
                                //tasks2[2] = "Demoraste <color=yellow>" + minutes + ":" + seconds + "</color> e como tal serás recompensado com a classificação<color=blue>" + s + "</color>.";
                                tasks2[2] = "Demoraste " + minutes + ":" + seconds + " e como tal serás recompensado com a classificação" + s + ".";
                            }
                            else{
                                //tasks2[2] = "You took <color=yellow>" + minutes + ":" + seconds + "</color> to finish and as such you'll be awarded with a<color=blue>" + s + "</color> rank.";
                                tasks2[2] = "You took " + minutes + ":" + seconds + " to finish and as such you'll be awarded with a" + s + " rank.";
                            }
                        }
                    }
                isComplete = false;
                tutorialStep = 0;
                StartCoroutine(FinalMessage());
            }
        }
    }

}
