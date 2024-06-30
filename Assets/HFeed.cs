using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class HFeed : MonoBehaviour
{
    
    private bool isRunningH = false;
    public float intensity = 0.1f;
    public float duration = 0.1f;


    public void StartHFeed()
    {
        if (!isRunningH)
        {
            
            StartCoroutine(BHapticPulse(OVRInput.Controller.RTouch));
            isRunningH = true;
        }
    }

    public void StopHFeed()
    {
        if(isRunningH){
            
            StopCoroutine(BHapticPulse(OVRInput.Controller.RTouch));
            isRunningH = false;
        }
    }

    public IEnumerator BHapticPulse(OVRInput.Controller controller)
    {
            OVRInput.SetControllerVibration(1f, intensity, controller);
            yield return new WaitForSeconds(duration);
            OVRInput.SetControllerVibration(0f, 0f, controller);
    }

}
