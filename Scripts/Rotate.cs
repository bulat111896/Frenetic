using UnityEngine;

public class Rotate : MonoBehaviour
{
    public AudioSource generator;
    public float speed;

    void Update()
    {
        if (generator == null || generator.loop)
            transform.Rotate(Vector3.forward * Time.deltaTime * speed);
    }
}