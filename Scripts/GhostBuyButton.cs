using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class GhostBuyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public GameObject[] panelPush;
    public AudioSource _audio;
    public GameObject debiting;
    public Text money, numberGhost;
    Image circle;

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.3f);
        StopAllCoroutines();
    }

    IEnumerator Anim(float alpha)
    {
        while (true)
        {
            circle.color = Color.Lerp(circle.color, new Color(1, 1, 1, alpha), Time.deltaTime * 25);
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

    public void OnPointerClick(PointerEventData data)
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
            _audio.Play();
        if (PlayerPrefs.GetInt("Money") >= 2)
        {
            if (PlayerPrefs.GetInt("Ghost") < 99)
            {
                PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") - 2);
                PlayerPrefs.SetInt("Ghost", PlayerPrefs.GetInt("Ghost") + 1);
                money.text = PlayerPrefs.GetInt("Money").ToString();
                numberGhost.text = PlayerPrefs.GetInt("Ghost").ToString();
                debiting.SetActive(false);
                debiting.SetActive(true);
                debiting.GetComponent<Text>().text = "-2";
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
        numberGhost.text = PlayerPrefs.GetInt("Ghost").ToString();
        circle = transform.GetChild(1).GetComponent<Image>();
    }
}
