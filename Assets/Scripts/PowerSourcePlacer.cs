using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using UnityEngine;

public class PowerSourcePlacer : MonoBehaviour
{

    public GameObject powerGridPrefab;
    public GameObject cablePrefab;


    public GameObject powergrid;
    // Start is called before the first frame update
    void Start()
    {
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
        EventManager.Instance.OnObjectPlaced += CreateCablesBetweenObjAndPowerGrid;
    }
    public void CreateCablesBetweenObjAndPowerGrid(GameObject obj)
    {
        //First we create a cable from the obj to the ceiling. Then from that point to the powergrid
        var cableToCeiling = Instantiate(cablePrefab);
        cableToCeiling.GetComponent<CableController>().startPoint = obj.transform;
        cableToCeiling.GetComponent<CableController>().endPoint = Instantiate(new GameObject()).transform;
        cableToCeiling.GetComponent<CableController>().endPoint.position = new Vector3(obj.transform.position.x, powergrid.transform.position.y, obj.transform.position.z);

        var cableToPowerGrid = Instantiate(cablePrefab);
        cableToPowerGrid.GetComponent<CableController>().startPoint = cableToCeiling.GetComponent<CableController>().endPoint;
        cableToPowerGrid.GetComponent<CableController>().endPoint = powergrid.transform;

        obj.GetComponent<PlaceableObject>().SetConnectedToElectricity(true);

    
    }
    // Update is called once per frame


}
