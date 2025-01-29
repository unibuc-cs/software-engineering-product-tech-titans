using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundEffect : MonoBehaviour
{
    [SerializeField] public float interval = 10f;
    [SerializeField] public AudioManager audio_manager;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            audio_manager.PlaySFX(audio_manager.randomNoise);
            timer = 0f;
        }
    }
}
