using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Buttons : MonoBehaviour
{
    public GameObject screen, run, ghost;
    public Sprite[] sprites;
    public Image pause, music, sound, vibration, centerPoint, ban, dot_1, dot_2, dot_3, dot_4, bg;
    public Slider sensitivity;
    public RectTransform panelSettings;
    public Joystick joystick;
    public CameraRotation cameraRotation;
    public Dropdown dropdown;
    public Text timer, fps;
    public Button goToCar;
    int startWidth, startHeight;
    bool isPause, isMusic, isSound, isVibration;

    IEnumerator FPS()
    {
        fps.text = "";
        while (true)
        {
            yield return new WaitForSeconds(1f);
            fps.text = "FPS: " + Mathf.RoundToInt(1f / Time.deltaTime);
        }
    }

    IEnumerator CountTime()
    {
        float saveTime = Time.time, time = 0;
        byte minute = 0, second = 0;
        while (true)
        {
            time = Time.time - saveTime;
            second = (byte)(time - minute * 60);
            if (second == 60)
            {
                minute++;
                PlayerPrefs.SetInt("TotalMinutes", PlayerPrefs.GetInt("TotalMinutes") + 1);
            }
            if (minute == 60)
                break;
            timer.text = minute.ToString("00") + ":" + second.ToString("00") + ":" + ((time - (int)time) * 1000).ToString("000");
            yield return null;
        }
        timer.color = new Color(1, 0, 0, timer.color.a);
    }

    IEnumerator FirstStepToLuckEnd()
    {
        yield return new WaitForSeconds(2f);
        Destroy(GameObject.Find("CanvasLoading"));
        Destroy(Camera.main.gameObject.GetComponent<Animation>());
        AudioListener.volume = 1;
        while (bg.color.a > 0.01f)
        {
            bg.color = Color.Lerp(bg.color, new Color(0, 0, 0, 0), Time.deltaTime * 5);
            if (bg.color.a < 0.5f)
                bg.raycastTarget = false;
            yield return null;
        }
        Destroy(bg.gameObject);
        joystick.gameObject.SetActive(true);
        run.SetActive(true);
        if (PlayerPrefs.GetInt("Difficulty") < 4)
            ghost.SetActive(true);
        StartCoroutine(CountTime());
    }

    IEnumerator FirstStepToLuck()
    {
        bg.gameObject.SetActive(true);
        joystick.gameObject.SetActive(false);
        run.SetActive(false);
        ghost.SetActive(false);
        AudioListener.volume = 0;
        yield return null;
        RenderSettings.fog = PlayerPrefs.GetInt("Fog") == 1;
        yield return new WaitForSeconds(0.5f);
        if (Screen.height > 900)
        {
            float width = Screen.width, height = Screen.height, maxFPS = 0, lastResolution = (PlayerPrefs.GetInt("Fog") == 1 ? 0.96f : 1f);
            for (int i = 0; i < 10; i++)
            {
                if (1f / Time.deltaTime > maxFPS)
                    maxFPS = 1f / Time.deltaTime;
                Screen.SetResolution((int)(width * 0.97f), (int)(height * 0.97f), true);
                yield return null;
            }
            if (maxFPS > 29f)
            {
                Screen.SetResolution((int)(width * lastResolution), (int)(height * lastResolution), true);
                StartCoroutine(FirstStepToLuckEnd());
            }
            else
            {
                while (maxFPS < 29f)
                {
                    maxFPS = 0;
                    for (int i = 0; i < 10; i++)
                    {
                        if (1f / Time.deltaTime > maxFPS)
                            maxFPS = 1f / Time.deltaTime;
                        yield return null;
                    }
                    width *= 0.97f;
                    height *= 0.97f;
                    Screen.SetResolution((int)width, (int)height, true);
                    yield return null;
                }
                Screen.SetResolution((int)(width * lastResolution), (int)(height * lastResolution), true);
                StartCoroutine(FirstStepToLuckEnd());
            }
        }
        else
            StartCoroutine(FirstStepToLuckEnd());
    }

    void Awake()
    {
        dropdown.value = PlayerPrefs.GetInt("Shooting");
        if (!PlayerPrefs.HasKey("Sensitivity"))
            PlayerPrefs.SetFloat("Sensitivity", sensitivity.value);             // изменение чувствительности от значения слайдера
        else
            sensitivity.value = PlayerPrefs.GetFloat("Sensitivity");            // изменение значения слайдера
        CameraRotation.sensitivity = sensitivity.value;                         // изменение чувствительности для вращения камеры
        isMusic = PlayerPrefs.GetInt("Music") == 1 ? false : true;              // инверсирование переменной для вызова Music(), где эта она снова инверсируется в правельное значение
        isSound = PlayerPrefs.GetInt("Sound") == 1 ? false : true;              // инверсирование переменной для вызова Sound(), где эта она снова инверсируется в правельное значение
        Music();                                                                // вызов метода для установки иконки
        Sound();                                                                // вызов метода для установки иконки
        isVibration = PlayerPrefs.GetInt("Vibration") == 1;
        vibration.sprite = sprites[isVibration ? 7 : 6];
        vibration.color = isVibration ? new Color(1, 1, 1, 0.9f) : new Color(1, 1, 1, 0.4f);
        if (PlayerPrefs.GetInt("FPS") == 1)
            StartCoroutine(FPS());
        else
            Destroy(fps.gameObject);
        startWidth = Screen.width;
        startHeight = Screen.height;
        StartCoroutine(FirstStepToLuck());
        Destroy(GameObject.Find("AudioMusic"));
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (bg == null && !screen.activeSelf)
        {
            isPause = hasFocus;
            if (!isPause)
                Pause();
        }
    }

    public void SetShootingMode()
    {
        PlayerPrefs.SetInt("Shooting", dropdown.value);
    }

    public void Pause()
    {
        isPause = !isPause;
        pause.sprite = sprites[isPause ? 1 : 0];
        panelSettings.localPosition = isPause ? new Vector3(0, 0, 0) : new Vector3(0, 1000, 0);
        Time.timeScale = isPause ? 0 : 1;
        joystick.enabled = !isPause;
        joystick.OnPointerUp(null);
        cameraRotation.enabled = !isPause;
        GameObject.Find("Hand").GetComponent<Button>().enabled = !isPause;
        centerPoint.enabled = !isPause;
        ban.enabled = !isPause;
        dot_1.enabled = !isPause;
        dot_2.enabled = !isPause;
        dot_3.enabled = !isPause;
        dot_4.enabled = !isPause;
        goToCar.enabled = !isPause;
        if (isSound)
            AudioListener.pause = isPause;
    }

    public void Sensitivity()
    {
        PlayerPrefs.SetFloat("Sensitivity", sensitivity.value);
        CameraRotation.sensitivity = sensitivity.value;
    }

    public void Music()
    {
        isMusic = !isMusic;
        music.sprite = sprites[isMusic ? 3 : 2];
        music.color = isMusic ? new Color(1, 1, 1, 0.9f) : new Color(1, 1, 1, 0.4f);
        PlayerPrefs.SetInt("Music", isMusic ? 1 : 0);
    }

    public void Sound()
    {
        isSound = !isSound;
        sound.sprite = sprites[isSound ? 5 : 4];
        sound.color = isSound ? new Color(1, 1, 1, 0.9f) : new Color(1, 1, 1, 0.4f);
        PlayerPrefs.SetInt("Sound", isSound ? 1 : 0);

        AudioListener.volume = isSound ? 1 : 0;
    }

    public void Vibration()
    {
        isVibration = !isVibration;
        vibration.sprite = sprites[isVibration ? 7 : 6];
        vibration.color = isVibration ? new Color(1, 1, 1, 0.9f) : new Color(1, 1, 1, 0.4f);
        if (isVibration)
            Handheld.Vibrate();
        PlayerPrefs.SetInt("Vibration", isVibration ? 1 : 0);
    }

    public void LoadScene(int index)
    {
        Time.timeScale = 1;
        Screen.SetResolution(startWidth, startHeight, true);
        if (index == 1)
            index = 2;
        PlayerPrefs.SetInt("Scene", index);
        SceneManager.LoadScene(index);
    }
}