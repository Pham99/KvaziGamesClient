using System;
using System.Collections.Concurrent;
using UnityEngine;

public class MainThreadDispatcher : MonoBehaviour
{
    // 1. Replaced Queue with ConcurrentQueue. It is inherently thread-safe and lock-free.
    private static readonly ConcurrentQueue<Action> _executionQueue = new();
    private static MainThreadDispatcher _instance;

    public static MainThreadDispatcher Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("MainThreadDispatcher");
                _instance = obj.AddComponent<MainThreadDispatcher>();
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }

    public void Enqueue(Action action)
    {
        if (action == null) return;
        
        // No lock needed.
        _executionQueue.Enqueue(action);
    }

    void Update()
    {
        // 2. Frame-Drop Safeguard.
        // Limit the number of actions processed per frame to guarantee the game doesn't freeze.
        int actionsProcessed = 0;
        int maxActionsPerFrame = 50; 

        // 3. TryDequeue handles the safety internally without heavy locking overhead.
        while (_executionQueue.TryDequeue(out Action action))
        {
            Debug.Log("Executing action on main thread.");
            action?.Invoke();
            
            actionsProcessed++;
            if (actionsProcessed >= maxActionsPerFrame)
            {
                Debug.LogWarning("Dispatcher hit max actions per frame. Postponing remainder.");
                break; 
            }
        }
    }
}