using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public AudioManager audioManager;

    Resolution[] resolutions;

    public TMP_Dropdown resolutionDropDown;

    public float mousAcceleration = 1f;

    public static OptionsMenu Instance { get; private set; }



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);


            resolutions = Screen.resolutions;

            resolutionDropDown.ClearOptions();

            List<string> options = new List<string>();

            int currentResIndex = 0;

            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                    currentResIndex = i;
            }


            resolutionDropDown.AddOptions(options);
            resolutionDropDown.value = currentResIndex;
            resolutionDropDown.RefreshShownValue();
        }
        else
        {
            Destroy(gameObject);
        }


    }

    public void setVolume(float volume)
    {
        audioMixer.SetFloat("volume",volume);
       
    }

    public void setAcceleration(float acceleration)
    {
        mousAcceleration = acceleration;
    }

    public void setFullscreen (bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void setResoulution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width,resolution.height , Screen.fullScreen);
    }
}
