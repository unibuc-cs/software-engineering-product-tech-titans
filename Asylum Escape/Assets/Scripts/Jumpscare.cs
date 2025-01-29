using System.Collections;
using UnityEngine;

public class Jumpscare : MonoBehaviour
{
    [SerializeField] private Light[] lightList;
    private bool[] lightFlickers;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private float audioTime = 2.5f;

    [SerializeField] private float minIntensity = 0.1f;
    [SerializeField] private float maxIntensity = 1f;
    [SerializeField] private float flickerSpeed = 0.1f;

    [SerializeField] private float flickeringDuration = 3.0f;
    [SerializeField] private bool lightTurnsOff = false;

    private void Awake()
    {
        lightFlickers = new bool[lightList.Length];
        for (int i = 0; i < lightList.Length; i++)
        {
            lightFlickers[i] = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (audioClip != null)
        {
            AudioManager.Instance.PlaySFXForXSeconds(audioClip, audioTime);
        }

        for (int i = 0; i < lightList.Length; i++)
        {
            if (!lightFlickers[i])
            {
                StartCoroutine(FlickerLight(i));
            }
        }
    }

    private IEnumerator FlickerLight(int index)
    {
        lightFlickers[index] = true;
        float endTime = Time.time + flickeringDuration; /// we make the light Flicker for 3 seconds

        while (Time.time < endTime)
        {
            // float flickerValue = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PerlinNoise(Time.time * flickerSpeed, 0));
            float flickerValue = Random.Range(0f, 1f) > 0.8f ? maxIntensity : minIntensity; // 0.8 we want it to be dark most of the flickering
            lightList[index].intensity = flickerValue;
            yield return new WaitForSeconds(flickerSpeed);
        }

        // After 3 seconds, we set the light intesity back to normal
        
        if (lightTurnsOff == true)
        {
            lightList[index].intensity = 0.0f;
        }
        else
        {
            lightList[index].intensity = 1.0f;
        }

        lightFlickers[index] = false;

    }

    private void OnTriggerExit(Collider other)
    {
        this.GetComponent<BoxCollider>().enabled = false;
    }
}
