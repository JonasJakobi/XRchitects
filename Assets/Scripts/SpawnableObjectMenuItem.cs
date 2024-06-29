using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SpawnableObjectMenu", menuName = "spawnables" , order = 1)]
[System.Serializable]
public class SpawnableObjectMenuItem :ScriptableObject
{   
    public string name;
    public GameObject prefabForPlacing;

    public GameObject previewPrefab;
    public GameObject prefabDuringMenu;

    public string SurfaceToSpawnOnString;
}