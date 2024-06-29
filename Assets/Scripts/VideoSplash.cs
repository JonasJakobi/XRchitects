using UnityEngine;
using UnityEngine.Video;

public class VideoSplash : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public SceneFader sceneFader;

    private void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        sceneFader.FadeToScene();
    }
}
