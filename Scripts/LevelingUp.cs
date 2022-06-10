using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class LevelingUp : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public GameObject[] panelPush;
    public AudioSource _audio;
    public GameObject debiting;
    public Text money, ghostTime;
    public bool isSpeed;
    Image bg;

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.3f);
        StopAllCoroutines();
    }

    IEnumerator Anim(float alpha)
    {
        while (true)
        {
            bg.color = Color.Lerp(bg.color, new Color(1, 1, 1, alpha), Time.deltaTime * 25);
            yield return null;
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        StopAllCoroutines();
        StartCoroutine(Anim(0.35f));
        StartCoroutine(Wait());
    }

    public void OnPointerUp(PointerEventData data)
    {
        StopAllCoroutines();
        StartCoroutine(Anim(0));
        StartCoroutine(Wait());
    }

    public void SetGhostTimeText()
    {
        ghostTime.text = (PlayerPrefs.GetInt("Language") == 0 ? "Valid for " : "Действует в течение ") + (PlayerPrefs.GetInt("GhostTime") / 10f).ToString().Replace(',', PlayerPrefs.GetInt("Language") == 0 ? '.' : ',') + (PlayerPrefs.GetInt("Language") == 0 ? " seconds" : " секунд");
    }

    public void SetSpeedText()
    {
        char point = PlayerPrefs.GetInt("Language") == 0 ? '.' : ',';
        ghostTime.text = (PlayerPrefs.GetInt("Speed") / 10f).ToString().Replace(',', point) + (PlayerPrefs.GetInt("Speed") == 10 || PlayerPrefs.GetInt("Speed") == 20 ? point + "0" : "") + "X";
    }

    void Operation()
    {
        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") - 1);
        money.text = PlayerPrefs.GetInt("Money").ToString();
        debiting.SetActive(false);
        debiting.SetActive(true);
        debiting.GetComponent<Text>().text = "-1";
    }

    public void OnPointerClick(PointerEventData data)
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
            _audio.Play();
        if (PlayerPrefs.GetInt("Money") >= 1)
        {
            if (isSpeed && PlayerPrefs.GetInt("Speed") < 20)
            {
                PlayerPrefs.SetInt("Speed", PlayerPrefs.GetInt("Speed") + 1);
                SetSpeedText();
                Operation();
            }
            else if (!isSpeed && PlayerPrefs.GetInt("GhostTime") < 200)
            {
                PlayerPrefs.SetInt("GhostTime", PlayerPrefs.GetInt("GhostTime") + 1);
                SetGhostTimeText();
                Operation();
            }
        }
        else
        {
            panelPush[0].SetActive(false);
            panelPush[1].SetActive(false);
            panelPush[1].SetActive(true);
        }
    }

    void Start()
    {
        bg = transform.GetChild(1).GetComponent<Image>();
    }
}