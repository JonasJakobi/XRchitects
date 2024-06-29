using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectMeshManager : MonoBehaviour
{
    public string nameOfGameObject = "Room - ";
    private GameObject foundGameObject;


    void Start()
    {
        DisableEffectMesh();
    }


    public void FindGameObjectByPartOfName(string partOfName)
    { 
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains(partOfName))
            {
                foundGameObject = obj;
                Debug.LogError("Found object: " + obj.name);
            }
        }
    }



    public void DisableEffectMesh()
    {
        FindGameObjectByPartOfName(nameOfGameObject);
        foundGameObject.SetActive(false);
    }


    public void EnableEffectMesh()
    {
        FindGameObjectByPartOfName(nameOfGameObject);
        foundGameObject.SetActive(true);
    }
}
