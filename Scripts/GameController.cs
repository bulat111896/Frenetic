using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{
    public Material sky;
    public Enemy enemy;
    public Safe safe;
    public Fire fire;
    public GameObject lantern, screen, pistolToDestroy, shotgunToDestroy, setActiveObjects;
    public Transform[] interactiveThing, spawnPoint, portalNULL;
    public Transform cam, gun, Null, Interactive, buttons, notebook, circleTable, gunUI, panelGenerator, BluePortal;
    public Image hand, inventory, fill, squareCenterPoint, run, ghost;
    public Text progress, info, code, numberGhost;
    public Slider gasolineSlider;
    public Sprite[] guns;
    public CharacterController player;
    public Animation[] door;
    public Animation chest, sink;
    public AudioClip noise, doorClose, doorOpen, doorClose2, doorOpen2, write, valve, engine, ignition, error, _true, chest_open, chest_open_2, chest_close, gasolineNotFull, gasolineFull, bigDoor_open, bigDoor_close, radioDestroy, hitAudio, booste, selection;
    public AudioSource radio, cupboard, generator, forest;
    public TextMesh lockText;
    public Material[] butMat;
    public DoorAnim doorAnim;
    public ParticleSystem spray;
    public AnimationClip bigDoor_Open, bigDoor_Close, bigDoor_Open_invert, bigDoor_Close_invert;
    public Rigidbody Rahmen;
    RaycastHit hit, hit2;
    Vector3 playerSavePos;
    float gasoline;
    public int ActiveThing = -1;
    int[] randomNumberCode = new int[4];
    int gasolineStick = 1;
    bool[] isOpenDoor = new bool[5];
    public static bool isChestOpen, isRun, isGhost;
    public bool isRadioDestroy, isChestOpened, isGasolineFull, isCanOpenSafe;
    bool isOpenLock, isLastFrame, isPadlockOpen, isRadioEnabled;

    IEnumerator PlayerMove()
    {
        Vector3 playerPos = player.transform.position;
        while (!Joystick.isClick && Vector3.Distance(player.transform.position, playerPos + new Vector3(0, 0.285f, -0.12f)) > 0.02f)
        {
            player.transform.position = Vector3.Lerp(player.transform.position, playerPos + new Vector3(0, 0.285f, -0.12f), Time.deltaTime * 10);
            yield return null;
        }
    }

    IEnumerator Ghost()
    {
        isGhost = true;
        ghost.GetComponent<Outline>().enabled = true;
        PlayerPrefs.SetInt("Ghost", PlayerPrefs.GetInt("Ghost") - 1);
        numberGhost.text = PlayerPrefs.GetInt("Ghost").ToString();
        if (enemy.isSaw)
        {
            enemy.StopCoroutine("Chase");
            enemy.StartCoroutine("SearchPlayer");
        }
        float ghostTime = 1f / PlayerPrefs.GetInt("GhostTime") * 10f;
        while (ghost.fillAmount > 0f)
        {
            yield return null;
            ghost.fillAmount -= ghostTime * Time.deltaTime;
        }
        isGhost = false;
        ghost.GetComponent<Outline>().enabled = false;
        while (ghost.fillAmount < 1f)
        {
            yield return null;
            ghost.fillAmount += 1.5f * Time.deltaTime;
        }
        ghost.fillAmount = 1;
        if (numberGhost.text != "0")
            ghost.transform.parent.GetComponent<Button>().interactable = true;
    }

    public void OnGhostClick()
    {
        StopCoroutine("Ghost");
        StartCoroutine("Ghost");
        GetComponent<AudioSource>().PlayOneShot(booste);
        ghost.transform.parent.GetComponent<Button>().interactable = false;
    }

    IEnumerator Run()
    {
        isRun = true;
        run.GetComponent<Outline>().enabled = true;
        while (run.fillAmount > 0f)
        {
            yield return null;
            run.fillAmount -= 0.35f * Time.deltaTime;
        }
        isRun = false;
        run.GetComponent<Outline>().enabled = false;
        while (run.fillAmount < 1f)
        {
            yield return null;
            run.fillAmount += 0.035f * Time.deltaTime;
        }
        run.fillAmount = 1;
        run.transform.parent.GetComponent<Button>().interactable = true;
    }

    public void OnRunClick()
    {
        StopCoroutine("Run");
        StartCoroutine("Run");
        GetComponent<AudioSource>().PlayOneShot(booste);
        run.transform.parent.GetComponent<Button>().interactable = false;
    }

    public void InfoChest()
    {
        StartCoroutine(Info("not enough space"));
    }

    IEnumerator Info(string str)
    {
        if (info.text == "")
        {
            switch (str)
            {
                case "gasoline":
                    info.text = PlayerPrefs.GetInt("Language") == 0 ? "I don't have a canister" : "У меня нет канистры";
                    break;
                case "not enough":
                    info.text = PlayerPrefs.GetInt("Language") == 0 ? "I don't have enough gas" : "У меня не хватает бензина";
                    break;
                case "enough":
                    info.text = PlayerPrefs.GetInt("Language") == 0 ? "Enough petrol" : "Достаточно бензина";
                    break;
                case "electricity":
                    info.text = PlayerPrefs.GetInt("Language") == 0 ? "The building is de-energized" : "Здание обесточено";
                    break;
                case "rust_key":
                    info.text = PlayerPrefs.GetInt("Language") == 0 ? "I don't have the key to the chest" : "У меня нет ключа от сундука";
                    break;
                case "key":
                    info.text = PlayerPrefs.GetInt("Language") == 0 ? "I don't have a key" : "У меня нет ключа";
                    break;
                case "wrench":
                    info.text = PlayerPrefs.GetInt("Language") == 0 ? "I don't have a wrench" : "У меня нет гаечного ключа";
                    break;
                case "crowbar":
                    info.text = PlayerPrefs.GetInt("Language") == 0 ? "I don't have a crowbar" : "У меня нет лома";
                    break;
                case "wardrobe":
                    info.text = PlayerPrefs.GetInt("Language") == 0 ? "Locked" : "Заперто";
                    break;
                case "not enough space":
                    info.text = PlayerPrefs.GetInt("Language") == 0 ? "There is not enough space in the chest" : "В сундуке не хватает места";
                    break;
                case "far":
                    info.text = PlayerPrefs.GetInt("Language") == 0 ? "The crossbar is too far away" : "Перекладина слишком далеко";
                    break;
                case "far2":
                    info.text = PlayerPrefs.GetInt("Language") == 0 ? "The hatch is too far away" : "Люк слишком далеко";
                    break;
                case "cartridges":
                    info.text = PlayerPrefs.GetInt("Language") == 0 ? "There are no more bullets" : "Патронов больше нет";
                    break;
                case "bullet":
                    info.text = PlayerPrefs.GetInt("Language") == 0 ? "Bullets are collected" : "Патроны собраны";
                    break;
                case "pistol":
                    info.text = PlayerPrefs.GetInt("Language") == 0 ? "Apparently, the gun is useless" : "Видимо, пистолет бесполезен";
                    break;
            }
            yield return new WaitForSeconds(2);
            info.text = "";
        }
    }

    IEnumerator Radio()
    {
        radio.Play();
        radio.transform.GetChild(0).gameObject.SetActive(true);
        radio.transform.GetChild(1).gameObject.SetActive(true);
        PlayerPrefs.SetInt("Auditions", PlayerPrefs.GetInt("Auditions") + 1);
        yield return new WaitForSeconds(87);
        radio.Stop();
        if (!isRadioDestroy)
        {
            radio.clip = noise;
            radio.volume = 0.2f;
            radio.loop = true;
            radio.Play();
        }
    }

    IEnumerator OpenAndClose(byte index)
    {
        if (!door[index].isPlaying)
        {
            if (index < 3)
                door[index].Play(isOpenDoor[index] ? "Door_" + (index + 1) + "_Close" : "Door_" + (index + 1) + "_Open");
            else
                door[index].Play(isOpenDoor[index] ? "Wardrobe_Door_" + (index - 2) + "_Close" : "Wardrobe_Door_" + (index - 2) + "_Open");
            cupboard.clip = isOpenDoor[index] ? (index == 0 ? doorClose : doorClose2) : (index == 0 ? doorOpen : doorOpen2);
            cupboard.Play();
            isOpenDoor[index] = !isOpenDoor[index];
            while (door[index].isPlaying)
            {
                player.Move(Vector3.one * 0.0000000001f * (isOpenDoor[index] ? -1 : 1));   // действие физики на игрока
                yield return null;
            }
        }
    }

    IEnumerator StartEngine()
    {
        generator.PlayOneShot(ignition);
        if (gasoline > 0.9f)
        {
            yield return new WaitForSeconds(0.2f);
            if (!generator.loop)
            {
                generator.loop = true;
                generator.clip = engine;
                generator.Play();
                generator.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                for (int i = 0; i < 6; i++)
                {
                    circleTable.GetChild(i).gameObject.SetActive(true);
                }
                for (int i = 0; i < 3; i++)
                {
                    butMat[i].color = new Color(butMat[i].color.r, butMat[i].color.g, butMat[i].color.b, 1);
                    notebook.GetChild(i).gameObject.SetActive(true);
                }
                if (enemy != null && PlayerPrefs.GetInt("Difficulty") != 4)
                    enemy.SetDestination();
            }
        }
        else
            StartCoroutine(Info("not enough"));
    }

    void SliderColor()
    {
        if (gasolineSlider.value <= 0.33f)
            fill.color = Color.Lerp(fill.color, new Color(0.9f, 0.5f, 0.25f), Time.deltaTime); // orange
        else if (gasolineSlider.value > 0.33f && gasolineSlider.value < 0.66f)
            fill.color = Color.Lerp(fill.color, new Color(0.9f, 0.8f, 0.25f), Time.deltaTime); // yellow
        else if (gasolineSlider.value >= 0.66f)
            fill.color = Color.Lerp(fill.color, new Color(0.3f, 0.8f, 0.3f), Time.deltaTime); // green
    }

    IEnumerator Valve()
    {
        generator.clip = valve;
        generator.Play();
        while (ActiveThing == 3 && hand.enabled && gasolineSlider.value > 0)
        {
            SliderColor();
            if (gasolineSlider.value < 0.9f && isGasolineFull)
            {
                isGasolineFull = false;
                interactiveThing[3].GetComponent<AudioSource>().clip = gasolineNotFull;
            }
            gasolineSlider.value -= Time.deltaTime * 0.3f;
            gasoline += Time.deltaTime * 0.3f;
            progress.text = (int)(gasolineSlider.value * 100) + "%";
            if (gasoline > 0.18f * gasolineStick)
            {
                switch (gasolineStick)
                {
                    case 1:
                        panelGenerator.GetChild(gasolineStick - 1).GetComponent<Renderer>().material.color = new Color(0.5f, 0.1f, 0);
                        break;
                    case 2:
                        panelGenerator.GetChild(gasolineStick - 1).GetComponent<Renderer>().material.color = new Color(0.5f, 0.3f, 0);
                        break;
                    case 3:
                        panelGenerator.GetChild(gasolineStick - 1).GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0);
                        break;
                    case 4:
                        panelGenerator.GetChild(gasolineStick - 1).GetComponent<Renderer>().material.color = new Color(0.3f, 0.5f, 0);
                        break;
                    case 5:
                        panelGenerator.GetChild(gasolineStick - 1).GetComponent<Renderer>().material.color = new Color(0.1f, 0.5f, 0);
                        break;
                }
                gasolineStick++;
            }
            yield return null;
        }
        if (hand.enabled && gasoline > 0.9f)
        {
            yield return new WaitForSeconds(0.1f);
            Drop();
            interactiveThing[3].gameObject.layer = LayerMask.NameToLayer("Default");
            Destroy(interactiveThing[3].GetComponent<AudioSource>(), 2);
            Destroy(interactiveThing[3].GetComponent<Fall>(), 2);
            Destroy(gasolineSlider.gameObject);
        }
        generator.clip = null;
    }

    IEnumerator Barrel()
    {
        GetComponent<AudioSource>().clip = valve;
        GetComponent<AudioSource>().Play();
        while (ActiveThing == 3 && hand.enabled && gasolineSlider.value < 1)
        {
            SliderColor();
            if (gasolineSlider.value > 0.9f && !isGasolineFull)
            {
                isGasolineFull = true;
                interactiveThing[3].GetComponent<AudioSource>().clip = gasolineFull;
            }
            gasolineSlider.value += Time.deltaTime * 0.3f;
            progress.text = (int)(gasolineSlider.value * 100) + "%";
            yield return null;
        }
        if (hand.enabled)
        {
            yield return new WaitForSeconds(0.1f);
            GetComponent<AudioSource>().clip = null;
        }
    }

    IEnumerator ErrorOrOpen()
    {
        GetComponent<AudioSource>().clip = null;
        GetComponent<AudioSource>().PlayOneShot(isOpenLock ? _true : error);
        for (int i = 0; i < 2; i++)
        {
            lockText.text = isOpenLock ? (PlayerPrefs.GetInt("Language") == 0 ? "OPEN" : "ОТКРЫТО") : (PlayerPrefs.GetInt("Language") == 0 ? "ERROR" : "ОШИБКА");
            yield return new WaitForSeconds(0.2f);
            lockText.text = "";
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator Wait(int ActiveThingWait)
    {
        yield return new WaitForSeconds(0.2f);
        if (ActiveThingWait != 6)
            interactiveThing[ActiveThingWait].GetComponent<MeshCollider>().enabled = true;
        else
        {
            interactiveThing[ActiveThingWait].transform.GetChild(0).GetComponent<MeshCollider>().enabled = true;
            interactiveThing[ActiveThingWait].transform.GetChild(1).GetComponent<MeshCollider>().enabled = true;
        }
    }

    IEnumerator ScaleGasoline()
    {
        float scale = 0.03f;
        while (scale < 0.035f)
        {
            scale += Time.deltaTime * 0.01f;
            interactiveThing[3].transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }
        interactiveThing[3].transform.localScale = new Vector3(0.035f, 0.035f, 0.035f);
    }

    public void Drop()
    {
        if (ActiveThing > -1)
        {
            if (ActiveThing == 3)
                StartCoroutine(ScaleGasoline());
            interactiveThing[ActiveThing].parent = Interactive;
            interactiveThing[ActiveThing].GetComponent<Rigidbody>().isKinematic = false;
            Physics.Raycast(cam.position, cam.forward, out hit);
            interactiveThing[ActiveThing].GetComponent<Rigidbody>().AddForce((ActiveThing == 3 ? interactiveThing[3].up : interactiveThing[ActiveThing].forward * (ActiveThing == 5 ? -1 : 1)) * (ActiveThing < 3 ? 120 : 200) * (Mathf.Clamp01(hit.distance * 0.7f) - 0.6f));
            interactiveThing[ActiveThing].GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(-200, 200), 0));
            StartCoroutine(Wait(ActiveThing));
            inventory.enabled = false;
            ActiveThing = -1;
        }
    }

    void GetThing()
    {
        interactiveThing[ActiveThing].parent = Null;
        interactiveThing[ActiveThing].GetComponent<Rigidbody>().isKinematic = true;
        if (ActiveThing != 6)
            interactiveThing[ActiveThing].GetComponent<MeshCollider>().enabled = false;
        else
        {
            interactiveThing[ActiveThing].transform.GetChild(0).GetComponent<MeshCollider>().enabled = false;
            interactiveThing[ActiveThing].transform.GetChild(1).GetComponent<MeshCollider>().enabled = false;
        }

        if (ActiveThing == 3)
        {
            interactiveThing[ActiveThing].transform.localRotation = Quaternion.Euler(-10, 30, 40);
            interactiveThing[ActiveThing].transform.localPosition = new Vector3(0.2f, -0.12f, 0.4f);
            interactiveThing[ActiveThing].transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
        }
        else if (ActiveThing == 5)
        {
            interactiveThing[ActiveThing].transform.localRotation = Quaternion.Euler(40, -235, 10);
            interactiveThing[ActiveThing].transform.localPosition = new Vector3(0.38f, -0.4f, 0.2f);
        }
        else if (ActiveThing == 6)
        {
            interactiveThing[ActiveThing].transform.localRotation = Quaternion.Euler(-10, 15, 50);
            interactiveThing[ActiveThing].transform.localPosition = new Vector3(0.155f, -0.1f, 0.45f);
        }
        else
        {
            interactiveThing[ActiveThing].transform.localRotation = Quaternion.Euler(-20, -20, 80);
            interactiveThing[ActiveThing].transform.localPosition = new Vector3(0.25f, -0.1f, 0.6f);
        }

        if (gun.GetChild(0).gameObject.activeSelf)
            player.GetComponent<Player>().GetGun();
        else if (Null.GetChild(2).gameObject.activeSelf)
            player.GetComponent<Player>().GetNotRealGun();
        else
            lantern.SetActive(true);
        inventory.enabled = true;
    }

    IEnumerator AnimBigDoor()
    {
        Animation anim = hit.collider.GetComponent<Animation>();
        if (!anim.isPlaying)
        {
            bool isInvert = (hit.collider.transform.parent.forward.x < -0.99f && (player.transform.position - hit.collider.transform.position).x > 0) ||
                            (hit.collider.transform.parent.forward.x > 0.99f && (player.transform.position - hit.collider.transform.position).x < 0) ||
                            (hit.collider.transform.parent.forward.z < -0.99f && (player.transform.position - hit.collider.transform.position).z > 0) ||
                            (hit.collider.transform.parent.forward.z > 0.99f && (player.transform.position - hit.collider.transform.position).z < 0);

            GetComponent<AudioSource>().pitch = Random.Range(1f, 1.2f);
            if (anim.clip == bigDoor_Open)
            {
                anim.clip = bigDoor_Close;
                GetComponent<AudioSource>().PlayOneShot(bigDoor_close);
            }
            else if (anim.clip == bigDoor_Open_invert)
            {
                anim.clip = bigDoor_Close_invert;
                GetComponent<AudioSource>().PlayOneShot(bigDoor_close);
            }
            else if (anim.clip == bigDoor_Close || anim.clip == bigDoor_Close_invert)
            {
                if (isInvert)
                    anim.clip = bigDoor_Open_invert;
                else
                    anim.clip = bigDoor_Open;
                GetComponent<AudioSource>().PlayOneShot(bigDoor_open);
            }
            anim.Play();

            while (anim.isPlaying)
            {
                player.Move(Vector3.one * 0.0000000001f);   // действие физики на игрока
                yield return null;
            }
            GetComponent<AudioSource>().pitch = 1;
        }
    }

    IEnumerator SafeOpen(Animation door)
    {
        hit.collider.gameObject.layer = LayerMask.NameToLayer("Default");
        safe.Open();
        door.Play();
        while (door.isPlaying)
        {
            player.Move(Vector3.one * 0.0000000001f);   // действие физики на игрока
            yield return null;
        }
    }

    IEnumerator OpenChest()
    {
        yield return new WaitForSeconds(1f);
        isChestOpened = true;
        isChestOpen = true;
    }

    public void Hand()
    {
        switch (hit.collider.name)
        {
            case "Hatch":
                if (hit.distance < 0.5f)
                {
                    RenderSettings.fogColor = new Color(0.04f, 0.09f, 0.12f);
                    RenderSettings.skybox = sky;
                    playerSavePos = player.transform.position;
                    player.transform.position = new Vector3(0.7f, 101, 0);
                    player.transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 0);
                    CameraRotation.Remove();
                    player.transform.GetChild(1).localRotation = Quaternion.Euler(0, 0, 0);
                    GrassController.isOnTheStreet = true;
                    setActiveObjects.SetActive(true);
                    forest.Play();
                    enemy.Street();
                }
                else
                    StartCoroutine(Info("far2"));
                break;
            case "Hatch_2":
                if (hit.distance < 1.5f)
                {
                    RenderSettings.fogColor = new Color(1, 0.9f, 0.6f);
                    RenderSettings.skybox = null;
                    player.transform.position = playerSavePos;
                    GrassController.isOnTheStreet = false;
                    setActiveObjects.SetActive(false);
                    forest.Pause();
                }
                else
                    StartCoroutine(Info("far2"));
                break;
            case "Paper":
                Drop();
                ActiveThing = 7;
                GetThing();
                break;
            case "BigDoor":
                StartCoroutine(AnimBigDoor());
                break;
            case "Radio":
                if (ActiveThing != 5)
                {
                    if (!isRadioEnabled)
                    {
                        isRadioEnabled = true;
                        StartCoroutine(Radio());
                    }
                }
                else
                {
                    GetComponent<AudioSource>().PlayOneShot(hitAudio);
                    isRadioDestroy = true;
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
                break;
            case "Door_1":
                StartCoroutine(OpenAndClose(0));
                break;
            case "Door_2":
                StartCoroutine(OpenAndClose(1));
                break;
            case "Door_3":
                StartCoroutine(OpenAndClose(2));
                break;
            case "Wardrobe_Doors_L":
                if (isPadlockOpen)
                    StartCoroutine(OpenAndClose(3));
                else
                    StartCoroutine(Info("wardrobe"));
                break;
            case "Wardrobe_Doors_R":
                if (isPadlockOpen)
                    StartCoroutine(OpenAndClose(4));
                else
                    StartCoroutine(Info("wardrobe"));
                break;
            case "Padlock":
                if (ActiveThing == 1 || ActiveThing == 5)
                {
                    isPadlockOpen = true;
                    Rigidbody[] lockParents = hit.collider.transform.parent.GetComponentsInChildren<Rigidbody>();
                    for (int i = 0; i < 2; i++)
                    {
                        lockParents[i].isKinematic = false;
                        lockParents[i].gameObject.layer = LayerMask.NameToLayer("Default");
                        Destroy(lockParents[i].GetComponent<Rigidbody>(), 2);
                        Destroy(lockParents[i].GetComponent<AudioSource>(), 2);
                        Destroy(lockParents[i].GetComponent<Fall>(), 2);
                    }
                }
                else
                    StartCoroutine(Info("key"));
                break;
            case "RustKey":
                Drop();
                ActiveThing = 0;
                GetThing();
                break;
            case "Key":
                Drop();
                ActiveThing = 1;
                GetThing();
                break;
            case "USB":
                Drop();
                ActiveThing = 2;
                GetThing();
                break;
            case "Gasoline":
                Drop();
                ActiveThing = 3;
                GetThing();
                break;
            case "Wrench":
                Drop();
                ActiveThing = 4;
                GetThing();
                break;
            case "Crowbar":
                Drop();
                ActiveThing = 5;
                GetThing();
                break;
            case "MeeseeksBox":
                Drop();
                ActiveThing = 6;
                GetThing();
                break;
            case "MeeseeksBoxButton":
                if (!hit.collider.GetComponent<Animation>().isPlaying)
                    hit.collider.GetComponent<Animation>().Play();
                int saveIndex = portalNULL.Length - 1;
                for (int i = 0; i < portalNULL.Length - 1; i++)
                    if (Vector3.Distance(hit.point, portalNULL[i].position) < Vector3.Distance(hit.point, portalNULL[saveIndex].position))
                        saveIndex = i;    
                bool isX = (portalNULL[saveIndex].localRotation.y > -0.05f && portalNULL[saveIndex].localRotation.y < 0.05f) || (portalNULL[saveIndex].localRotation.y > 0.9f);
                BluePortal.position = new Vector3(!isX ? portalNULL[saveIndex].position.x : Mathf.Clamp(player.transform.position.x, portalNULL[saveIndex].position.x - portalNULL[saveIndex].localScale.z, portalNULL[saveIndex].position.x + portalNULL[saveIndex].localScale.z), portalNULL[saveIndex].position.y,
                                                  isX ? portalNULL[saveIndex].position.z : Mathf.Clamp(player.transform.position.z, portalNULL[saveIndex].position.z - portalNULL[saveIndex].localScale.z, portalNULL[saveIndex].position.z + portalNULL[saveIndex].localScale.z));
                BluePortal.rotation = portalNULL[saveIndex].rotation;
                BluePortal.gameObject.SetActive(true);
                break;
            case "Pistol":
            case "Shotgun":
                Drop();
                gunUI.GetChild(0).gameObject.SetActive(true);
                gunUI.GetChild(1).gameObject.SetActive(true);
                if (hit.collider.name == "Shotgun")
                {
                    fire.isPistol = false;
                    gunUI.GetChild(1).GetComponent<Image>().sprite = guns[0];
                    gunUI.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(180, 80);
                    player.GetComponent<Player>().GetGun();
                    fire.Count(true);
                    shotgunToDestroy.SetActive(false);
                    if (!pistolToDestroy.activeSelf)
                    {
                        pistolToDestroy.SetActive(true);
                        pistolToDestroy.transform.position = new Vector3(player.transform.position.x, 0.04f, player.transform.position.z);
                    }
                }
                else
                {
                    StartCoroutine(Info("pistol"));
                    fire.isPistol = true;
                    gunUI.GetChild(1).GetComponent<Image>().sprite = guns[1];
                    gunUI.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(100, 60);
                    player.GetComponent<Player>().GetNotRealGun();
                    fire.CountPistol();
                    pistolToDestroy.SetActive(false);
                    if (!shotgunToDestroy.activeSelf)
                    {
                        shotgunToDestroy.SetActive(true);
                        shotgunToDestroy.transform.position = new Vector3(player.transform.position.x, 0.02f, player.transform.position.z);
                        shotgunToDestroy.transform.rotation = Quaternion.Euler(0, 0, 90);
                    }
                }
                break;
            case "Valve":
                if (ActiveThing == 3)
                {
                    if (gasolineSlider.value > 0)
                    {
                        if (!generator.isPlaying)
                        {
                            StopCoroutine("Valve");
                            StartCoroutine("Valve");
                        }
                    }
                    else
                        StartCoroutine(Info("not enough"));
                }
                else if (generator.loop)
                    StartCoroutine(Info("enough"));
                else
                    StartCoroutine(Info("gasoline"));
                break;
            case "Barrel":
                if (ActiveThing == 3)
                {
                    if (((!GetComponent<AudioSource>().isPlaying && GetComponent<AudioSource>().clip == valve) || (GetComponent<AudioSource>().clip != valve)) && gasolineSlider.value < 1)
                    {
                        StopCoroutine("Barrel");
                        StartCoroutine("Barrel");
                    }
                }
                else
                    StartCoroutine(Info("gasoline"));
                break;
            case "Sink":
                if (ActiveThing == 4)
                {
                    sink.Play();
                    Destroy(sink.gameObject, 2);
                    spray.Play();
                    Destroy(spray.gameObject, 3);
                    hit.collider.gameObject.layer = LayerMask.NameToLayer("Default");
                    hit.collider.GetComponent<AudioSource>().Play();
                    PlayerPrefs.SetInt("Fix", PlayerPrefs.GetInt("Fix") + 1);
                }
                else
                    StartCoroutine(Info("wrench"));
                break;
            case "Cartridges":
                fire.AddBullets();
                StartCoroutine(Info("bullet"));
                hit.collider.name = "Cartridges_empty";
                GetComponent<AudioSource>().PlayOneShot(selection);
                break;
            case "Cartridges_empty":
                StartCoroutine(Info("cartridges"));
                break;
            case "crossbars":
                if (hit.distance < 0.75f && Mathf.Abs(hit.collider.transform.position.x - player.transform.position.x) < 0.2f)
                {
                    if (player.transform.position.y < 2f)
                    {
                        StopCoroutine("PlayerMove");
                        StartCoroutine("PlayerMove");
                    }
                }
                else
                    StartCoroutine(Info("far"));
                break;
            case "lock":
                safe.Lock();
                break;
            case "faucet":
                if (isCanOpenSafe)
                    StartCoroutine(SafeOpen(hit.collider.transform.parent.GetComponent<Animation>()));
                else
                    StartCoroutine(Info("wardrobe"));
                break;
            case "propeller":
                hit.collider.GetComponent<Rotate>().speed = 0;
                break;
            case "Notebook":
                if (generator.loop)
                {
                    screen.GetComponent<NotebookController>().isUSB = ActiveThing == 2;
                    screen.SetActive(true);
                }
                else
                    StartCoroutine(Info("electricity"));
                break;
            case "Button":
                StartCoroutine(StartEngine());
                break;
            case "Chest":
                if (ActiveThing == 0 || isChestOpened)
                {
                    if (!chest.isPlaying)
                    {
                        GetComponent<AudioSource>().clip = null;
                        GetComponent<AudioSource>().PlayOneShot(isChestOpen ? chest_close : (isChestOpened ? chest_open_2 : chest_open));
                        if (isChestOpened)
                            chest["Cheat_open"].normalizedTime = 0.625f;
                        else
                            StartCoroutine(OpenChest());
                        chest.Play(isChestOpen ? "Cheat_close" : "Cheat_open");
                        if (isChestOpened)
                            isChestOpen = !isChestOpen;
                    }
                }
                else
                    StartCoroutine(Info("rust_key"));
                break;
            case "BigVent1":
                if (ActiveThing == 5)
                {
                    hit.collider.gameObject.layer = LayerMask.NameToLayer("Default");
                    Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                    rb.useGravity = true;
                    rb.isKinematic = false;
                    rb.AddForce(hit.collider.transform.forward * 5);
                    rb.AddTorque(hit.collider.transform.right * -10);
                    GameObject.Find("Passage").GetComponent<VentArea>().isCan = true;
                }
                else
                    StartCoroutine(Info("crowbar"));
                break;
            case "BigVent2":
                if (ActiveThing == 5)
                {
                    hit.collider.gameObject.layer = LayerMask.NameToLayer("Default");
                    Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                    rb.useGravity = true;
                    rb.isKinematic = false;
                    rb.AddForce(hit.collider.transform.forward * 5);
                    rb.AddTorque(hit.collider.transform.right * -10);
                    GameObject.Find("Passage_2").GetComponent<VentArea>().isCan = true;
                }
                else
                    StartCoroutine(Info("crowbar"));
                break;
            case "Lamp_wall":
                hit.collider.gameObject.layer = LayerMask.NameToLayer("Default");
                hit.collider.GetComponent<Animation>().Play();
                Rahmen.isKinematic = false;
                break;
            case "Delete":
                if (generator.loop)
                {
                    GetComponent<AudioSource>().clip = null;
                    GetComponent<AudioSource>().PlayOneShot(write);
                    if (lockText.text.Length > 0 && !isOpenLock)
                        lockText.text = lockText.text.Substring(0, lockText.text.Length - 1);
                }
                else
                    StartCoroutine(Info("electricity"));
                break;
            case "Enter":
                if (generator.loop)
                {
                    GetComponent<AudioSource>().clip = null;
                    GetComponent<AudioSource>().PlayOneShot(write);
                    if (lockText.text.Length == 4 && !isOpenLock)
                    {
                        if (randomNumberCode[0].ToString() == lockText.text[0].ToString() &&
                        randomNumberCode[1].ToString() == lockText.text[1].ToString() &&
                        randomNumberCode[2].ToString() == lockText.text[2].ToString() &&
                        randomNumberCode[3].ToString() == lockText.text[3].ToString())
                        {
                            isOpenLock = true;
                            for (int i = 0; i < 12; i++)
                                buttons.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Default");
                            doorAnim.StartOpen();
                        }
                        StartCoroutine(ErrorOrOpen());
                    }
                }
                else
                    StartCoroutine(Info("electricity"));
                break;
            default:
                if (generator.loop)
                {
                    GetComponent<AudioSource>().clip = null;
                    GetComponent<AudioSource>().PlayOneShot(write);
                    if (!isOpenLock)
                        for (int i = 0; i < 10; i++)
                            if (hit.collider.name == i.ToString() && lockText.text.Length < 4)
                                lockText.text += i;
                }
                else
                    StartCoroutine(Info("electricity"));
                break;
        }
    }

    void Start()
    {
        isChestOpen = false;
        isRun = false;
        cam.GetComponent<Camera>().fieldOfView = 1.618f * 55f * Screen.height / Screen.width;
        numberGhost.text = PlayerPrefs.GetInt("Ghost").ToString();
        if (numberGhost.text == "0")
        {
            ghost.transform.parent.GetComponent<Button>().interactable = false;
        }
        code.text = PlayerPrefs.GetInt("Language") == 0 ? "code: " : "код: ";
        for (int i = 0; i < 4; i++)
        {
            randomNumberCode[i] = Random.Range(0, 10);
            code.text += randomNumberCode[i];
        }
        for (int i = 0; i < 3; i++)
            butMat[i].color = new Color(butMat[i].color.r, butMat[i].color.g, butMat[i].color.b, 0.2f);
        gunUI.GetChild(0).gameObject.SetActive(false);
        gunUI.GetChild(1).gameObject.SetActive(false);
        if (PlayerPrefs.GetInt("Difficulty") == 4)
            Destroy(enemy.gameObject);
        setActiveObjects.SetActive(false);
        int GasPlace = (new int[] { 5, 7 })[Random.Range(0, 2)];
        interactiveThing[3].position = spawnPoint[GasPlace].position;
        interactiveThing[3].rotation = spawnPoint[GasPlace].rotation;
        if (GasPlace == 5)
        {
            int RustKeyPlace = (new int[] { 3, 7, 8 })[Random.Range(0, 3)];
            interactiveThing[0].position = spawnPoint[RustKeyPlace].position;
            interactiveThing[0].rotation = spawnPoint[RustKeyPlace].rotation;
            if (RustKeyPlace == 3)
            {
                int WrenchPlace = (new int[] { 7, 8 })[Random.Range(0, 2)];
                interactiveThing[4].position = spawnPoint[WrenchPlace].position;
                interactiveThing[4].rotation = spawnPoint[WrenchPlace].rotation;
                int KeyPlace = (new int[] { 0, 1, 2, 4 })[Random.Range(0, 4)];
                interactiveThing[1].position = spawnPoint[KeyPlace].position;
                interactiveThing[1].rotation = spawnPoint[KeyPlace].rotation;
                if (KeyPlace == 0 || KeyPlace == 1 || KeyPlace == 2)
                {
                    int USBPlace = 4;
                    interactiveThing[2].position = spawnPoint[USBPlace].position;
                    interactiveThing[2].rotation = spawnPoint[USBPlace].rotation;
                    int CrowbarPlace = 6;
                    interactiveThing[5].position = spawnPoint[CrowbarPlace].position;
                    interactiveThing[5].rotation = spawnPoint[CrowbarPlace].rotation;
                }
                else if (KeyPlace == 4)
                {
                    int USBPlace = (new int[] { 0, 1, 2 })[Random.Range(0, 3)];
                    interactiveThing[2].position = spawnPoint[USBPlace].position;
                    interactiveThing[2].rotation = spawnPoint[USBPlace].rotation;
                    int CrowbarPlace = 6;
                    interactiveThing[5].position = spawnPoint[CrowbarPlace].position;
                    interactiveThing[5].rotation = spawnPoint[CrowbarPlace].rotation;
                }
            }
            else if (RustKeyPlace == 7 || RustKeyPlace == 8)
            {
                int KeyPlace = (new int[] { 0, 1, 2, 3 })[Random.Range(0, 4)];
                interactiveThing[1].position = spawnPoint[KeyPlace].position;
                interactiveThing[1].rotation = spawnPoint[KeyPlace].rotation;
                if (KeyPlace == 0 || KeyPlace == 1 || KeyPlace == 2)
                {
                    int WrenchPlace = 6;
                    interactiveThing[4].position = spawnPoint[WrenchPlace].position;
                    interactiveThing[4].rotation = spawnPoint[WrenchPlace].rotation;
                    int USBPlace = 3;
                    interactiveThing[2].position = spawnPoint[USBPlace].position;
                    interactiveThing[2].rotation = spawnPoint[USBPlace].rotation;
                    int CrowbarPlace = 9;
                    interactiveThing[5].position = spawnPoint[CrowbarPlace].position;
                    interactiveThing[5].rotation = spawnPoint[CrowbarPlace].rotation;
                }
                else if (KeyPlace == 3)
                {
                    int WrenchPlace = 6;
                    interactiveThing[4].position = spawnPoint[WrenchPlace].position;
                    interactiveThing[4].rotation = spawnPoint[WrenchPlace].rotation;
                    int USBPlace = (new int[] { 0, 1, 2 })[Random.Range(0, 3)];
                    interactiveThing[2].position = spawnPoint[USBPlace].position;
                    interactiveThing[2].rotation = spawnPoint[USBPlace].rotation;
                    int CrowbarPlace = 9;
                    interactiveThing[5].position = spawnPoint[CrowbarPlace].position;
                    interactiveThing[5].rotation = spawnPoint[CrowbarPlace].rotation;
                }
            }
        }
        else if (GasPlace == 7)
        {
            int KeyPlace = (new int[] { 3, 5 })[Random.Range(0, 2)];
            interactiveThing[1].position = spawnPoint[KeyPlace].position;
            interactiveThing[1].rotation = spawnPoint[KeyPlace].rotation;
            if (KeyPlace == 3)
            {
                int WrenchPlace = (new int[] { 5, 6 })[Random.Range(0, 2)];
                interactiveThing[4].position = spawnPoint[WrenchPlace].position;
                interactiveThing[4].rotation = spawnPoint[WrenchPlace].rotation;
                if (WrenchPlace == 5)
                {
                    int RustKeyPlace = (new int[] { 0, 1, 2, 4, 6 })[Random.Range(0, 5)];
                    interactiveThing[0].position = spawnPoint[RustKeyPlace].position;
                    interactiveThing[0].rotation = spawnPoint[RustKeyPlace].rotation;
                    if (RustKeyPlace == 0 || RustKeyPlace == 1 || RustKeyPlace == 2)
                    {
                        int USBPlace = 4;
                        interactiveThing[2].position = spawnPoint[USBPlace].position;
                        interactiveThing[2].rotation = spawnPoint[USBPlace].rotation;
                        int CrowbarPlace = 6;
                        interactiveThing[5].position = spawnPoint[CrowbarPlace].position;
                        interactiveThing[5].rotation = spawnPoint[CrowbarPlace].rotation;
                        Destroy(interactiveThing[5].gameObject);
                    }
                    else if (RustKeyPlace == 4 || RustKeyPlace == 6)
                    {
                        int USBPlace = (new int[] { 0, 1, 2 })[Random.Range(0, 3)];
                        interactiveThing[2].position = spawnPoint[USBPlace].position;
                        interactiveThing[2].rotation = spawnPoint[USBPlace].rotation;
                        int CrowbarPlace = 9;
                        interactiveThing[5].position = spawnPoint[CrowbarPlace].position;
                        interactiveThing[5].rotation = spawnPoint[CrowbarPlace].rotation;
                    }
                }
                else if (WrenchPlace == 6)
                {
                    int USBPlace = 4;
                    interactiveThing[2].position = spawnPoint[USBPlace].position;
                    interactiveThing[2].rotation = spawnPoint[USBPlace].rotation;
                    int CrowbarPlace = 5;
                    interactiveThing[5].position = spawnPoint[CrowbarPlace].position;
                    interactiveThing[5].rotation = spawnPoint[CrowbarPlace].rotation;
                    int RustKeyPlace = (new int[] { 0, 1, 2 })[Random.Range(0, 3)];
                    interactiveThing[0].position = spawnPoint[RustKeyPlace].position;
                    interactiveThing[0].rotation = spawnPoint[RustKeyPlace].rotation;
                }
            }
            else if (KeyPlace == 5)
            {
                int RustKeyPlace = 3;
                interactiveThing[0].position = spawnPoint[RustKeyPlace].position;
                interactiveThing[0].rotation = spawnPoint[RustKeyPlace].rotation;
                int WrenchPlace = (new int[] { 0, 1, 2, 6 })[Random.Range(0, 4)];
                interactiveThing[4].position = spawnPoint[WrenchPlace].position;
                interactiveThing[4].rotation = spawnPoint[WrenchPlace].rotation;
                if (WrenchPlace == 0 || WrenchPlace == 1 || WrenchPlace == 2)
                {
                    int USBPlace = 4;
                    interactiveThing[2].position = spawnPoint[USBPlace].position;
                    interactiveThing[2].rotation = spawnPoint[USBPlace].rotation;
                    int CrowbarPlace = 6;
                    interactiveThing[5].position = spawnPoint[CrowbarPlace].position;
                    interactiveThing[5].rotation = spawnPoint[CrowbarPlace].rotation;
                }
                else if (WrenchPlace == 6)
                {
                    int USBPlace = (new int[] { 0, 1, 2, 4 })[Random.Range(0, 4)];
                    interactiveThing[2].position = spawnPoint[USBPlace].position;
                    interactiveThing[2].rotation = spawnPoint[USBPlace].rotation;
                    int CrowbarPlace = 9;
                    interactiveThing[5].position = spawnPoint[CrowbarPlace].position;
                    interactiveThing[5].rotation = spawnPoint[CrowbarPlace].rotation;
                }
            }
        }
    }

    void Update()
    {
        if (Physics.Raycast(cam.position, cam.forward, out hit, 1.6f, LayerMask.GetMask("Interactive")))
            hand.enabled = !(Physics.Raycast(cam.position, cam.forward, out hit2, 2, LayerMask.GetMask("Default")) && hit.distance > hit2.distance);
        else
            hand.enabled = false;
        squareCenterPoint.enabled = hand.enabled;
        if (hand.enabled && gasolineSlider != null)
        {
            gasolineSlider.gameObject.SetActive(ActiveThing == 3 && (hit.collider.name == "Valve" || hit.collider.name == "Barrel"));
            isLastFrame = gasolineSlider.gameObject.activeSelf;
        }
        else if (gasolineSlider != null)
        {
            gasolineSlider.gameObject.SetActive(false);
            if (isLastFrame)
            {
                if (generator.clip == valve)
                    generator.Pause();
                else if (GetComponent<AudioSource>().clip == valve)
                    GetComponent<AudioSource>().Pause();
            }
            isLastFrame = false;
        }
    }
}