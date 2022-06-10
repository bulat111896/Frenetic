using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Monetization;
using System.Collections;
using System;

public class PanelShop : MonoBehaviour
{
    public GameObject video, loading, debiting, text;
    public Button ads;
    public Text money, timer;
    public bool isPanel;

    public void SetPos(bool isOpen)
    {
        if (!isPanel && isOpen)
        {
            isPanel = true;
            GetComponent<RectTransform>().localPosition -= new Vector3(0, 1000, 0);
        }
        else if (isPanel && !isOpen)
        {
            isPanel = false;
            GetComponent<RectTransform>().localPosition += new Vector3(0, 1000, 0);
        }
    }

    void HandleShowResult(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + 2);
            money.text = PlayerPrefs.GetInt("Money").ToString();
            debiting.SetActive(false);
            debiting.SetActive(true);
            debiting.GetComponent<Text>().text = "+2";
            PlayerPrefs.SetInt("Views", PlayerPrefs.GetInt("Views") + 1);
            PlayerPrefs.SetString("Date", (DateTime.Now + new TimeSpan(0, 0, 300)).ToString());
        }
    }

    IEnumerator Timer()
    {
        TimeSpan ts = DateTime.Parse(PlayerPrefs.GetString("Date")) - DateTime.Now;
        while (ts.TotalSeconds > 0)
        {
            timer.text = ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
            ts = ts.Add(new TimeSpan(0, 0, -1));
            yield return new WaitForSeconds(1);
        }
        video.SetActive(true);
        timer.gameObject.SetActive(false);
        text.SetActive(true);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(15);
        video.SetActive(true);
        loading.SetActive(false);
        text.SetActive(true);
        StopAllCoroutines();
    }

    IEnumerator Ads()
    {
        while (true)
        {
            if (Monetization.IsReady("rewardedVideo"))
            {
                ShowAdCallbacks options = new ShowAdCallbacks();
                options.finishCallback = HandleShowResult;
                (Monetization.GetPlacementContent("rewardedVideo") as ShowAdPlacementContent).Show(options);
                video.SetActive(true);
                loading.SetActive(false);
                text.SetActive(true);
                StopAllCoroutines();
            }
            Monetization.Initialize("3668539", false);
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void PlayVideo()
    {
        if (PlayerPrefs.HasKey("Date") && PlayerPrefs.GetInt("Views") > 4)
        {
            video.SetActive(false);
            timer.gameObject.SetActive(true);
            text.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(Timer());
        }
        else
        {
            video.SetActive(false);
            text.SetActive(false);
            loading.SetActive(true);
            loading.transform.rotation = Quaternion.identity;
            StopAllCoroutines();
            StartCoroutine(Wait());
            StartCoroutine(Ads());
        }
    }

    void Start()
    {
        if (PlayerPrefs.GetInt("Ads") == 1)
            ads.interactable = false;
        if (PlayerPrefs.HasKey("Date") && (DateTime.Now - DateTime.Parse(PlayerPrefs.GetString("Date"))).TotalSeconds > 0)
        {
            PlayerPrefs.SetInt("Views", 0);
        }
    }

    void Update()
    {
        if (loading.activeSelf)
            loading.transform.Rotate(Vector3.forward * Time.deltaTime * 250f);
    }
}