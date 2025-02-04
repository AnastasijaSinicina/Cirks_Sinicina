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

    void Start()
    {
        // Set up volume slider
        volumeSlider.onValueChanged.AddListener(SetVolume);

        // Set up music dropdown
        musicDropdown.onValueChanged.AddListener(ChangeMusic);
        PopulateMusicDropdown();

        // Set up resolution dropdown
        resolutionDropdown.onValueChanged.AddListener(ChangeResolution);
        PopulateResolutionDropdown();
    }

    // Method to set the volume
    void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    // Method to change music
    void ChangeMusic(int index)
    {
        audioSource.clip = musicClips[index];
        audioSource.Play();
    }

    // Method to change resolution
    void ChangeResolution(int index)
    {
        Resolution resolution = Screen.resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    // Populate the music dropdown
    void PopulateMusicDropdown()
    {
        musicDropdown.options.Clear();
        foreach (var clip in musicClips)
        {
            musicDropdown.options.Add(new TMP_Dropdown.OptionData(clip.name));
        }
    }

    // Populate the resolution dropdown
    void PopulateResolutionDropdown()
    {
        resolutionDropdown.options.Clear();
        foreach (var res in Screen.resolutions)
        {
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(res.width + " x " + res.height));
        }
    }
}
