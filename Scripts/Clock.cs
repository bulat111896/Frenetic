using UnityEngine;
using System.Collections;

public class Clock : MonoBehaviour
{
    public AudioSource chasy;
    public float speed1, speed2;
    Transform arrow1, arrow2, _arrow1, _arrow2;
    bool isCan;

    IEnumerator Wait()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            arrow1.rotation = _arrow1.rotation;
            arrow2.rotation = _arrow2.rotation;
        }
    }

    void Start()
    {
        arrow1 = transform.GetChild(0);
        arrow2 = transform.GetChild(1);
        _arrow1 = transform.GetChild(2);
        _arrow2 = transform.GetChild(3);
        StartCoroutine(Wait());
    }

    void Update()
    {
        _arrow1.Rotate(Vector3.forward * speed1 * Time.deltaTime);
        _arrow2.Rotate(Vector3.forward * speed2 * Time.deltaTime);
        if (_arrow2.transform.localRotation.z < 0)
        {
            if (isCan)
            {
                isCan = false;
                chasy.Play();
            }
        }
        else if (!isCan)
        {
            isCan = true;
            chasy.Play();
        }
    }
}