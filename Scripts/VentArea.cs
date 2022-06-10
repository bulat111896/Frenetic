using System.Collections;
using UnityEngine;

public class VentArea : MonoBehaviour
{
    public bool isCan;
    public Transform player;
    public GameObject goParkour, joystick, run, ghost;

    IEnumerator Wait()
    {
        isCan = false;
        joystick.SetActive(false);
        run.SetActive(false);
        ghost.SetActive(false);
        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<SphereCollider>().enabled = false;
        if (gameObject.name == "Passage_2")
            player.GetComponent<Animation>().Play(transform.position.x - player.position.x > 0 ? "Parkour2_" : "Parkour_");
        else
            player.GetComponent<Animation>().Play(transform.position.z - player.position.z > 0 ? "Parkour2" : "Parkour");
        while (player.GetComponent<Animation>().isPlaying)
        {
            yield return new WaitForSeconds(0.2f);
        }
        if (joystick.transform.parent.gameObject.activeSelf)                           // провеерка на убийство врагом
        {
            joystick.SetActive(true);
            run.SetActive(true);
            ghost.SetActive(true);
            player.GetComponent<CharacterController>().enabled = true;
            player.GetComponent<SphereCollider>().enabled = true;
            yield return new WaitForSeconds(1);
            isCan = true;
        }
    }

    public void Click()
    {
        if (Vector3.Distance(transform.position, player.position) < 5f)
        {
            StopAllCoroutines();
            StartCoroutine(Wait());
        }
    }

    void Update()
    {
        // не упрощать
        if (Vector3.Distance(transform.position, player.position) < 5f)
        {
            if (isCan && Vector3.Distance(transform.position, player.position) < 1.2f)
                goParkour.SetActive(true);
            else
                goParkour.SetActive(false);
        }
    }
}