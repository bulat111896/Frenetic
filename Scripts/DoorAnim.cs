using System.Collections;
using UnityEngine;

public class DoorAnim : MonoBehaviour
{
    Transform left, right, top;

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
        StopAllCoroutines();
    }

    IEnumerator Open()
    {
        transform.GetChild(4).GetComponent<ParticleSystem>().Play();
        transform.GetChild(5).GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(1);
        GetComponent<AudioSource>().Play();
        StartCoroutine(Wait());
        while (true)
        {
            left.localPosition = Vector3.MoveTowards(left.localPosition, new Vector3(1.1f, 0, 0), Time.deltaTime * 0.5f);
            right.localPosition = Vector3.MoveTowards(right.localPosition, new Vector3(-1, 0, 0), Time.deltaTime * 0.5f);
            top.localPosition = Vector3.MoveTowards(top.localPosition, new Vector3(0, 1.55f, 0), Time.deltaTime * 0.7f);
            yield return null;
        }
    }

    public void StartOpen()
    {
        StartCoroutine(Open());
    }

    void Start()
    {
        left = transform.GetChild(1);
        right = transform.GetChild(2);
        top = transform.GetChild(3);
    }
}