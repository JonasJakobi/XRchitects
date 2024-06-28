using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using UnityEngine;

public class TestingScript : MonoBehaviour
{
    public SpawnableObjectMenuItem currentObject;  

    
    private GameObject currentPreview;
    void Update(){
        //Try Place
        if(OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch) || OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch)){
            if(!IsPositionOccupied(getPositionFromRaycast())){
                PlacePrefabAtControllerPointedSpot();
            }
        }
        //try delete
        else if(OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch)){
            DeletePrefabAtControllerPointedSpot();
        }
        else{
            TryPlacePreviewPrefab();
        }
            
    }
   
    public void PlacePrefabAtControllerPointedSpot(){
        if (currentObject != null){
            Quaternion rot = Quaternion.LookRotation(getSurfaceNormalFromPointedWall());
            Vector3 pos = getPositionFromRaycast();
            Instantiate(currentObject.prefabForPlacing, pos, rot);
            EventManager.Instance.TriggerObjectPlaced();
        
        }
    }
    private void TryPlacePreviewPrefab(){
        //if the user is pointing at a wall or floor, we place the preview prefab at the closest point
        if (currentObject != null){
            if(currentPreview == null){
                currentPreview = Instantiate(currentObject.previewPrefab);
            }
            Vector3 position = getPositionFromRaycast();
            if (position == Vector3.zero || IsPositionOccupied(position)){
                Destroy(currentPreview);
            }
            else{
                currentPreview.transform.position = getPositionFromRaycast();
                currentPreview.transform.rotation = Quaternion.LookRotation(getSurfaceNormalFromPointedWall());
                
            }
            
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

 private Vector3 getPositionFromRaycast(){
        //Where the right touch controller is pointing, we place at closest point after raycasting MRUKRoom
        Vector3 controllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        Quaternion controllerRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
        Vector3 controllerForward = controllerRotation * Vector3.forward;
        Ray ray = new Ray(controllerPosition, controllerForward);

        
        LabelFilter filter = new LabelFilter();
        RaycastHit hit;
        MRUKAnchor anchor;
        
        MRUK.Instance.GetCurrentRoom().Raycast(ray, 2000f, filter,out hit, out anchor);
        if (ReferenceEquals(hit, null)){
            return Vector3.zero;
        }
        return hit.point;
    }

    private bool IsPositionOccupied(Vector3 position){
        float checkRadius = 0.1f; // Adjust based on the size of the objects being placed
        Collider[] hitColliders = Physics.OverlapSphere(position, checkRadius);
        foreach (var hitCollider in hitColliders){
            if (hitCollider.gameObject.tag == "PlacedObject"){ // Ensure the current preview object is not considered as an obstacle
                return true; // Position is occupied
            }
        }
        return false; // Position is not occupied
    }
    

    private void DeletePrefabAtControllerPointedSpot(){
        Vector3 position = getPositionFromRaycast();
        if (position != Vector3.zero){
            Collider[] hitColliders = Physics.OverlapSphere(position, 0.1f);
            foreach (var hitCollider in hitColliders){
                if (hitCollider.gameObject.tag == "PlacedObject"){
                    Destroy(hitCollider.gameObject);
                    EventManager.Instance.TriggerObjectDeleted();
                }
            }
        }
    }

    
}
