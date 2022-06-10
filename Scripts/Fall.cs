using System.Collections;
using UnityEngine;

public class Fall : MonoBehaviour
{
    public Enemy enemy;
    bool isCan;

    IEnumerator FalseOnStart()
    {
        yield return new WaitForSeconds(4);
        isCan = true;
    }

    void Start()
    {
        StartCoroutine(FalseOnStart());
    }

    IEnumerator Play()
    {
        isCan = false;
        GetComponent<AudioSource>().Play();

        int mode = PlayerPrefs.GetInt("Difficulty");
        if (enemy != null)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if ((mode == 0 && distance < 12f) || (mode == 1 && distance < 20f) || mode > 1)
                enemy.SetDestination();
        }
        yield return new WaitForSeconds(0.7f);
        isCan = true;
    }

    void OnCollisionEnter(Collision other)
    {
        if (isCan && other.collider.tag != "Player" && GetComponent<Rigidbody>().velocity.magnitude > 0.1f)
        {
            StopAllCoroutines();
            StartCoroutine(Play());
        }
    }
}