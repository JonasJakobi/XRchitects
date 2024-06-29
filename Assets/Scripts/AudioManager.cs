using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip placementSound;
    public AudioSource audioSource;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
        //subscribe to placement event
      //  EventManager.Instance.OnObjectPlaced += PlayPlacementSound;
    }
    private void PlayPlacementSound(GameObject obj)
    {
        if (placementSound != null)
        {
            audioSource.PlayOneShot(placementSound);
        }
    }
}
