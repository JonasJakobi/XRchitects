using System.Collections;
using System.Collections.Generic;
using Meta.XR.BuildingBlocks;
using Meta.XR.MRUtilityKit;
using Unity.VisualScripting;
using UnityEngine;

public class SavingManager : MonoBehaviour{

     private IEnumerator WaitAndSaveAnchor(OVRSpatialAnchor anchor){
        yield return new WaitUntil (() => anchor.Created);
        SaveAnchor(anchor);
        SaveUUIDToPlayerPrefs(anchor);
    }
    private async void SaveAnchor(OVRSpatialAnchor anchor){
         var result = await OVRSpatialAnchor.SaveAnchorsAsync(new OVRSpatialAnchor[]{anchor});

    }
    private void SaveUUIDToPlayerPrefs(OVRSpatialAnchor anchor){
        //Create a list of UUIDs, and add it if it isnt there yet
        List<string> uuids = new List<string>();
        if(PlayerPrefs.HasKey("UUIDs")){
            string[] uuidsArray = PlayerPrefs.GetString("UUIDs").Split(' ');
            foreach (string uuid in uuidsArray){
                uuids.Add(uuid);
            }
            if (!uuids.Contains(anchor.Uuid.ToString())){
                uuids.Add(anchor.Uuid.ToString());
            PlayerPrefs.SetString("UUIDs", string.Join(" ", uuids.ToArray()));    
            }
        }
        else{
            PlayerPrefs.SetString("UUIDs", anchor.Uuid.ToString());

        }
    }

    async void EraseOVRAnchor(OVRSpatialAnchor _spatialAnchor)
    {
        var result = await _spatialAnchor.EraseAnchorAsync();
        if (result.Success)
        {
            Debug.Log($"Successfully erased anchor.");
        }
        else
        {
            Debug.LogError($"Failed to erase anchor {_spatialAnchor.Uuid} with result {result.Status}");
        }
    }
}