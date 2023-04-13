using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanciateCommand : IAction
{
    private GameObject SpawnGameObject;
    private Vector3 SpawnPoint;
    private Quaternion SpawnRotation;
    private GameObject SpawnParent;
    public InstanciateCommand(GameObject spawnObject, Vector3 position, Quaternion rotation, GameObject parent)
    {
        SpawnGameObject = spawnObject;
        SpawnPoint = position;
        SpawnRotation = rotation;
        SpawnParent = parent;
    }
    public void ExecuteCommand()
    {
       GameObject.Instantiate(SpawnGameObject, SpawnPoint,SpawnRotation,SpawnParent.transform);
    }

    public void UndoCommand()
    {
        throw new System.NotImplementedException();
    }

}
