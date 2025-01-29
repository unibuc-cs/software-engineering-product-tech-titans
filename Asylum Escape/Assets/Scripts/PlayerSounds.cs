using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField] private Player playerObject;
    [SerializeField] private AudioClip[] footStepsSounds;
    [SerializeField] private float footSpeed = 0.4f;
    [SerializeField] private float coroutineCooldown = 0.3f;

    private Coroutine currentCoroutine = null;
    private float lastCoroutineTime = 0f;

    void Update()
    {
        if (playerObject.GetComponent<PlayerActions>().isCrouching == true)
        {
            if (footSpeed == 0.4f)
            {
                // Tocmai am dat crouch
                footSpeed = 0.8f;
                RestartCoroutine();
            }
            footSpeed = 0.8f;
        }
        else
        {
            if (footSpeed == 0.8f)
            {
                footSpeed = 0.4f;
                RestartCoroutine();
            }
            footSpeed = 0.4f;
        }

        bool isMoving = (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && playerObject._isGrounded;

        if (isMoving && currentCoroutine == null && Time.time - lastCoroutineTime >= coroutineCooldown)
        {
            lastCoroutineTime = Time.time;
            currentCoroutine = StartCoroutine(PlayFootStepsSoundEveryXSeconds(footSpeed));
        }
        else if (!isMoving && currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
    }

    IEnumerator PlayFootStepsSoundEveryXSeconds(float seconds)
    {
        while (true)
        {
            int randomIndex = Random.Range(0, footStepsSounds.Length);
            AudioManager.Instance.PlaySFX(footStepsSounds[randomIndex]);
            yield return new WaitForSeconds(seconds);
        }
    }

    void RestartCoroutine()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        if (Time.time - lastCoroutineTime >= coroutineCooldown)
            currentCoroutine = StartCoroutine(PlayFootStepsSoundEveryXSeconds(footSpeed));
    }

}
