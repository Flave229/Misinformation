using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.IO;

public class SettingsMenu : MonoBehaviour
{
    public GameSettings gameSettings;
    public AudioMixer audioMixer;
    public Dropdown resolutionDropdown;
    public Resolution[] resolutions;

    void OnEnable()
    {
        gameSettings = new GameSettings();
        SetUpResolutions();
        if (File.Exists(Application.persistentDataPath + "/gamesettings.json") == true)
        {
            LoadSettings();
            SetFullscreen(gameSettings.fullscreen);
            SetResolution(gameSettings.resolutionIndex);
            SetVolume(gameSettings.volume);
        }
        else
            SaveSettings();
    }

    public void SetVolume(float volume)
    {
        gameSettings.volume = volume;
        audioMixer.SetFloat("volume", gameSettings.volume);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        gameSettings.fullscreen = isFullscreen;
        Screen.fullScreen = gameSettings.fullscreen;
    }

    public void SetUpResolutions()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        gameSettings.resolutionIndex = currentResolutionIndex;
        RefreshResolutionDropdown();
    }

    public void SetResolution(int resolutionIndex)
    {
        gameSettings.resolutionIndex = resolutionIndex;
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        RefreshResolutionDropdown();
    }
    private void RefreshResolutionDropdown()
    {
        resolutionDropdown.value = gameSettings.resolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SaveSettings()
    {
        string jsonData = JsonUtility.ToJson(gameSettings);
        File.WriteAllText(Application.persistentDataPath + "/gamesettings.json", jsonData);
    }

    public void LoadSettings()
    {
        gameSettings = JsonUtility.FromJson<GameSettings>(File.ReadAllText(Application.persistentDataPath + "/gamesettings.json"));
        /*Screen.fullScreen = gameSettings.fullscreen;
        Resolution resolution = resolutions[gameSettings.resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, gameSettings.fullscreen);
        audioMixer.SetFloat("volume", gameSettings.volume);*/
    }
}
