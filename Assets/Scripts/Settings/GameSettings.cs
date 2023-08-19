using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class GameSettings : MonoBehaviour
{
    public AudioMixer MasterAudioMixer;

    Resolution[] ResolutionsAvailableList;

    public Slider GameVolumeSlider;

    public Toggle FullScreenToggle;

    public Dropdown GameResolutionDropdown;

    public TMP_Dropdown GameQualityDropdown;

    private void Start()
    {
        ResolutionsAvailableList = Screen.resolutions;
        GameResolutionDropdown.ClearOptions();

        List<string> ResolutionOptionsStringList = new List<string>();

        int currentResolutionIndex = 0;
        float currentVolume = 0;
        bool currentFullScreenModeEnabled = false;
        int currentGraphicsQualityIndex = 0;

        for(int i = 0; i < ResolutionsAvailableList.Length; i++)
        {
            string ResolutionOption = ResolutionsAvailableList[i].width + "X" + ResolutionsAvailableList[i].height + " @ " + ResolutionsAvailableList[i].refreshRate + "Hz";
            ResolutionOptionsStringList.Add(ResolutionOption);

            if(ResolutionsAvailableList[i].width == Screen.currentResolution.width && ResolutionsAvailableList[i].height == Screen.currentResolution.height && ResolutionsAvailableList[i].refreshRate == Screen.currentResolution.refreshRate)
            {
                currentResolutionIndex = i;
            }
        }

        GameResolutionDropdown.AddOptions(ResolutionOptionsStringList);
        GameResolutionDropdown.value = currentResolutionIndex;
        GameResolutionDropdown.RefreshShownValue();

        MasterAudioMixer.GetFloat("Volume", out currentVolume);
        GameVolumeSlider.value = currentVolume;

        currentFullScreenModeEnabled = Screen.fullScreen;
        FullScreenToggle.isOn = currentFullScreenModeEnabled;

        currentGraphicsQualityIndex = QualitySettings.GetQualityLevel();
        GameQualityDropdown.value = currentGraphicsQualityIndex;
        GameQualityDropdown.RefreshShownValue();


    }

    public void SetGraphicsQuality(int GraphicsQualityIndex)
    {
        QualitySettings.SetQualityLevel(GraphicsQualityIndex);
    }

    public void SetGameVolume(float Volume)
    {
        MasterAudioMixer.SetFloat("Volume", Volume);
    }

    public void SetGameFullScreen(bool IsFullScreen)
    {
        Screen.fullScreen = IsFullScreen;
    }

    public void SetGameResolution(int resolutionIndex)
    {
        Resolution resolution = ResolutionsAvailableList[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
