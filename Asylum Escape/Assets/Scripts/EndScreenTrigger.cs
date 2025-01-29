using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreenTrigger : MonoBehaviour
{
    public GameObject EndScreen;
    public string sceneToLoad; 
    public float fadeDuration = 1.5f; 

    private bool isFading = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isFading)
        {
            StartCoroutine(FadeToScene());
        }
    }

    private IEnumerator FadeToScene()
    {
        isFading = true;
        Image fadeImage = EndScreen.GetComponent<Image>();
        Color fadeColor = fadeImage.color;
        float elapsedTime = 0f;

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        EndScreen.SetActive(true);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            fadeColor.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            if (fadeColor.a > 0.9) fadeColor.a = 0.9f;
            fadeImage.color = fadeColor; 
            yield return null;
        }
    }
}
