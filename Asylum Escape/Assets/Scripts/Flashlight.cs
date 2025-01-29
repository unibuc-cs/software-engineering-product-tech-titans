using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Flashlight : MonoBehaviour
{
    public GameObject flashlight;
    public bool exists = false;

    [SerializeField] private AudioClip turnOn;
    [SerializeField] private AudioClip turnOff;
    [SerializeField] private AudioClip reloadBattery;
    [SerializeField] private AudioClip wrongReload;
    [SerializeField] private GameObject body;
    [SerializeField] private UI_Inventory inventory;

    [SerializeField] private float lifetime = 60.0f; // 60 secunde dureaza cu 2 baterii

    public bool on, off;    
    public int batteries = 0;

    private float flickeringDuration = 1.0f;
    private Coroutine currentCoroutine = null;

    void Start()
    {
        off = true;
        flashlight.GetComponent<Light>().enabled = false;
        body.active = false;
        // flashlight.SetActive(false);
    }


    void Update()
    {
        if (!exists)
            return;

        if (off && Input.GetButtonDown("F"))
        {
            flashlight.GetComponent<Light>().enabled = true;
            // flashlight.SetActive(true);
            AudioManager.Instance.PlaySFX(turnOn);
            off = false;
            on = true;
        }
        else if (on && Input.GetButtonDown("F"))
        {
            flashlight.GetComponent<Light>().enabled = false;
            // flashlight.SetActive(false);
            AudioManager.Instance.PlaySFX(turnOff);
            off = true;
            on = false;
        }

        if (on)
        {
            lifetime -= 1 * Time.deltaTime;
            flashlight.GetComponent<Light>().enabled = true;
        }

        if (off && currentCoroutine == null)
        {
            flashlight.GetComponent<Light>().enabled = false;
        }

        if (lifetime <= 0)
        {
            // Lanterna s-a descarcat
            currentCoroutine = StartCoroutine(FlickerLight());

            // flashlight.GetComponent<Light>().enabled = false;
            on = false;
            off = true;
            lifetime = 0;
        }

        if (lifetime >= 60)
        {
            // poate sa aiba maxim 2 baterii
            lifetime = 60;
        }

        if (Input.GetButtonDown("Reload Battery"))
        {
            if (batteries >= 1)
            {
                AudioManager.Instance.PlaySFX(reloadBattery);
                batteries -= 1;
                inventory.removeBattery();
                lifetime += 30;
            } 
            else
            {
                AudioManager.Instance.PlaySFX(wrongReload);
            }
        }

        if (batteries <= 0)
        {
            // Doar de precautie
            batteries = 0;
        }

    }


    private IEnumerator FlickerLight()
    {
        float endTime = Time.time + flickeringDuration; /// we make the light Flicker for 1 seconds

        while (Time.time < endTime)
        {
            float flickerValue = Random.Range(0f, 1f) > 0.8f ? 1.2f : 0.0f; // 0.8 we want it to be dark most of the flickering
            flashlight.GetComponent<Light>().intensity = flickerValue;
            yield return new WaitForSeconds(0.1f);
        }

        // After 1 seconds of flickering it turns off

        flashlight.GetComponent<Light>().intensity = 1.2f;
        flashlight.GetComponent<Light>().enabled = false;

        currentCoroutine = null;

    }

    public void showBody()
    {
        body.active = true;
    }

}