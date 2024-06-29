using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

   private void Update() {
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch))
        {
            PopulateInventory(InventoryType.Sockets);
        }
        else if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
        {
            PopulateInventory(InventoryType.Switches);
        }

        //if pretty much upside down, print a debug statement
        //use diretion angle to get rough area of direction
        if(transform.rotation.eulerAngles.x > 90 && transform.rotation.eulerAngles.x < 270)
            Debug.Log("Upside down");
   }

    private void OnEnable() {
        PopulateInventory(InventoryType.Sockets);
    }
    //All of these lists are populated in the inspector
    public List<SpawnableObjectMenuItem> socketsObjects = new List<SpawnableObjectMenuItem>();
    public List<SpawnableObjectMenuItem> switchesObjects = new List<SpawnableObjectMenuItem>();
    public List<SpawnableObjectMenuItem> lightsObjects = new List<SpawnableObjectMenuItem>();
    public List<SpawnableObjectMenuItem> powerGridObjects = new List<SpawnableObjectMenuItem>();

    public List<GameObject> inventoryFieldParents = new List<GameObject>();
    

    public void PopulateInventory(InventoryType inventoryType)
    {
        List<SpawnableObjectMenuItem> objects = new List<SpawnableObjectMenuItem>();
        switch (inventoryType)
        {
            case InventoryType.Sockets:
                objects = socketsObjects;
                break;
            case InventoryType.Switches:
                objects = switchesObjects;
                break;
            case InventoryType.Lights:
                objects = lightsObjects;
                break;
            case InventoryType.PowerGrid:
                objects = powerGridObjects;
                break;
        }   
        Debug.Log("Populating inventory");
        for (int i = 0; i < inventoryFieldParents.Count; i++)
        {

            var inventoryField = inventoryFieldParents[i];
            if(inventoryField.transform.childCount > 0)
                Destroy(inventoryField.transform.GetChild(0).gameObject);
            if(objects.Count <= i)
            {
                continue;
            }
            Debug.Log("Spawning another object in inventory");

            var newObject = Instantiate(objects[i].prefabDuringMenu, inventoryField.transform);
            newObject.transform.localPosition = new Vector3(0, 0, -6f);
            newObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            Vector3 scale = newObject.transform.localScale * 70;
            newObject.transform.localScale = Vector3.zero;
            newObject.transform.DOScale(scale, 1f).SetEase(Ease.OutBack);
        }
    }


}

public enum InventoryType
{
    Sockets,
    Switches,
    Lights,
    PowerGrid
}
