using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Load : MonoBehaviour
{
    public GameObject[] squares;

    IEnumerator Start()
    {
        DontDestroyOnLoad(gameObject);
        for (int i = 0; i < 3; i++)
            squares[i].SetActive(false);
        SceneManager.LoadSceneAsync(PlayerPrefs.GetInt("Scene"));
        PlayerPrefs.DeleteKey("Scene");
        for (int i = 0; i < 3; i++)
        {
            squares[i].SetActive(true);
            yield return new WaitForSeconds(0.65f);
        }
    }
}