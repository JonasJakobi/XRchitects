using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using UnityEngine;

public class TestingScript : MonoBehaviour
{
    public GameObject currentObject;  

    
    private GameObject currentPreview;
    void Update(){
            TryPlacePreviewPrefab();
    }
   
    public void PlacePrefabAtControllerPointedSpot(GameObject prefab){
        if (currentObject != null){
            Instantiate(currentObject, getSurfaceNormalFromPointedWall(), Quaternion.identity);
        
        }
    }
    private void TryPlacePreviewPrefab(){
        //if the user is pointing at a wall or floor, we place the preview prefab at the closest point
        if (currentObject != null){
            if(currentPreview == null){
                currentPreview = Instantiate(currentObject);
            }
            //currentPreview.transform.position = GetClosestPointOnWall(currentPreview.transform.position, ());
            currentPreview.transform.rotation = Quaternion.LookRotation(getSurfaceNormalFromPointedWall());
        }
    }


    private Vector3 getSurfaceNormalFromPointedWall(){
        //Where the right touch controller is pointing, we place at closest point after raycasting MRUKRoom
        Vector3 controllerPosition =  OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        Vector3 controllerForward = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch) * Vector3.forward;
        Ray ray = new Ray(controllerPosition, controllerForward);
        Vector3 surfaceNormal;
        
        LabelFilter filter = new LabelFilter();
        MRUKAnchor anchor;
        
        MRUK.Instance.GetCurrentRoom().GetBestPoseFromRaycast(ray, 2000f, filter,out anchor, out surfaceNormal);
        return surfaceNormal;
    }

     

    
}
