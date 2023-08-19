using System.Collections.Generic;
using UnityEngine;

public class MainThreadDispatcher : MonoBehaviour
{
    private static readonly Queue<System.Action> ExecuteOnMainThread = new Queue<System.Action>();

    public void Update()
    {
        while (ExecuteOnMainThread.Count > 0)
        {
            ExecuteOnMainThread.Dequeue().Invoke();
        }
    }

    public static void RunOnMainThread(System.Action action)
    {
        lock (ExecuteOnMainThread)
        {
            ExecuteOnMainThread.Enqueue(action);
        }
    }
}
