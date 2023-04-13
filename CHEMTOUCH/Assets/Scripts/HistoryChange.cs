using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoryChange : MonoBehaviour
{

    private Stack<IAction> historyStack;
    private Stack<IAction> redoStack;
    private GameObject worldObjects;
    // Start is called before the first frame update
    void Start()
    {
        historyStack = new Stack<IAction>();
        worldObjects = GameObject.Find("WorldObjects");
    }

    // Update is called once per frame
    public void ExecuteCommand(IAction action)
    {
        action.ExecuteCommand();
        historyStack.Push(action);
    }

    public void UndoCommand()
    {
        if (historyStack.Count > 0)
        {
            redoStack.Push(historyStack.Peek());
            historyStack.Pop().UndoCommand();
        }
        
    }

    public void RedoCommand()
    {
        if (redoStack.Count > 0)
        {
            historyStack.Push(redoStack.Peek());
            redoStack.Pop().UndoCommand();
        }
    }
}
