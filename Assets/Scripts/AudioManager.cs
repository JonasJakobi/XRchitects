using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioClip placementSound;
    public AudioClip buttonSound;
    public AudioClip plugInSound;
    public AudioClip plugOutSound;
    public AudioSource audioSource;

    private bool plugIn = true;

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

    public void PlayPlug(){
        if(plugIn){
            audioSource.PlayOneShot(plugInSound);
        }else{
            audioSource.PlayOneShot(plugOutSound);
        }
        plugIn = !plugIn;

    }

}
