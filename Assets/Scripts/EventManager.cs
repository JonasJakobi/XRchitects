
// Singleton EventManager class to manage events for object placement and deletion
using System;
using UnityEngine;

public class EventManager
{
    // Singleton instance
    private static EventManager instance;

    // Events
    public event Action<GameObject> OnObjectPlaced;
    public event Action OnObjectDeleted;

    // Private constructor to prevent instantiation
    private EventManager() { }

    // Public property to access the singleton instance
    public static EventManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EventManager();
            }
            return instance;
        }
    }

    // Method to trigger the OnObjectPlaced event
    public void TriggerObjectPlaced(GameObject obj)
    {
        OnObjectPlaced?.Invoke(obj);
    }

    // Method to trigger the OnObjectDeleted event
    public void TriggerObjectDeleted()
    {
        OnObjectDeleted?.Invoke();
    }
}