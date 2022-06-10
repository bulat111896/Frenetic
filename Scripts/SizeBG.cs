using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Monetization;
using System.Collections;

public class SizeBG : MonoBehaviour
{
    public GameObject[] EscapeObjs;
    public GameObject panelExit;
    public AudioSource _audio, _audio_music;
    public AudioClip close;
    public Text money;
    RectTransform rectCanvas;
    Vector2 save;

    public void ShowLeaderboard()
    {
        PlayGames._instance.ShowLeaderboard();
    }

    public void ShowAchievements()
    {
        PlayGames._instance.ShowAchievements();
    }

    IEnumerator Ads()
    {
        while (true)
        {
            if (Monetization.IsReady("video"))
            {
                (Monetization.GetPlacementContent("video") as ShowAdPlacementContent).Show();
                yield break;
            }
            Monetization.Initialize("3668539", false);
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void OpenURL()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.funnycloudgames.frenetic");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void PlayClick()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
            _audio.Play();
    }

    public void PlaySound()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
            _audio.PlayOneShot(close);
    }

    void SetSize()
    {
        GetComponent<RectTransform>().sizeDelta = (float)Screen.width / Screen.height < 1920f / 1080f ? new Vector2(rectCanvas.sizeDelta.y * (1920f / 1080f), rectCanvas.sizeDelta.y) : new Vector2(rectCanvas.sizeDelta.x, rectCanvas.sizeDelta.x / (1920f / 1080f));
    }

    static float GetSeconds(string time)
    {
        int.TryParse(time.Split(':')[0], out int minutes);
        int.TryParse(time.Split(':')[1], out int seconds);
        int.TryParse(time.Split(':')[2], out int milliseconds);
        return minutes * 60f + seconds + milliseconds / 1000f;
    }

    void Awake()
    {
        if (PlayerPrefs.GetInt("Money") > 999999)
            Application.Quit();
        AudioListener.pause = false;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        rectCanvas = transform.parent.GetComponent<RectTransform>();
        if (!PlayerPrefs.HasKey("Music"))
            PlayerPrefs.SetInt("Music", 1);
        if (!PlayerPrefs.HasKey("Sound"))
            PlayerPrefs.SetInt("Sound", 1);
        if (!PlayerPrefs.HasKey("GhostTime"))
            PlayerPrefs.SetInt("GhostTime", 30);
        if (!PlayerPrefs.HasKey("Speed"))
            PlayerPrefs.SetInt("Speed", 10);
        if (PlayerPrefs.GetInt("Music") == 0)
            _audio_music.Stop();
        Monetization.Initialize("3668539", false);
        if (PlayerPrefs.HasKey("Time"))
        {
            if (!PlayerPrefs.HasKey("BestTime") || GetSeconds(PlayerPrefs.GetString("BestTime")) > GetSeconds(PlayerPrefs.GetString("Time")))
                PlayerPrefs.SetString("BestTime", PlayerPrefs.GetString("Time"));
            PlayerPrefs.DeleteKey("Time");

            if (!PlayerPrefs.HasKey("Ads"))
                StartCoroutine(Ads());
        }
        money.text = PlayerPrefs.GetInt("Money").ToString();
        if (GameObject.Find("CanvasLoading") != null)
            Destroy(GameObject.Find("CanvasLoading"));
        DontDestroyOnLoad(_audio_music.gameObject);
        if (PlayerPrefs.HasKey("BestTime") && PlayerPrefs.GetString("SetLeaderboard") != PlayerPrefs.GetString("BestTime"))
            PlayGames._instance.AddScoreToLeaderboard((int)GetSeconds(PlayerPrefs.GetString("BestTime")) * 1000);
    }

    void Update()
    {
        if (rectCanvas.sizeDelta != save)
        {
            SetSize();
            save = rectCanvas.sizeDelta;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (EscapeObjs[0].GetComponent<MainBut>().isPanel || EscapeObjs[1].GetComponent<PanelShop>().isPanel || EscapeObjs[2].GetComponent<PanelAchievement>().isPanel)
            {
                EscapeObjs[0].GetComponent<MainBut>().PanelSettings(false);
                EscapeObjs[1].GetComponent<PanelShop>().SetPos(false);
                EscapeObjs[2].GetComponent<PanelAchievement>().SetPos(false);
            }
            else
                panelExit.SetActive(true);
        }
    }
}