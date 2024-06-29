using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class SplashScene : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName = "Scenes/sampleScene"; 

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished; 
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName); // Scene loader
    }
}
