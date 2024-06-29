using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    //All of these lists are populated in the inspector
    public List<SpawnableObjectMenuItem> socketsObjects = new List<SpawnableObjectMenuItem>();
    public List<SpawnableObjectMenuItem> switchesObjects = new List<SpawnableObjectMenuItem>();
    public List<SpawnableObjectMenuItem> lightsObjects = new List<SpawnableObjectMenuItem>();
    public List<SpawnableObjectMenuItem> powerGridObjects = new List<SpawnableObjectMenuItem>();

    public List<GameObject> inventoryFieldParents = new List<GameObject>();
    

    public void SwapToSockets(){
        //populate the inventory with the sockets objectss
        
    }


}
