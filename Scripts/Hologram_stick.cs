using UnityEngine;
using System.Collections;

public class Hologram_stick : MonoBehaviour
{
    float maxHeight, speed;

    IEnumerator Wait()
    {
        while (true)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(transform.localScale.x, Mathf.PingPong(speed * Time.time, maxHeight), transform.localScale.z), speed);
            yield return new WaitForSeconds(0.05f);
        }
    }

    void Start()
    {
        maxHeight = Random.Range(0.05f, 0.1f);
        speed = Random.Range(0.05f, 0.2f);
        StartCoroutine(Wait());
    }
}