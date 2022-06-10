using UnityEngine;
using System.Collections;

public class Heartbeat : MonoBehaviour
{
    public RectTransform heart;
    AudioSource _audio;
    public float waitingTime;
    float increase = 0.4f, calmingDown = 1.4f, size = 100;
    bool isFright;

    public void OnEnemySee()
    {
        if (!isFright)
        {
            isFright = true;
            StartCoroutine("Increase");
        }
    }

    public void Run()
    {
        StartCoroutine(RunWait());
    }

    public void OnEnemyLostPlayer()
    {
        isFright = false;
        StopCoroutine("Increase");
        StopCoroutine("CalmingDown");
        StartCoroutine("CalmingDown");
    }

    IEnumerator RunWait()
    {
        OnEnemySee();
        yield return new WaitForSeconds(2.5f);
        OnEnemyLostPlayer();
    }

    IEnumerator Increase()
    {
        while (true)
        {
            waitingTime = Mathf.Lerp(waitingTime, increase, 0.6f);
            if (waitingTime < increase + 0.01f)
                break;
            yield return new WaitForSeconds(0.1f);
        }
        waitingTime = increase;
    }

    IEnumerator CalmingDown()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            waitingTime = Mathf.Lerp(waitingTime, calmingDown, 0.1f);
            if (waitingTime > calmingDown - 0.01f)
                break;
        }
        waitingTime = calmingDown;
    }

    IEnumerator AnimHeart()
    {
        while(true)
        {
            size += Time.deltaTime * 70;
            if (size > 95)
                break;
            heart.sizeDelta = new Vector2(size, size);
            yield return null;
        }
        while (true)
        {
            size -= Time.deltaTime * 70;
            if (size < 85)
                break;
            heart.sizeDelta = new Vector2(size, size);
            yield return null;
        }
    }

    IEnumerator Heart()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitingTime);
            _audio.pitch = Random.Range(0.9f, 1.1f);
            _audio.Play();
            StopCoroutine("AnimHeart");
            StartCoroutine("AnimHeart");
        }
    }

    void Start()
    {
        waitingTime = calmingDown;
        _audio = GetComponent<AudioSource>();
        StartCoroutine(Heart());
    }
}