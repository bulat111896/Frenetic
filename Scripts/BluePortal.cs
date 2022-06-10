using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BluePortal : MonoBehaviour
{
    public Transform player;
    public Text record, timer, bat;
    float saveScale, x, y, rot;
    bool isStart;

    IEnumerator AnimBG()
    {
        if (GameObject.Find("Enemy") != null)
            Destroy(GameObject.Find("Enemy"));
        Destroy(GameObject.Find("Canvas"));
        Destroy(GameObject.Find("GameController"));
        Destroy(GameObject.Find("Heartbeat"));
        record.transform.parent.parent.gameObject.SetActive(true);
        PlayerPrefs.SetString("Time", timer.text);
        record.text = timer.text;
        PlayerPrefs.SetInt("Wins", PlayerPrefs.GetInt("Wins") + 1);
        switch (PlayerPrefs.GetInt("Difficulty"))
        {
            case 0:
                PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + 2);
                bat.text = "+2";
                break;
            case 1:
                PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + 3);
                bat.text = "+3";
                break;
            case 2:
                PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + 5);
                bat.text = "+5";
                PlayerPrefs.SetInt("HardMode", 1);
                break;
            case 3:
                PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + 10);
                bat.text = "+10";
                break;
            case 4:
                PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + 1);
                bat.text = "+1";
                break;
        }
        yield return new WaitForSeconds(1);
        Destroy(GameObject.Find("Player").GetComponent<CharacterController>());
        Destroy(GameObject.Find("Player").GetComponent<Player>());
        Destroy(GameObject.Find("Player").GetComponent<Rigidbody>());
        Destroy(GameObject.Find("House"));
        Destroy(GameObject.Find("Interactive"));
        yield return new WaitForSeconds(5);
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);
    }

    IEnumerator Wait3()
    {
        Light _light = transform.GetChild(0).GetComponent<Light>();
        _light.intensity = 1;
        while (_light.intensity < 500)
        {
            _light.intensity += Time.deltaTime * 5 * _light.intensity;
            yield return null;
        }
        _light.intensity = 500;
    }

    IEnumerator Wait2()
    {
        while (true)
        {
            rot = Random.Range(-0.1f, 0.2f);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator Wait()
    {
        while (true)
        {
            x = Random.Range(saveScale * 0.9f, saveScale * 1.1f);
            y = Random.Range(saveScale * 0.9f, saveScale * 1.1f);
            yield return new WaitForSeconds(0.3f);
        }
    }

    void Start()
    {
        saveScale = transform.localScale.x;
        transform.localScale = Vector3.zero;
        StartCoroutine(Wait());
        StartCoroutine(Wait2());
        StartCoroutine(Wait3());
    }

    void Update()
    {
        transform.localScale = new Vector3(Mathf.Lerp(transform.localScale.x, x, Time.deltaTime * 3f), Mathf.Lerp(transform.localScale.y, y, Time.deltaTime * 3f), 1);
        transform.Rotate(Vector3.forward * rot);
        if (!isStart && Vector3.Distance(player.position, transform.position) < 0.8f)
        {
            isStart = true;
            StartCoroutine(AnimBG());
        }
    }
}
