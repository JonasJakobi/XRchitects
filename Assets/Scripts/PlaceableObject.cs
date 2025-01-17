using System.Collections;
using System.Collections.Generic;
using Meta.XR.BuildingBlocks;
using Meta.XR.MRUtilityKit;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using Sequence = DG.Tweening.Sequence;
public class PlaceableObject : MonoBehaviour{


    private bool connectedToElectricity = false;

    public List<GameObject> cables = new List<GameObject>();
    private void Start(){
        AudioManager.Instance.PlayPlacementSound();
        SetOutlineBasedOnElectricity();
        Vector3 localScale = transform.localScale;
        
        transform.localScale = new Vector3(0,0,0);
        Sequence plop = DOTween.Sequence();
        transform.DOScale(localScale, 1f).SetEase(Ease.OutBack);
       
        
    }


    public bool GetConnectedToElectricity(){
        return connectedToElectricity;
    }
    public void SetConnectedToElectricity(bool value){
        connectedToElectricity = value;
        SetOutlineBasedOnElectricity();
    }

    private void SetOutlineBasedOnElectricity(){
        if(connectedToElectricity){
            //set outline width to 0.1f if the object is connected to electricity
            GetComponent<Outline>().OutlineColor = Color.green;
            GetComponent<Outline>().OutlineWidth = 1;
        }
        else{
            //set outline width to 0.0f if the object is not connected to electricity
            GetComponent<Outline>().OutlineColor = Color.red;
            GetComponent<Outline>().OutlineWidth = 1;

        }
    }

    public void DestroyAllConnectedCables(){
        //go from endpoint to endpoint and destroy them backwards.
        foreach(var cable in cables){
            Destroy(cable);
            
        }

    }

    public void AddCable(GameObject cable){
        cables.Add(cable);
    }
}