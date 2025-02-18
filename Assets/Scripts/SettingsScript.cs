using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public Slider volumeSlider;
    public TMP_Dropdown musicDropdown;
    public TMP_Dropdown resolutionDropdown;
    public AudioSource audioSource;
    public AudioClip[] musicClips;
    public Button saveButton; // Reference to Save Button

    void Start()
    {
        // Load saved settings
        LoadSettings();

        // Set up volume slider
        volumeSlider.onValueChanged.AddListener(SetVolume);

        // Set up music dropdown
        musicDropdown.onValueChanged.AddListener(ChangeMusic);
        PopulateMusicDropdown();

        // Set up resolution dropdown
        resolutionDropdown.onValueChanged.AddListener(ChangeResolution);
        PopulateResolutionDropdown();

        // Set up Save button
        saveButton.onClick.AddListener(SaveSettings);
    }

    void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    void ChangeMusic(int index)
    {
        audioSource.clip = musicClips[index];
        audioSource.Play();
    }

    void ChangeResolution(int index)
    {
        Resolution resolution = Screen.resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    void PopulateMusicDropdown()
    {
        musicDropdown.options.Clear();
        foreach (var clip in musicClips)
        {
            musicDropdown.options.Add(new TMP_Dropdown.OptionData(clip.name));
        }
    }

    void PopulateResolutionDropdown()
    {
        resolutionDropdown.options.Clear();
        foreach (var res in Screen.resolutions)
        {
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(res.width + " x " + res.height));
        }
    }

    // Save settings to PlayerPrefs
    void SaveSettings()
    {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.SetInt("MusicIndex", musicDropdown.value);
        PlayerPrefs.SetInt("ResolutionIndex", resolutionDropdown.value);
        PlayerPrefs.Save();
    }

    // Load saved settings
    void LoadSettings()
    {
        if (PlayerPrefs.HasKey("Volume"))
        {
            volumeSlider.value = PlayerPrefs.GetFloat("Volume");
            SetVolume(volumeSlider.value);
        }

        if (PlayerPrefs.HasKey("MusicIndex"))
        {
            musicDropdown.value = PlayerPrefs.GetInt("MusicIndex");
            ChangeMusic(musicDropdown.value);
        }

        if (PlayerPrefs.HasKey("ResolutionIndex"))
        {
            resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionIndex");
            ChangeResolution(resolutionDropdown.value);
        }
    }
}
