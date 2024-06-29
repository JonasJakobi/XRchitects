using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMRTransition : MonoBehaviour
{

    // Drag and Drop the Passthrough Building Block here
    [SerializeField] private OVRPassthroughLayer passthroughLayer;

    private float transitionSpeed = 2f;
    private bool isTransitioning = false;



    public void TransitionToVR()
    {
        if (isTransitioning == false)
        {
            StartCoroutine("CR_TransitionToVR");
        }
    }

    private IEnumerator CR_TransitionToVR()
    {
        while (passthroughLayer.textureOpacity > 0f)
        {
            isTransitioning = true;
            passthroughLayer.textureOpacity -= Time.deltaTime * transitionSpeed;
            Debug.Log(passthroughLayer.textureOpacity);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        isTransitioning = false;
        Debug.Log("Stopped Coroutine CR_TransitionToVR");
    }







    public void TransitionToMR()
    {
        if (isTransitioning == false)
        {
            StartCoroutine("CR_TransitionToMR");
        }
    }

    private IEnumerator CR_TransitionToMR()
    {
        while (passthroughLayer.textureOpacity < 1f)
        {
            isTransitioning = true;
            passthroughLayer.textureOpacity += Time.deltaTime * transitionSpeed;
            Debug.Log(passthroughLayer.textureOpacity);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        isTransitioning = false;
        Debug.Log("Stopped Coroutine CR_TransitionToMR");
    }
}
