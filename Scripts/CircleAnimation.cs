using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class CircleAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public AudioSource _audio, _audio_music;
    public Sprite[] sprites;
    Transform[] objects = new Transform[4];
    Image sprite, circle;
    bool isMusicSound;

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
        isMusicSound = !isMusicSound;
        sprite.sprite = sprites[isMusicSound ? 1 : 0];
        PlayerPrefs.SetInt(gameObject.name, isMusicSound ? 1 : 0);
        if (gameObject.name == "Music")
        {
            if (isMusicSound)
                _audio_music.Play();
            else
                _audio_music.Pause();
        }
        if (PlayerPrefs.GetInt("Sound") == 1)
            _audio.Play();
    }

    void Start()
    {
        isMusicSound = PlayerPrefs.GetInt(gameObject.name) == 1 ? true : false;
        for (int i = 0; i < 4; i++)
            objects[i] = transform.GetChild(i);
        sprite = transform.GetChild(4).GetComponent<Image>();
        circle = transform.GetChild(5).GetComponent<Image>();
        sprite.sprite = sprites[isMusicSound ? 1 : 0];
    }

    void Update()
    {
        for (int i = 0; i < 4; i++)
            objects[i].Rotate(Vector3.forward * Time.deltaTime * 50 * (i % 2 == 0 ? 1 : -1));
    }
}