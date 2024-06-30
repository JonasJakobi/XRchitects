using System.Collections;
using UnityEngine;

public class MoveOnTrigger : MonoBehaviour
{
    public GameObject button;
    public float moveDistance = 1.0f;

    private Vector3 originalLocalPosition;
    private bool isUp = false;

    void Start()
    {
        if (button == null)
        {
            Debug.LogError("Button GameObject is not assigned!");
            return;
        }
        originalLocalPosition = button.transform.localPosition;
    }

    public void MoveUp()
    {
        if (!isUp)
        {
            button.transform.localPosition = originalLocalPosition + new Vector3(0, 0, moveDistance);
            StartCoroutine(HapticPulse(OVRInput.Controller.RTouch));
            isUp = true;
            Debug.Log("Button moved up. New local position: " + button.transform.localPosition);
            
        }
    }

    public void MoveBack()
    {
        if (isUp)
        {
            button.transform.localPosition = originalLocalPosition;
            StopCoroutine(HapticPulse(OVRInput.Controller.RTouch));
            isUp = false;
            Debug.Log("Button moved back. New local position: " + button.transform.localPosition);
        }
    }

    public IEnumerator HapticPulse(OVRInput.Controller controller)
    {
            OVRInput.SetControllerVibration(1f, 0.2f, controller);
            yield return new WaitForSeconds(0.1f);
            OVRInput.SetControllerVibration(0, 0, controller);
    }
}