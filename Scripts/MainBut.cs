using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MainBut : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public AudioSource _audio;
    public MainBut settings;
    public RectTransform panelSettings;
    Image image;
    public bool isPanel;

    IEnumerator Anim(float range)
    {
        while (image.pixelsPerUnitMultiplier != range)
        {
            image.pixelsPerUnitMultiplier = Mathf.Lerp(image.pixelsPerUnitMultiplier, range, Time.deltaTime * 20);
            image.SetVerticesDirty();
            image.color = new Color(1, 1, 1, Mathf.Lerp(image.color.a, range == 10 ? 1 : 0.9f, Time.deltaTime * 20));
            transform.localScale = Vector3.Lerp(transform.localScale, range == 10 ? new Vector3(0.97f, 0.97f, 1) : Vector3.one, Time.deltaTime * 20);
            yield return null;
        }
    }

    public void PanelSettings(bool isOpen)
    {
        if (!isPanel && isOpen)
        {
            isPanel = true;
            panelSettings.localPosition -= new Vector3(0, 1000, 0);
        }
        else if (isPanel && !isOpen)
        {
            isPanel = false;
            panelSettings.localPosition += new Vector3(0, 1000, 0);
        }
        if (isOpen)
            panelSettings.GetComponent<Animation>().Play();
    }

    public void Exit()
    {
        Application.Quit();
    }

    void Start()
    {
        image = GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData data)
    {
        StopAllCoroutines();
        StartCoroutine(Anim(10));
    }

    public void OnPointerUp(PointerEventData data)
    {
        StopAllCoroutines();
        StartCoroutine(Anim(15));
    }

    public void OnPointerClick(PointerEventData data)
    {
        switch (gameObject.name)
        {
            case "Play":
                PlayerPrefs.SetInt("Scene", 2);
                UnityEngine.SceneManagement.SceneManager.LoadScene(1);
                break;
            case "Shop":
                settings.PanelSettings(false);
                panelSettings.GetComponent<PanelShop>().SetPos(true);
                break;
            case "Settings":
                PanelSettings(true);
                break;
        }
        if (PlayerPrefs.GetInt("Sound") == 1)
            _audio.Play();
    }
}