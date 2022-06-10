using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GunPhisics : MonoBehaviour
{
    public GameObject ban, aimDot, centerPoint, gunBut;
    public Transform cam;
    public static bool isCan;
    bool isEnter, isTrevoga;

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
        StopAllCoroutines();
    }

    public void StartEnd()
    {
        StartCoroutine(End());
    }

    IEnumerator End()
    {
        while (!isCan)
            yield return null;
        isEnter = false;
        if (transform.GetChild(0).gameObject.activeSelf)
        {
            ban.SetActive(false);
            if (!Fire.isAim)
            {
                centerPoint.SetActive(true);
                aimDot.SetActive(true);
            }
            StartCoroutine(Wait());
            while (true)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0.18f, -0.18f, 0.6f), 10 * Time.deltaTime);
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 0, 0), 10 * Time.deltaTime);
                yield return null;
            }
        }
        else
        {
            transform.localPosition = new Vector3(0.18f, -0.18f, 0.6f);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    IEnumerator Enter()
    {
        while (true)
        {
            if (gunBut != null && gunBut.activeSelf && gunBut.transform.parent.GetComponent<Button>().interactable)
            {
                isEnter = true;
                if (transform.GetChild(0).gameObject.activeSelf)
                {
                    ban.SetActive(true);
                    centerPoint.SetActive(false);
                    aimDot.SetActive(false);
                }
                GetComponent<Animator>().enabled = false;  
                StopAllCoroutines();
            }
            yield return null;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" && other.tag != "InteractivePhysics")
            StartCoroutine(Enter());
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag != "Player" && other.tag != "InteractivePhysics")
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-0.05f, -0.25f, 0.3f), 5 * Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(15, -70, 0), 5 * Time.deltaTime);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player" && other.tag != "InteractivePhysics")
            StartCoroutine(End());
    }

    public void Disable()
    {
        ban.SetActive(false);
        centerPoint.SetActive(true);
        aimDot.SetActive(false);
        if (!isEnter)
            StartCoroutine(End());
    }

    public void _Enter()
    {
        if (isEnter)
            StartCoroutine(Enter());
    }

    void Update()
    {
        if (transform.GetChild(0).gameObject.activeSelf)
        {
            if (cam.eulerAngles.x < 300 && cam.eulerAngles.x > 100)
            {
                if (!isEnter)
                    ban.SetActive(true);
                if (!Fire.isAim)
                {
                    centerPoint.SetActive(false);
                    aimDot.SetActive(false);
                }
            }
            else if (!isEnter)
            {
                ban.SetActive(false);
                if (!Fire.isAim)
                {
                    centerPoint.SetActive(true);
                    aimDot.SetActive(true);
                }
            }
        }
        if (isCan && isEnter)
        {
            if (isTrevoga)
            {
                StartCoroutine(End());
                isTrevoga = false;
            }
            else
                isTrevoga = true;
        }
        else
            isTrevoga = false;
    }

    void Awake()
    {
        isCan = false;
    }
}