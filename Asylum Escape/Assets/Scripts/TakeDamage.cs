using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TakeDamage : MonoBehaviour
{
    public float intensity = 0;
    public Player player;
    public CreatureAI demon;

    private PostProcessVolume volume;
    private Vignette vignette;
    private bool active = false;
    void Start()
    {
        volume = GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings<Vignette>(out vignette);

        if (!vignette)
        {
            print("error vignette empty");
        }
        else
        {
            vignette.enabled.Override(false);
        }
    }

    void Update()
    {
        if (player.hp == 1 && active == false)
        {
            active = true;
            StartCoroutine(TakeDamageEffect());
        }

    }

    public IEnumerator TakeDamageEffect() {
        intensity = 0.35f;
        vignette.enabled.Override(true);
        vignette.intensity.Override(intensity);

        float timer = player.hpTimeThreshold / 5;

        while (timer > 0) {
            Debug.Log(intensity);
            intensity -= 0.01f;
            timer -= 0.1f;

            if (intensity < 0.15) intensity = 0.15f;

            vignette.intensity.Override(intensity);

            yield return new WaitForSeconds(0.5f);
        }

        intensity = 0;
        active = false;
        vignette.enabled.Override(false);
        yield break;
    }
}
