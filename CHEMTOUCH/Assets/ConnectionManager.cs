using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    ConnectionV2 pendingConnection;
    private bool isConnectionPending;
    public bool IsConnectionPending { get => isConnectionPending; set => isConnectionPending = value; }

    public ConnectionV2 CreateConnection(AtomV2 a1, AtomV2 a2)
    {
            IsConnectionPending = true;
            pendingConnection = new ConnectionV2();
            return pendingConnection;
    }

    public ConnectionV2 GetPendingConnection()
    {
        return pendingConnection;
    }
}
