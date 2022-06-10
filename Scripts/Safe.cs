using System.Collections;
using UnityEngine;

public class Safe : MonoBehaviour
{
    public AudioClip[] snaps;
    public AudioClip openSound;
    public GameController gameController;
    public Transform _lock;

    int stage;

    IEnumerator Snap()
    {
        int random = Random.Range(0, snaps.Length);
        GetComponent<AudioSource>().clip = snaps[random];
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(snaps[random].length);
        stage++;
        if (stage == 3)
        {
            gameController.isCanOpenSafe = true;
            _lock.gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    IEnumerator Wait()
    {
        int newStage = stage;
        while (stage == newStage && gameController.hand.enabled)
        {
            _lock.Rotate(Vector3.forward * ((stage == 1) ? -1 : 1) * Time.deltaTime * 30);
            yield return null;
        }
        GetComponent<AudioSource>().Stop();
        StopAllCoroutines();
    }

    public void Open()
    {
        GetComponent<AudioSource>().clip = openSound;
        GetComponent<AudioSource>().Play();
    }

    public void Lock()
    {
        if (!gameController.isCanOpenSafe)
        {
            StopAllCoroutines();
            StartCoroutine(Wait());
            StartCoroutine(Snap());
        }
    }
}
