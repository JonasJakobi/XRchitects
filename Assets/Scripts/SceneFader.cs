using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public Image fadeOutImage;
    public float fadeDuration = 1.0f;
    public string nextSceneName = "Scenes/sampleScene";

    private void Start()
    {
        fadeOutImage.gameObject.SetActive(true);
    }

    public void FadeToScene()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0.0f;
        Color color = fadeOutImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeOutImage.color = color;
            yield return null;
        }

        SceneManager.LoadScene(nextSceneName);
    }
}
