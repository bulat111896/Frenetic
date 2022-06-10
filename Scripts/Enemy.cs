using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public GameObject lostCanvas;
    public Transform player, head;
    public Transform[] nulls, doors, carNulls;
    public AudioClip[] steps;
    public AudioClip end, laughter, laughter_2, glitch, scary, scary_2;
    public AnimationClip bigDoor_Open, bigDoor_Open_invert;
    public CarArea[] carArea;
    public Heartbeat heartbeat;
    CharacterController playerCharacterController;
    NavMeshAgent agent;
    Animator animator;
    RaycastHit hit;
    float angle, speedDifficulty;
    public bool isSaw;
    bool isAttack, isScared, isSitInPlace;

    public void Street()
    {
        StopCoroutine("Chase");
        StopCoroutine("SitInPlace");
        StartCoroutine("SearchPlayer");
    }

    public void StartSitInPlace(int index)
    {
        StopCoroutine("SitInPlace");
        isSitInPlace = false;
        if (index == 0)
            StartCoroutine("SitInPlace");
    }

    IEnumerator SitInPlace()
    {
        yield return new WaitForSeconds(1);
        isSitInPlace = true;
    }

    IEnumerator WalkAudio()
    {
        int i = 0;
        while (true)
        {
            yield return new WaitForSeconds(1f / animator.speed * 0.75f);
            GetComponent<AudioSource>().PlayOneShot(steps[i]);
            i++;
            if (i == steps.Length)
                i = 0;
        }
    }

    public void Home()
    {
        PlayerPrefs.SetInt("Scene", 0);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void SetDestination()
    {
        if (agent != null)
            agent.SetDestination(player.position);
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerCharacterController = player.GetComponent<CharacterController>();
        if (PlayerPrefs.GetInt("Difficulty") == 0)
            speedDifficulty = 0.8f;
        else if (PlayerPrefs.GetInt("Difficulty") == 1)
            speedDifficulty = 1.1f;
        else if (PlayerPrefs.GetInt("Difficulty") == 2)
            speedDifficulty = 1.4f;
        else if (PlayerPrefs.GetInt("Difficulty") == 3)
            speedDifficulty = 1.8f;
        StartCoroutine("SearchPlayer");
        StartCoroutine("BigDoorController");
        StartCoroutine("WalkAudio");
    }

    IEnumerator BigDoorController()
    {
        int i;
        while (!isAttack)
        {
            for (i = 0; i < doors.Length; i++)
                if (Vector3.Distance(transform.position, doors[i].position) < 2f)
                {
                    Animation anim = doors[i].GetChild(0).GetComponent<Animation>();
                    bool isInvert = (doors[i].forward.x < -0.99f && (transform.position - doors[i].position).x > 0) ||
                                    (doors[i].forward.x > 0.99f && (transform.position - doors[i].position).x < 0) ||
                                    (doors[i].forward.z < -0.99f && (transform.position - doors[i].position).z > 0) ||
                                    (doors[i].forward.z > 0.99f && (transform.position - doors[i].position).z < 0);
                    if (anim.clip.name == "BigDoor_Close" || anim.clip.name == "BigDoor_Close_invert")
                    {
                        if (!anim.isPlaying)
                        {
                            if (isInvert)
                                anim.clip = bigDoor_Open_invert;
                            else
                                anim.clip = bigDoor_Open;
                            anim.Play();
                            GetComponent<AudioSource>().pitch = Random.Range(1f, 1.2f);
                            GetComponent<AudioSource>().Play();
                            while (anim.isPlaying)
                                yield return null;
                            GetComponent<AudioSource>().pitch = 1;
                        }
                    }
                }
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator Attack()
    {
        if (GameController.isGhost)
            yield break;
        if (carArea[0].isThisPlace)
        {
            carArea[0].EnemyKill();
            int index = Vector3.Distance(transform.position, carNulls[0].position) < Vector3.Distance(transform.position, carNulls[1].position) ? 0 : 1;
            player.position = new Vector3(carNulls[index].position.x, player.position.y, carNulls[index].position.z);
        }
        else if (carArea[1].isThisPlace)
            carArea[1].EnemyKill();
        else
            player.GetComponent<Animation>().Stop();
        StopCoroutine("Chase");
        StopCoroutine("SearchPlayer");
        StopCoroutine("BigDoorController");
        StopCoroutine("WalkAudio");
        isAttack = true;
        agent.isStopped = true;
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z) - player.right);
        player.GetChild(0).localRotation = Quaternion.Euler(0, 0, 0);
        player.GetChild(1).localRotation = Quaternion.Euler(0, 0, 0);
        animator.SetBool("Attack", true);
        GameObject.Find("Canvas").SetActive(false);
        Destroy(GameObject.Find("Player").GetComponent<CharacterController>());
        Destroy(GameObject.Find("Player").GetComponent<Player>());
        Destroy(GameObject.Find("Player").GetComponent<Rigidbody>());
        Destroy(GameObject.Find("GameController"));
        Destroy(GameObject.Find("Heartbeat"));
        yield return null;                                                           // обновление animator.speed
        animator.speed = speedDifficulty;                                            // изменение скорости анимации
        agent.speed = 2f * speedDifficulty;                                          // изменение скорости персонажа
        yield return new WaitForSeconds(1f / animator.speed * 1.55f);
        Destroy(GameObject.Find("Canvas"));
        Destroy(GameObject.Find("House"));
        Destroy(GameObject.Find("Interactive"));
        lostCanvas.SetActive(true);
        GetComponent<AudioSource>().spatialBlend = 0;
        GetComponent<AudioSource>().PlayOneShot(end);
        Image bg = lostCanvas.transform.GetChild(0).GetComponent<Image>();
        Image text = lostCanvas.transform.GetChild(1).GetComponent<Image>();
        Text continueText = lostCanvas.transform.GetChild(2).GetComponent<Text>();
        bg.color = new Color(0, 0, 0, 1);
        text.color = new Color(1, 0, 0, 0);
        continueText.color = new Color(1, 1, 1, 0);
        PlayerPrefs.SetInt("Deaths", PlayerPrefs.GetInt("Deaths") + 1);
        yield return new WaitForSeconds(1);
        GetComponent<AudioSource>().PlayOneShot(laughter_2);
        StartCoroutine(Glitch());
        bg.GetComponent<RectTransform>().sizeDelta = (float)Screen.width / Screen.height < 1920f / 1080f ? new Vector2(lostCanvas.GetComponent<RectTransform>().sizeDelta.y * (1920f / 1080f), lostCanvas.GetComponent<RectTransform>().sizeDelta.y) : new Vector2(lostCanvas.GetComponent<RectTransform>().sizeDelta.x, lostCanvas.GetComponent<RectTransform>().sizeDelta.x / (1920f / 1080f));
        while (true)
        {
            bg.color = new Color(Mathf.Lerp(bg.color.r, 0.5f, Time.deltaTime * 0.3f), 0, 0);
            text.color = new Color(1, 0, 0, Mathf.Lerp(text.color.a, 1f, Time.deltaTime * 0.2f));
            continueText.color = new Color(1, 1, 1, Mathf.Lerp(continueText.color.a, 0.3f, Time.deltaTime * 0.3f));
            yield return null;
        }
    }

    IEnumerator Glitch()
    {
        yield return new WaitForSeconds(4);
        Transform text = lostCanvas.transform.GetChild(1);
        Vector3 savePos = text.position;
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3f, 10f));
            GetComponent<AudioSource>().clip = glitch;
            GetComponent<AudioSource>().pitch = Random.Range(0.75f, 3f);
            GetComponent<AudioSource>().Play();
            text.position += new Vector3(Random.Range(-50f, 50f), Random.Range(-50f, 50f), 0);
            yield return new WaitForSeconds(0.02f);
            text.position = savePos;
        }
    }

    void Update()
    {
        if (!isAttack)
        {
            Physics.Raycast(transform.position + 1.4f * Vector3.up, player.position - (transform.position + 1.4f * Vector3.up), out hit, Mathf.Infinity);
            if (hit.collider.tag == "Player" && !GameController.isGhost)
            {
                angle = Vector3.Angle(transform.TransformDirection(Vector3.forward), player.position - (transform.position + 1.1f * Vector3.up));
                if (angle < 70)
                {
                    if (Vector3.Distance(transform.position + 1.1f * Vector3.up, player.position) < (carArea[0].index == 0 ? 2f : 3f))
                    {
                        if (!isSitInPlace)
                            StartCoroutine(Attack());
                        else
                        {
                            StopCoroutine("Chase");
                            StartCoroutine("SearchPlayer");
                            return;
                        }
                    }
                    else
                        StopCoroutine("Chase");                                         // остановка всех коллизий для корректной работы
                    StopCoroutine("SearchPlayer");
                    StopCoroutine("SitInPlace");
                    StartCoroutine("Chase");                                            // начало преследования
                }
                else if (hit.distance < 3 * speedDifficulty)
                    SetDestination();                                                   // пассивный поворот, для проверки на присутвие игрока за спиной
            }
        }
        else
        {
            player.LookAt(head);
        }
    }

    IEnumerator Chase()
    {
        animator.speed = speedDifficulty;                                           // изменение скорости анимации
        agent.speed = 2f * speedDifficulty;                                          // изменение скорости персонажа
        playerCharacterController.enabled = true;
        heartbeat.OnEnemySee();
        if (!isSaw)
        {
            isSaw = true;
            GetComponent<AudioSource>().PlayOneShot(isScared ? scary_2 : scary);
            isScared = true;
        }
        Vector3 playerPos = player.position;                                        // сохранение последнего зафиксированного места игрока
        while (Vector3.Distance(playerPos, transform.position) > 2 && !isAttack)    // проверка на достижение последнего зафиксированного места игрока
        {
            if (isSitInPlace)
                yield break;
            if (!carArea[0].isThisPlace)
                SetDestination();                                                   // указание текущего местонахождения игрока
            yield return new WaitForSeconds(0.3f);                                  // ожидание для новой проверки
            playerCharacterController.enabled = true;
            if (carArea[1].index == 1 && Vector3.Distance(transform.position + 1.1f * Vector3.up, player.position) < 3.5f)          // если игрок в сундуке
            {
                StartCoroutine(Attack());
            }
        }
        yield return new WaitForSeconds(5);                                         // ожидание перед остановкой преследования
        if (playerCharacterController != null)
            playerCharacterController.enabled = true;
        if (!isAttack && Random.Range(0, 2) == 1)
            GetComponent<AudioSource>().PlayOneShot(laughter);
        StartCoroutine("SearchPlayer");                                             // переход в пассивное состояние поиска игрока
    }

    IEnumerator SearchPlayer()
    {
        animator.speed = 0.45f * speedDifficulty;                                   // изменение скорости анимации
        agent.speed = 0.7f * speedDifficulty;                                       // изменение скорости персонажа
        isSaw = false;
        heartbeat.OnEnemyLostPlayer();
        while (!isAttack)
        {
            agent.SetDestination(nulls[Random.Range(0, nulls.Length)].position);    // отправка случайного места, где потенциально может находиться игрок
            while (agent.remainingDistance > 2)                                     // проверка на достижение случайно выбранного места
                yield return new WaitForSeconds(0.3f);                              // ожидание для новой проверки
            yield return null;
        }
    }
}