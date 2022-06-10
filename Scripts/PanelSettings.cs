using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanelSettings : MonoBehaviour
{
    public AudioSource _audio;
    public Dropdown dropdown;
    public GameObject delete;
    public Toggle fog, fps;
    public Slider slider, slider2;
    public Image vibration;
    public Sprite[] sprites;
    bool isVibration;

    public void PlayClick()
    {
        if (PlayerPrefs.GetInt("Sound") == 1 && Time.time > 0.1f)        // устранение бага с звуком клика кнопки при старте
            _audio.Play();
    }

    public void Fog()
    {
        PlayerPrefs.SetInt("Fog", fog.isOn ? 1 : 0);
    }

    public void FPS()
    {
        PlayerPrefs.SetInt("FPS", fps.isOn ? 1 : 0);
    }

    public void Vibration()
    {
        isVibration = !isVibration;
        vibration.sprite = sprites[isVibration ? 1 : 0];
        vibration.color = isVibration ? new Color(1, 1, 1, 0.9f) : new Color(1, 1, 1, 0.4f);
        if (isVibration)
            Handheld.Vibrate();
        PlayerPrefs.SetInt("Vibration", isVibration ? 1 : 0);
        PlayClick();
    }

    public void Difficulty()
    {
        PlayerPrefs.SetInt("Difficulty", dropdown.value);
        PlayClick();
    }

    void Start()
    {
        dropdown.value = PlayerPrefs.GetInt("Difficulty");

        fog.isOn = PlayerPrefs.GetInt("Fog") == 1;
        fps.isOn = PlayerPrefs.GetInt("FPS") == 1;

        isVibration = PlayerPrefs.GetInt("Vibration") == 1;
        vibration.sprite = sprites[isVibration ? 1 : 0];
        vibration.color = isVibration ? new Color(1, 1, 1, 0.9f) : new Color(1, 1, 1, 0.4f);
    }
}