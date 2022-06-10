using UnityEngine;
using UnityEngine.UI;

public class CarArea : MonoBehaviour
{
    public AudioClip open, close;
    public Transform player;
    public GameObject goToCarBut, joystick, run, ghost;
    public Sprite[] sprites;
    public int index;
    Vector3 lastPos;
    public bool isThisPlace;

    public void EnemyKill()
    {
        if (transform.parent.name == "Chest")
            player.position = lastPos;
        GetComponent<AudioSource>().PlayOneShot(close);
    }

    public void Click()
    {
        if (!isThisPlace)
            return;

        if (index == 0 && transform.parent.name == "Chest")
        {
            Transform Interactive = GameObject.Find("Interactive").transform;
            for (int i = 0; i < Interactive.childCount; i++)
            {
                if (Vector3.Distance(transform.position, Interactive.GetChild(i).position) < 0.3f)
                {
                    GameObject.Find("GameController").GetComponent<GameController>().InfoChest();
                    return;
                }
            }
            player.GetChild(0).localPosition = new Vector3(0, 0.3f, 0);
        }

        goToCarBut.GetComponent<Image>().sprite = sprites[index];
        if (index == 0)
        {
            lastPos = player.position;
            joystick.SetActive(false);
            run.SetActive(false);
            if (PlayerPrefs.GetInt("Difficulty") < 4)
                ghost.SetActive(false);
            player.GetComponent<CharacterController>().enabled = false;
            player.GetComponent<SphereCollider>().enabled = false;
            player.position = transform.position;

            GetComponent<AudioSource>().PlayOneShot(open);
        }
        else
        {
            joystick.SetActive(true);
            run.SetActive(true);
            if (PlayerPrefs.GetInt("Difficulty") < 4)
                ghost.SetActive(true);
            player.GetComponent<CharacterController>().enabled = true;
            player.GetComponent<SphereCollider>().enabled = true;
            player.position = lastPos;

            GetComponent<AudioSource>().PlayOneShot(close);
        }

        if (PlayerPrefs.GetInt("Difficulty") < 4)
            GameObject.Find("Enemy").GetComponent<Enemy>().StartSitInPlace(index);

        index = index == 0 ? 1 : 0;
    }

    void Start()
    {
        goToCarBut.SetActive(false);
    }

    void Update()
    {
        if (goToCarBut != null)
        {
            if (Vector3.Distance(transform.position, player.position) < (transform.parent.name == "Chest" ? 1.5f : 2.5f))
            {
                if (!goToCarBut.activeSelf)
                {
                    if (transform.parent.name == "Chest" && !GameController.isChestOpen)
                        return;
                    goToCarBut.GetComponent<Image>().sprite = sprites[index == 0 ? 1 : 0];
                    goToCarBut.SetActive(true);
                    isThisPlace = true;

                }
                else if (transform.parent.name == "Chest" && !GameController.isChestOpen)
                {
                    goToCarBut.SetActive(false);
                    isThisPlace = false;
                }
            }
            else if (Vector3.Distance(transform.position, player.position) < 5f)
            {
                if (goToCarBut.activeSelf)
                {
                    goToCarBut.SetActive(false);
                    isThisPlace = false;
                }
            }
        }
    }
}