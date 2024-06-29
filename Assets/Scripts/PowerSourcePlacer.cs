using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Meta.XR.MRUtilityKit;
using UnityEngine;

public class PowerSourcePlacer : MonoBehaviour
{
    private bool showingCables = true;
    public static PowerSourcePlacer Instance;
    public GameObject powerGridPrefab;
    public GameObject cablePrefab;

    public GameObject CableConnectionPrefab;
    public GameObject powergrid;

    private List<GameObject> cables = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        StartCoroutine(WaitAndDoStartRoutine());
    }

   

    private IEnumerator WaitAndDoStartRoutine(){
        //wait for scene to be ready
        yield return new WaitForSeconds(2);
        //Find Biggest wall and place power source in the middle
        Vector2 wallScale;
        MRUKAnchor wallanch = MRUK.Instance.GetCurrentRoom().GetKeyWall(out wallScale);
        Bounds bounds = MRUK.Instance.GetCurrentRoom().GetRoomBounds();
        var center = bounds.center;
        Vector3 powergridPos;
        wallanch.GetClosestSurfacePosition(center, out powergridPos);
        powergrid = Instantiate(powerGridPrefab, powergridPos, Quaternion.identity);
        EventManager.Instance.OnObjectPlaced += CreateCableToPowerGrid;
    }
    public void CreateCableToPowerGrid(GameObject obj){
        CreateCablesBetweenObjects(obj, powergrid);
    }
    public void CreateCablesBetweenObjects(GameObject obj, GameObject other)
    {   

        MRUKAnchor startWall = GetWallClosestTo(obj.transform, new List<MRUKAnchor>());
        MRUKAnchor endWall = GetWallClosestTo(other.transform, new List<MRUKAnchor>(){startWall});
        CableController lastCable;
        //First we create a cable from the obj to the ceiling. Then from that point to the other
        CableController newCable = Instantiate(cablePrefab).GetComponent<CableController>();
        newCable.startPoint = obj.transform.GetChild(0).transform;
        obj.GetComponent<PlaceableObject>().AddCable(newCable.gameObject);
        newCable.endPoint = Instantiate(CableConnectionPrefab).transform;
        obj.GetComponent<PlaceableObject>().AddCable(newCable.endPoint.gameObject);
        newCable.endPoint.position = newCable.startPoint.position;
        newCable.endPoint.position = new Vector3(newCable.endPoint.position.x, GetWallHeight(), newCable.endPoint.position.z);
        lastCable = newCable;
        //check if the closest wall is the endwall
        if(endWall != startWall){
            //we ahve to transition over a sidewall
            if(GetWallClosestTo(newCable.endPoint, new List<MRUKAnchor> {startWall}) != endWall){
                //we have to go around two corners
                MRUKAnchor cornerWall = GetWallClosestTo(lastCable.endPoint, new List<MRUKAnchor> {startWall, endWall});
                newCable = Instantiate(cablePrefab).GetComponent<CableController>();
                obj.GetComponent<PlaceableObject>().AddCable(newCable.gameObject);

                newCable.startPoint = lastCable.endPoint;
                newCable.endPoint = Instantiate(CableConnectionPrefab).transform;
                obj.GetComponent<PlaceableObject>().AddCable(newCable.endPoint.gameObject);

                newCable.endPoint.position = lastCable.startPoint.position;
                newCable.endPoint.position = new Vector3(lastCable.endPoint.position.x, GetWallHeight(), lastCable.endPoint.position.z);
                Vector3 epos;
                cornerWall.GetClosestSurfacePosition(lastCable.endPoint.position, out epos);
                newCable.endPoint.position = epos;
                lastCable = newCable;

                //cable all the way to closest point of endwall\
                /*
                newCable = Instantiate(cablePrefab).GetComponent<CableController>();
                newCable.startPoint = lastCable.endPoint;
                newCable.endPoint = Instantiate(CableConnectionPrefab).transform;
                newCable.endPoint.position = endWall.transform.position;
                newCable.endPoint.position = new Vector3(newCable.endPoint.position.x, GetWallHeight(), newCable.endPoint.position.z);
                Vector3 poss;
                endWall.GetClosestSurfacePosition(lastCable.endPoint.position, out poss);
                newCable.endPoint.position = poss;
                lastCable = newCable;
                */

            }
            //cable to endwall
            newCable = Instantiate(cablePrefab).GetComponent<CableController>();
            obj.GetComponent<PlaceableObject>().AddCable(newCable.gameObject);

            newCable.startPoint = lastCable.endPoint;
            newCable.endPoint = Instantiate(CableConnectionPrefab).transform;
            obj.GetComponent<PlaceableObject>().AddCable(newCable.endPoint.gameObject);

            newCable.endPoint.position = lastCable.startPoint.position;
            newCable.endPoint.position = new Vector3(lastCable.endPoint.position.x, GetWallHeight(), lastCable.endPoint.position.z);
            Vector3 endpos;
            endWall.GetClosestSurfacePosition(lastCable.endPoint.position, out endpos);
            newCable.endPoint.position = endpos;
            lastCable = newCable;

            
        }
        //cable to right above powerrgrid
            newCable = Instantiate(cablePrefab).GetComponent<CableController>();
            obj.GetComponent<PlaceableObject>().AddCable(newCable.gameObject);

            newCable.startPoint = lastCable.endPoint;
            newCable.endPoint = Instantiate(CableConnectionPrefab).transform;
            obj.GetComponent<PlaceableObject>().AddCable(newCable.endPoint.gameObject);
            newCable.endPoint.position = other.transform.position;
            newCable.endPoint.position = new Vector3(newCable.endPoint.position.x, GetWallHeight(), newCable.endPoint.position.z);
            lastCable = newCable;


        //cable down to powergrid
        newCable = Instantiate(cablePrefab).GetComponent<CableController>();
        obj.GetComponent<PlaceableObject>().AddCable(newCable.gameObject);
        newCable.startPoint = lastCable.endPoint;
        newCable.endPoint = other.transform;
        obj.GetComponent<PlaceableObject>().SetConnectedToElectricity(true);

    }
    public float GetWallHeight(){
        return MRUK.Instance.GetCurrentRoom().GetRoomBounds().size.y - 0.2f;
    }
    public MRUKAnchor GetWallClosestTo(Transform transform, List<MRUKAnchor> wallAnchorsToIgnore){
        
        List<MRUKAnchor> walls = MRUK.Instance.GetCurrentRoom().WallAnchors;
        if(wallAnchorsToIgnore != null){
            walls = walls.Except(wallAnchorsToIgnore).ToList();
        }
        return walls.OrderBy(wall => getDistanceToWall(wall, transform.position)).First();
        
    }

    public float getDistanceToWall(MRUKAnchor wall, Vector3 point){
        Vector3 wallPos;
        wall.GetClosestSurfacePosition(point, out wallPos);
        return Vector3.Distance(point, wallPos);
    }
    public bool GetShowingCables(){
        return showingCables;
    }
    public void SetShowingCables(bool value){
        showingCables = value;
        if(showingCables){
            foreach(var cable in cables){
                cable.GetComponent<CableController>().ShowCable();
            }
        }
        else{
            foreach(var cable in cables){
                cable.GetComponent<CableController>().HideCable();
            }
        }
    }

    public void RegisterCable(GameObject cable){
        cables.Add(cable);
    }
    public void UnregisterCable(GameObject cable){
        cables.Remove(cable);
    }


}

