using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public Color available;
    public Color unavailable;
    InventoryType currentInventoryType = InventoryType.Sockets;
 
    private void OnEnable() {
        PopulatePowerGridInventory();
    }
    //All of these lists are populated in the inspector
    public List<SpawnableObjectMenuItem> socketsObjects = new List<SpawnableObjectMenuItem>();
    public List<SpawnableObjectMenuItem> switchesObjects = new List<SpawnableObjectMenuItem>();
    public List<SpawnableObjectMenuItem> lightsObjects = new List<SpawnableObjectMenuItem>();
    public List<SpawnableObjectMenuItem> powerGridObjects = new List<SpawnableObjectMenuItem>();

    public List<GameObject> inventoryFieldParents = new List<GameObject>();
    

    public void PopulateInventory(InventoryType inventoryType)
    {
        AudioManager.Instance.PlayButton();
        currentInventoryType = inventoryType;
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
            //change color of image component
            inventoryField.GetComponent<Button>().gameObject.SetActive(true);
            inventoryField.GetComponent<UnityEngine.UI.Image>().color = available;
            if(inventoryField.transform.childCount > 0){
                Debug.Log("Destroying object in inventory" + i);
                Destroy(inventoryField.transform.GetChild(0).gameObject);
                

            }
            if(objects.Count <= i)
            {
                inventoryField.GetComponent<UnityEngine.UI.Image>().color = unavailable;
                inventoryField.GetComponent<Button>().gameObject.SetActive(false);
                continue;
            }
            Debug.Log("Spawning another object in inventory" + i);

            var newObject = Instantiate(objects[i].prefabDuringMenu, inventoryField.transform);
            newObject.transform.localPosition = new Vector3(0, 0, -6f);
            newObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            Vector3 scale = newObject.transform.localScale * 70;
            newObject.transform.localScale = Vector3.zero;
            newObject.transform.DOScale(scale, 1f).SetEase(Ease.OutBack);
        }
    }
    //4 mehtods for populating the inventory for each type 
    public void PopulateSocketsInventory()
    {
        PopulateInventory(InventoryType.Sockets);
    }
    public void PopulateSwitchesInventory()
    {
        PopulateInventory(InventoryType.Switches);
    }
    public void PopulateLightsInventory()
    {
        PopulateInventory(InventoryType.Lights);
    }
    public void PopulatePowerGridInventory()
    {
        PopulateInventory(InventoryType.PowerGrid);
    }
    private void UpdateSpawnable(int index){
        AudioManager.Instance.PlayButton();
        SpawnableObjectMenuItem spawnable = null;
        switch (currentInventoryType)
        {
            case InventoryType.Sockets:
                spawnable = socketsObjects[index];
                break;
            case InventoryType.Switches:
                spawnable = switchesObjects[index];
                break;
            case InventoryType.Lights:
                spawnable = lightsObjects[index];
                break;
            case InventoryType.PowerGrid:
                spawnable = powerGridObjects[index];
                break;
            default:
                break;
        }
        if(spawnable != null){
            TestingScript.Instance.currentObject = spawnable;
            Destroy(TestingScript.Instance.currentPreview);
        }

    }
    public void UpdateSpawnableOne(){
        UpdateSpawnable(0);
    }
    public void UpdateSpawnableTwo(){
        UpdateSpawnable(1);
    }
    public void UpdateSpawnableThree(){
        UpdateSpawnable(2);
    }
    public void UpdateSpawnableFour(){
        UpdateSpawnable(3);
    }
    public void UpdateSpawnableFive(){
        UpdateSpawnable(4);
    }
    public void UpdateSpawnableSix(){
        UpdateSpawnable(5);
    }
    public void UpdateSpawnableSeven(){
        UpdateSpawnable(6);
    }
    public void UpdateSpawnableEight(){
        UpdateSpawnable(7);
    }

}

[System.Serializable]
public enum InventoryType
{
    Sockets,
    Switches,
    Lights,
    PowerGrid
}
