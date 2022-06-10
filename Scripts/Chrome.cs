using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Chrome : MonoBehaviour
{
    public GameObject error, text;
    public Transform loading;

    IEnumerator Rotate()
    {
        loading.rotation = Quaternion.identity;
        while (true)
        {
            yield return null;
            loading.Rotate(Vector3.forward * Time.deltaTime * 150);
        }
    }

    IEnumerator WaitLoading()
    {
        loading.gameObject.SetActive(true);
        error.SetActive(false);
        text.SetActive(false);
        yield return new WaitForSeconds(Random.Range(0.5f, 3f));
        loading.gameObject.SetActive(false);
        error.SetActive(true);
        text.SetActive(true);
    }

    public void StartLoading()
    {
        StopAllCoroutines();
        StartCoroutine(Rotate());
        StartCoroutine(WaitLoading());
    }

    void OnEnable()
    {
        StartLoading();
    }

    void Start()
    {
        if (PlayerPrefs.GetInt("Language") == 0)
            text.GetComponent<Text>().text = "No internet connection";
    }
}