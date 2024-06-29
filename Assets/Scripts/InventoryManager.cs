using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

   private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PopulateInventory(InventoryType.Sockets);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PopulateInventory(InventoryType.Switches);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PopulateInventory(InventoryType.Lights);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PopulateInventory(InventoryType.PowerGrid);
        }
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

        for (int i = 0; i < inventoryFieldParents.Count; i++)
        {
            var inventoryField = inventoryFieldParents[i];
            if(inventoryField.transform.childCount > 0)
                Destroy(inventoryField.transform.GetChild(0).gameObject);
            if(objects.Count <= i)
            {
                break;
            }
            Debug.Log("Spawning another object in inventory");
            var newObject = Instantiate(objects[i].prefabDuringMenu, inventoryField.transform);
            newObject.transform.localPosition = new Vector3(0, 0, -0.05f);
            Vector3 scale = newObject.transform.localScale;
            newObject.transform.localScale = Vector3.zero;
            newObject.transform.DOScale(scale, 1.5f).SetEase(Ease.OutBack);
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
