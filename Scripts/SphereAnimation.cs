using System.Collections;
using UnityEngine;

public class SphereAnimation : MonoBehaviour
{
    float desiredScale, rotate, number, speed;
    bool isBig;

    void Awake()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        while (true)
        {
            speed = Random.Range(5f, 15f);
            desiredScale = isBig ? 1.2f : 0.8f;
            isBig = !isBig;
            number = Random.Range(-7f, 7f);
            yield return new WaitForSeconds(0.5f);
        }
    }

    void Update()
    {
        rotate = Mathf.Lerp(rotate, number, speed * Time.deltaTime);
        transform.Rotate(new Vector3(rotate, desiredScale * number, rotate * desiredScale) * Time.deltaTime);
        transform.localScale = Vector3.Lerp(transform.localScale, desiredScale * Vector3.one, 0.5f * Time.deltaTime);
    }
}