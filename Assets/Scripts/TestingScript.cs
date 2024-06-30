using System.Collections;
using System.Collections.Generic;
using Meta.XR.BuildingBlocks;
using Meta.XR.MRUtilityKit;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection.Emit;

public class TestingScript : MonoBehaviour
{

    public Image trashButton;
    public Sprite trashOn;
    public Sprite trashOff;
    public static TestingScript Instance;
    public SpawnableObjectMenuItem currentObject;  

    public OurInputMode inputMode;
    public GameObject currentPreview;
    [SerializeField]
    private GameObject previewPrefabForDeleting;

    public GameObject rayPrefab;
    public GameObject currentRay;
    private void Start() {
        Instance = this;
        currentRay = Instantiate(rayPrefab);
        ChangeInputModeToPlace();
        
    }
    void Update(){
        //Check with a regular raycast if the UI collider is in the way, tag is UI
        Vector3 controllerPosition =  OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        Vector3 controllerForward = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch) * Vector3.forward;
        //Place the ray int he direction of the controller
        currentRay.transform.position = controllerPosition;
        currentRay.transform.rotation = Quaternion.LookRotation(controllerForward);
        Ray ray = new Ray(controllerPosition, controllerForward);
        RaycastHit[] hits = Physics.RaycastAll(ray, 1000f);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.tag == "UI")
            {
            Destroy(currentPreview);
            return;
            }
        }

        if(OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch)){
            PowerSourcePlacer.Instance.SetShowingCables(!PowerSourcePlacer.Instance.GetShowingCables());
        }

        
        switch(inputMode){
            case OurInputMode.PlacingObject:
                HandlePlacingObjectInputs();
                break;
            case OurInputMode.DeletingObject:
                HandleDeletingObjectInputs();
                break;
            case OurInputMode.ConnectingLightWithSwitch:
                HandleConnectingLightWithSwitchInputs();
                break;
        }
        
    }

    public void ChangeInputMode(OurInputMode mode){
        if(inputMode == OurInputMode.PlacingObject || inputMode == OurInputMode.DeletingObject){
            if(currentPreview != null){
                Destroy(currentPreview);
            }
           
        }
        inputMode = mode;
    }
    public void ChangeInputModeToDelete(){
        if(inputMode == OurInputMode.DeletingObject){
            ChangeInputModeToPlace();
            return;
        }
        ChangeInputMode(OurInputMode.DeletingObject);
        trashButton.sprite = trashOn;
    }
    public void ChangeInputModeToPlace(){
        ChangeInputMode(OurInputMode.PlacingObject);
        trashButton.sprite = trashOff;

    }
    private void HandlePlacingObjectInputs(){
        //Try Place, noly allowed when nothing there
        if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch)){
            Debug.Log("Button pressed for placing");
            if( !IsPositionOccupied(getPositionFromRaycast())){
                PlacePrefabAtControllerPointedSpot();
            }

        }
        else{
            TryPlacePreviewPrefab();
        
        }
    }
    private void HandleDeletingObjectInputs(){
        var objs = GetPlaceableObjectsAtPointedSpot();
        //try delete
        if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch)){
            foreach(var obj in objs){
                obj.GetComponent<PlaceableObject>().DestroyAllConnectedCables();
                if(obj.transform.parent != null)
                    Destroy(obj);
                else
                    Destroy(obj.transform.parent.gameObject);
            }
            EventManager.Instance.TriggerObjectDeleted();
            
        }
        else{
            TryPlacePreviewPrefab();
        }
    }
    private void HandleConnectingLightWithSwitchInputs(){
    //while user holds the trigger, we connect the light with the switch
    //if the user releases the trigger, we stop connecting
    // TODO 
    }
    public void PlacePrefabAtControllerPointedSpot(){
        if (currentObject != null){
            Quaternion rot = Quaternion.LookRotation(getSurfaceNormalFromPointedWall());
            Vector3 pos = getPositionFromRaycast();
            var obj = Instantiate(currentObject.prefabForPlacing, pos, rot);
            //StartCoroutine(WaitAndSaveAnchor(anchor));
            if(obj.transform.GetChild(0).GetComponent<PowerGrid>() != null){
                PowerSourcePlacer.Instance.powergrid = obj.transform.GetChild(0).gameObject;
            }
            else{
                EventManager.Instance.TriggerObjectPlaced(obj.transform.GetChild(0).gameObject);
            }

            
        
        }
    }

   

        
    private void TryPlacePreviewPrefab(){
        //if the user is pointing at a wall or floor, we place the preview prefab at the closest point
        if (currentObject != null){
            if(currentPreview == null){
                if(inputMode == OurInputMode.PlacingObject)
                    currentPreview = Instantiate(currentObject.previewPrefab);
                else{
                    currentPreview = Instantiate(previewPrefabForDeleting);
                    }
            }
            Vector3 position = getPositionFromRaycast();
            if (position == Vector3.zero || (inputMode == OurInputMode.PlacingObject && IsPositionOccupied(position))){
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
        LabelFilter filter;
        if(inputMode == OurInputMode.DeletingObject){
             filter = LabelFilter.Included(new List<string> {"WALL_FACE", "CEILING"});
        }
        else{
            filter = LabelFilter.Included(new List<string> {currentObject.SurfaceToSpawnOnString});

        }
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

        LabelFilter filter;
        if(inputMode == OurInputMode.DeletingObject){
             filter = LabelFilter.Included(new List<string> {"WALL_FACE", "CEILING"});
        }
        else{
            filter = LabelFilter.Included(new List<string> {currentObject.SurfaceToSpawnOnString});
        }
        RaycastHit hit;
        MRUKAnchor anchor;
        
        MRUK.Instance.GetCurrentRoom().Raycast(ray, 2000f, filter,out hit, out anchor);
        if (ReferenceEquals(hit, null)){
            return Vector3.zero;
        }
        return hit.point;
    }

    private bool IsPositionOccupied(Vector3 position){
        if (position == Vector3.zero){
            return true; // Position is occupied
        }
        float checkRadius = 0.1f; // Adjust based on the size of the objects being placed
        Collider[] hitColliders = Physics.OverlapSphere(position, checkRadius);
        foreach (var hitCollider in hitColliders){
            if (hitCollider.gameObject.tag == "PlacedObject"){ // Ensure the current preview object is not considered as an obstacle
                return true; // Position is occupied
            }
        }
        return false; // Position is not occupied
    }
    

    private GameObject[] GetPlaceableObjectsAtPointedSpot(){
            Vector3 position = getPositionFromRaycast();
        if (position != Vector3.zero){
            Collider[] hitColliders = Physics.OverlapSphere(position, 0.05f);
            return hitColliders.Where(x => x.gameObject.tag == "PlacedObject").Select(x => x.gameObject).ToArray();
        }
        else{
            return null;
        }
    }

    
}
public enum OurInputMode
{
    PlacingObject,
    DeletingObject,
    ConnectingLightWithSwitch
}
    
