using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
    // Start is called before the first frame update
    void ExecuteCommand();
    
    void UndoCommand();
}
