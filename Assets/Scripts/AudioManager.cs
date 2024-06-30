using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioClip placementSound;
    public AudioClip buttonSound;
    public AudioSource audioSource;

    private void Start() {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        //subscribe to placement event
      //  EventManager.Instance.OnObjectPlaced += PlayPlacementSound;
    }
    public void PlayPlacementSound()
    {
        audioSource.PlayOneShot(placementSound);
    }

    public void PlayButton(){
        audioSource.PlayOneShot(buttonSound);
    }
}
