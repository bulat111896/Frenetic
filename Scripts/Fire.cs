using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Fire : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public Enemy enemy;
    public AudioSource _audio;
    public AudioClip reloadingAudio, misfireAudio, fireAudio, bulletAudio, zoomAudio, radioDestroy;
    public Texture2D[] holeSprites, hole_2_Sprites;
    public GameObject Hole, aimDot, centerPoint, ban, flare, glow;
    public Transform cam, player;
    public Button reloading, aim, gunBut;
    public Image circle, gunCol;
    public Text count;
    public Animator gun;
    public ParticleSystem smoke;
    public GameController gameController;
    public static bool isAim;
    public bool isPistol;
    float startFieldOfView;
    int magazine, bullets = 14;
    bool isStartToReloading;

    public void AddBullets()
    {
        bullets += Random.Range(3, 6);
        if (!isPistol)
            Count(false);
    }

    public void Count(bool isSetColor)
    {
        if (isSetColor)
        {
            if (!isStartToReloading)
            {
                Reloading();
                isStartToReloading = true;
            }
            count.color = new Color(1, 1, 1, 0.4f);
            gunCol.color = new Color(1, 1, 1, 0.4f);
        }
        count.text = magazine + "/<size='200'>" + bullets + "</size>";
        if (bullets == 0 && magazine == 0)
        {
            count.color = new Color(1, 0, 0, 0.5f);
            gunCol.color = new Color(1, 0, 0, 0.5f);
        }
        else
        {
            count.color = new Color(1, 1, 1, 0.8f);
            gunCol.color = new Color(1, 1, 1, 0.8f);
        }
    }

    IEnumerator CamAim()
    {
        _audio.PlayOneShot(zoomAudio);
        aimDot.SetActive(!isAim);
        centerPoint.SetActive(!isAim);
        while ((isAim && Camera.main.fieldOfView > 40.01f) || (!isAim && Camera.main.fieldOfView < 59.99f))
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, isAim ? (startFieldOfView * 0.7f) : startFieldOfView, 10 * Time.deltaTime);
            yield return null;
        }
        Camera.main.fieldOfView = isAim ? 40 : 60;
    }

    IEnumerator ReloadingAudioWait()
    {
        yield return new WaitForSeconds(0.15f);
        _audio.PlayOneShot(reloadingAudio);
    }

    IEnumerator ReloadingWait()
    {
        StartCoroutine(ReloadingAudioWait());

        gunBut.interactable = false;
        gun.enabled = true;
        aim.interactable = false;
        gun.Play("Reloading", -1, 0);
        reloading.interactable = false;
        circle.enabled = true;
        while (circle.fillAmount < 1)
        {
            circle.fillAmount += 1.4f * Time.deltaTime;
            yield return null;
        }
        bullets -= 1;
        magazine = 1;
        Count(false);
        yield return new WaitForSeconds(0.1f);
        reloading.interactable = true;
        aim.interactable = true;
        circle.enabled = false;
        circle.fillAmount = 0;
        gunBut.interactable = true;
        gun.GetComponent<GunPhisics>().StartEnd();
    }

    public void Aim()
    {
        if (!isPistol && !ban.activeSelf)
        {
            isAim = !isAim;
            gun.transform.localPosition = new Vector3(gun.transform.localPosition.x, gun.transform.localPosition.y, isAim ? 0.525f : 0.6f);
            StartCoroutine(CamAim());
        }
    }

    public void Reloading()
    {
        if (!isPistol && magazine == 0 && bullets > 0 && !ban.activeSelf)
            StartCoroutine(ReloadingWait());
    }

    IEnumerator Recoil()
    {
        float i = 0, camRot = Random.Range(-30f, -100f), playerRot = Random.Range(-20f, 20f);
        while (i < 0.1f)
        {
            yield return null;
            cam.Rotate(new Vector3(camRot * Time.deltaTime, 0, 0));
            player.Rotate(new Vector3(0, playerRot * Time.deltaTime, 0));
            i += Time.deltaTime;
        }
    }

    IEnumerator Wait()
    {
        _audio.PlayOneShot(fireAudio);
        yield return new WaitForSeconds(0.3f);
        gunBut.interactable = true;
        enemy.SetDestination();
    }

    IEnumerator FireAudioWait()
    {
        flare.SetActive(true);
        glow.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        flare.SetActive(false);
        glow.SetActive(false);
        _audio.PlayOneShot(bulletAudio);
    }

    IEnumerator Smoke()
    {
        yield return new WaitForSeconds(0.1f);
        smoke.Play();
    }

    IEnumerator Hit()
    {
        RaycastHit hit;
        bool isHit = false;
        for (int i = 0; i < 8; i++)
        {
            if (Physics.Raycast(cam.position, cam.forward + cam.TransformDirection(new Vector3(Random.Range(-0.04f, 0.04f), Random.Range(-0.04f, 0.04f), 0) * (isAim ? 0.5f : 1)), out hit, Mathf.Infinity))
            {
                if (hit.collider.name != "Head")
                {
                    if (hit.collider.tag == "InteractivePhysics")
                    {
                        isHit = true;
                        Rigidbody rb;
                        if (hit.collider.name != "MeeseeksBox")
                            rb = hit.collider.GetComponent<Rigidbody>();
                        else
                            rb = hit.collider.transform.parent.GetComponent<Rigidbody>();
                        rb.AddForce((hit.point - player.position).normalized * Random.Range(0f, 3f) + Vector3.up * Random.Range(0f, 1f), ForceMode.Impulse);
                    }
                    else
                    {
                        Transform hole = Instantiate(Hole, hit.point - cam.forward * 0.01f, Quaternion.FromToRotation(Vector3.back, hit.normal)).transform;
                        hole.GetComponent<Renderer>().material.mainTexture = hit.collider.tag == "Hole_2" ? hole_2_Sprites[i % 4] : holeSprites[i % 4];
                        if (hit.collider.name == "BigDoor" || hit.collider.name == "propeller")
                        {
                            hole.parent = hit.collider.transform;
                        }
                        else
                            hole.gameObject.isStatic = true;
                        Destroy(hole.gameObject, 10f);
                    }

                    if (!isHit && hit.collider.name == "Radio")
                    {
                        isHit = true;
                        if (!gameController.isRadioDestroy)
                        {
                            gameController.isRadioDestroy = true;
                            hit.collider.gameObject.layer = LayerMask.NameToLayer("Default");
                            hit.collider.transform.GetChild(1).gameObject.SetActive(true);
                            hit.collider.transform.GetChild(1).GetComponent<Animation>().Play();
                            Destroy(hit.collider.transform.GetChild(0).gameObject);
                            AudioSource radio = hit.collider.GetComponent<AudioSource>();
                            radio.Stop();
                            radio.clip = radioDestroy;
                            radio.volume = 1;
                            radio.loop = false;
                            radio.Play();
                            if (PlayerPrefs.GetInt("Vibration") == 1)
                                Handheld.Vibrate();
                        }
                    }
                }
                else if (!isHit)
                {
                    isHit = true;
                    yield return null;
                    Transform particles = enemy.transform.GetChild(2);
                    particles.GetComponent<ParticleSystem>().Play();
                    Destroy(particles.gameObject, 3f);
                    particles.parent = null;
                    yield return new WaitForSeconds(0.07f);
                    enemy.GetComponent<Animator>().SetBool("Death", true);
                    Destroy(enemy.GetComponent<Enemy>());
                    Destroy(enemy.GetComponent<UnityEngine.AI.NavMeshAgent>());
                    Destroy(enemy.gameObject, 0.33f);
                    PlayerPrefs.SetInt("Kills", PlayerPrefs.GetInt("Kills") + 1);
                    //PlayGames._instance.UnlockAchievement(GPS.achievement_killer, "Kills");
                }
            }
            yield return new WaitForSeconds(0.02f);
        }
    }

    void DoFire()
    {
        if (!isPistol && magazine == 1 && !ban.activeSelf)
        {
            magazine = 0;
            Count(false);
            gun.enabled = true;
            gunBut.interactable = false;
            gun.Play("Fire");
            StartCoroutine(Wait());
            StartCoroutine(Recoil());
            StartCoroutine(FireAudioWait());
            StartCoroutine(Smoke());
            StartCoroutine(Hit());
        }
        else if (!ban.activeSelf)
            _audio.PlayOneShot(misfireAudio);
    }

    public void OnPointerDown(PointerEventData data)
    {
        GetComponent<Image>().color = new Color(1, 1, 1, 0.2f);
        if (PlayerPrefs.GetInt("Shooting") == 0)
            DoFire();
    }

    public void OnPointerUp(PointerEventData data)
    {
        GetComponent<Image>().color = new Color(0, 0, 0, 0.2f);
    }

    public void OnPointerClick(PointerEventData data)
    {
        if (PlayerPrefs.GetInt("Shooting") == 1)
            DoFire();
    }

    public void CountPistol()
    {
        if (isPistol)
        {
            count.text = "0/<size='200'>0</size>";
            count.color = new Color(1, 0, 0, 0.5f);
            gunCol.color = new Color(1, 0, 0, 0.5f);
        }
    }

    void Start()
    {
        isAim = false;
        startFieldOfView = Camera.main.fieldOfView;
    }
}