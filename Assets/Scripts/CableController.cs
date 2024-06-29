
using DG.Tweening;
using UnityEngine;

public class CableController : MonoBehaviour
{
    public Transform startPoint; // The starting point of the cable
    public Transform endPoint; // The ending point of the cable


    private void Start() {
        PowerSourcePlacer.Instance.RegisterCable(this.gameObject);
        float xscale = transform.localScale.x;
        float zscale = transform.localScale.z;
        transform.localScale = new Vector3(0, transform.localScale.y, 0);
        transform.DOScaleX(xscale, 1f).SetEase(Ease.OutBack);
        transform.DOScaleZ(zscale, 1f).SetEase(Ease.OutBack);
    }
    void Update()
    {
        if (startPoint != null && endPoint != null)
        {
            // Calculate the midpoint between start and end points
            Vector3 midPoint = (startPoint.position + endPoint.position) / 2;
            transform.position = midPoint;

            // Calculate the direction and length between start and end points
            Vector3 direction = endPoint.position - startPoint.position;
            float distance = direction.magnitude;

            // Adjust the scale and rotation of the cylinder
            transform.localScale = new Vector3(transform.localScale.x, distance / 2, transform.localScale.z);
            transform.up = direction; // Align the cylinder along the direction
        }
    }
    private void OnDestroy() {
        PowerSourcePlacer.Instance.UnregisterCable(this.gameObject);
    }

    public void HideCable(){
        GetComponent<MeshRenderer>().enabled = false;
    }
    public void ShowCable(){
        GetComponent<MeshRenderer>().enabled = true;
    }
}