using System;
using System.Collections.Generic;
using UnityEngine;

public class MainThreadDispatcher : MonoBehaviour
{
    private static readonly Queue<Action> _executionQueue = new Queue<Action>();
    private static MainThreadDispatcher _instance;

    public static MainThreadDispatcher Instance
    {
        get
        {
            if (_instance == null)
            {
                // Create a new GameObject if one doesn't exist.
                GameObject obj = new GameObject("MainThreadDispatcher");
                _instance = obj.AddComponent<MainThreadDispatcher>();
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }

    /// <summary>
    /// Enqueue an action to be executed on the main thread.
    /// </summary>
    public void Enqueue(Action action)
    {
        if (action == null)
            throw new ArgumentNullException(nameof(action));

        lock (_executionQueue)
        {
            _executionQueue.Enqueue(action);
        }
    }

    void Update()
    {
        lock (_executionQueue)
        {
            if (_executionQueue.Count > 0)
                Debug.Log("Dispatcher executing " + _executionQueue.Count + " action(s).");
        }

        while (_executionQueue.Count > 0)
        {
            Action action;
            lock (_executionQueue)
            {
                action = _executionQueue.Dequeue();
            }
            action?.Invoke();
        }
    }
}