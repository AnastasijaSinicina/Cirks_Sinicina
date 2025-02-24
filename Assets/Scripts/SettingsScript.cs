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
    public Button saveButton;

    private Resolution[] resolutions;

    void Start()
    {
        // Устанавливаем доступные разрешения
        resolutions = Screen.resolutions;
        PopulateResolutionDropdown();
        PopulateMusicDropdown();

        // Загружаем настройки
        LoadSettings();

        // Подписки на события
        volumeSlider.onValueChanged.AddListener(SetVolume);
        musicDropdown.onValueChanged.AddListener(ChangeMusic);
        resolutionDropdown.onValueChanged.AddListener(ChangeResolution);
        saveButton.onClick.AddListener(SaveSettings);
    }

    void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    void ChangeMusic(int index)
    {
        if (index >= 0 && index < musicClips.Length)
        {
            audioSource.clip = musicClips[index];
            audioSource.Play();
        }
    }

    void ChangeResolution(int index)
    {
        if (index >= 0 && index < resolutions.Length)
        {
            Resolution resolution = resolutions[index];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }
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
        foreach (var res in resolutions)
        {
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(res.width + " x " + res.height));
        }
    }

    void SaveSettings()
    {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.SetInt("MusicIndex", musicDropdown.value);

        Resolution currentRes = Screen.currentResolution;
        PlayerPrefs.SetInt("ResWidth", currentRes.width);
        PlayerPrefs.SetInt("ResHeight", currentRes.height);
        
        PlayerPrefs.Save();
    }

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

        if (PlayerPrefs.HasKey("ResWidth") && PlayerPrefs.HasKey("ResHeight"))
        {
            int width = PlayerPrefs.GetInt("ResWidth");
            int height = PlayerPrefs.GetInt("ResHeight");

            for (int i = 0; i < resolutions.Length; i++)
            {
                if (resolutions[i].width == width && resolutions[i].height == height)
                {
                    resolutionDropdown.value = i;
                    ChangeResolution(i);
                    break;
                }
            }
        }
    }
}
