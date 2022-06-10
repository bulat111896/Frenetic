using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class Language : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public AudioSource _audio;
    public Sprite[] sprites;
    public Text[] texts;
    public Dropdown dropdown;
    public PanelAchievement panelAchievement;
    public LevelingUp levelingUpGhostTime, levelingUpSpeed;
    Image parent;
    public bool isLevel;

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
        StopAllCoroutines();
    }

    IEnumerator Anim(float scale, float alpha)
    {
        while (true)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(scale, scale, 1), Time.deltaTime * 20);
            parent.color = Color.Lerp(parent.color, new Color(1, 1, 1, alpha), Time.deltaTime * 20);
            yield return null;
        }
    }

    void SetLanguage()
    {
        GetComponent<Image>().sprite = sprites[PlayerPrefs.GetInt("Language")];
        if (PlayerPrefs.GetInt("Language") == 0)
        {
            if (isLevel)
            {
                texts[0].text = "Settings";
                texts[1].text = "Sensitivity";
                texts[2].text = "Shooting mode";
                texts[3].text = "Home";
                texts[4].text = "Replay";
                texts[5].text = "Continue";
                texts[6].text = "You didn't understand my intentions. Don't try to stop me. By destroying everything, I will enable another life form to create a perfect civilization.";
                texts[6].fontSize = 35;
                dropdown.options[0].text = "Pressing";
                dropdown.options[1].text = "Release";
            }
            else
            {
                texts[0].text = "Play";
                texts[1].text = "Settings";
                texts[2].text = "Shop";
                texts[3].text = "Settings\n_________________________";
                texts[4].text = "Game mode";
                texts[5].text = "Quit the game?\n_________________________";
                texts[6].text = "Yes";
                texts[7].text = "No";
                texts[8].text = "Click to go to Google Play";
                texts[11].text = "Fog";
                texts[12].text = "Show FPS";
                texts[14].text = "Purchase completed successfully!";
                texts[15].text = "Achievements\n_____________________________";
                dropdown.options[0].text = "Easy";
                dropdown.options[1].text = "Normal";
                dropdown.options[2].text = "Hard";
                dropdown.options[3].text = "Extreme";
                dropdown.options[4].text = "Practice";
                texts[16].text = "$0.99";
                texts[17].text = "$1.49";
                texts[18].text = "$2.49";
                texts[19].text = "$3.49";
                texts[21].text = "Watch the video and get 2 bats";
                texts[22].text = "About bats\n_______________________";
                texts[23].text = "\t\tBats can be obtained by completing the game in different difficulty modes\n\t\t• Easy – 2 pcs;\n\t\t• Normal – 3 pcs;\n\t\t• Difficult – 5 pcs;\n\t\t• Extreme – 10 pcs;\n\t\t• Training – 1 pc.\n\t\tThey are also issued for completing all tasks, viewing ads, or being purchased in the store.\n\t\tFor bats, you can buy the ability to become a ghost for 5 seconds.";
                texts[24].text = "Upgrade";
                texts[25].text = "Not enough bats!";
                texts[26].text = "Speed";
            }
        }
        else
        {
            if (isLevel)
            {
                texts[0].text = "Настройки";
                texts[1].text = "Чувствительность";
                texts[2].text = "Режим стрельбы";
                texts[3].text = "Главная";
                texts[4].text = "Переиграть";
                texts[5].text = "Продолжить";
                texts[6].text = "Ты не понял моих намерений. Не пытайся меня остановить. Разрушив всё, я позволю другой форме жизни создать совершенную цивилизацию.";
                texts[6].fontSize = 33;
                dropdown.options[0].text = "Нажатие";
                dropdown.options[1].text = "Отпускание";
            }
            else
            {
                texts[0].text = "Играть";
                texts[1].text = "Настройки";
                texts[2].text = "Магазин";
                texts[3].text = "Настройки\n_________________________";
                texts[4].text = "Режим игры";
                texts[5].text = "Выйти из игры?\n_________________________";
                texts[6].text = "Да";
                texts[7].text = "Нет";
                texts[8].text = "Нажми, чтобы перейти в Google Play";
                texts[11].text = "Туман";
                texts[12].text = "Отображать FPS";
                texts[14].text = "Покупка успешно совершена!";
                texts[15].text = "Достижения\n_____________________________";
                dropdown.options[0].text = "Легко";
                dropdown.options[1].text = "Нормально";
                dropdown.options[2].text = "Сложно";
                dropdown.options[3].text = "Экстремально";
                dropdown.options[4].text = "Тренировка";
                texts[16].text = "50₽";
                texts[17].text = "99₽";
                texts[18].text = "199₽";
                texts[19].text = "249₽";
                texts[21].text = "Посмотреть видео и получить 2 летучей мыши";
                texts[22].text = "О летучих мышах\n_______________________";
                texts[23].text = "\t\tЛетучих мышей можно получить, пройдя игру на разных режимах сложности:\n\t\t• Легко – 2 шт;\n\t\t• Нормально – 3 шт;\n\t\t• Сложно – 5 шт;\n\t\t• Экстремально – 10 шт;\n\t\t• Тренировка – 1 шт.\n\t\tТакже они выдаются за выполнение всех заданий, просмотр рекламы или приобретаются в магазине.\n\t\tЗа летучих мышей можно купить возможность становиться призраком на 5 секунд.";
                texts[24].text = "Улучшить";
                texts[25].text = "Не хватает летучих мышей!";
                texts[26].text = "Скорость";
            }
        }
        if (!isLevel)
        {
            texts[9].text = (PlayerPrefs.GetInt("Language") == 0 ? "BEST TIME: " : "ЛУЧШЕЕ ВРЕМЯ: ") + (PlayerPrefs.HasKey("BestTime") ? PlayerPrefs.GetString("BestTime") : "--:--:---");
            texts[20].text = texts[2].text;
            panelAchievement.SetData();
            levelingUpGhostTime.SetGhostTimeText();
            levelingUpSpeed.SetSpeedText();
        }
        dropdown.captionText.text = dropdown.options[dropdown.value].text;
    }

    void Start()
    {
        parent = transform.parent.GetComponent<Image>();

        if (!isLevel)
            if (!PlayerPrefs.HasKey("Language"))
            {
                if (Application.systemLanguage == SystemLanguage.Russian)
                    PlayerPrefs.SetInt("Language", 1);
                else
                    PlayerPrefs.SetInt("Language", 0);
            }
        SetLanguage();
    }

    public void OnPointerDown(PointerEventData data)
    {
        StopAllCoroutines();
        StartCoroutine(Anim(1.1f, 0));
        StartCoroutine(Wait());
    }

    public void OnPointerUp(PointerEventData data)
    {
        StopAllCoroutines();
        StartCoroutine(Anim(1, 0.7f));
        StartCoroutine(Wait());
    }

    public void OnPointerClick(PointerEventData data)
    {
        PlayerPrefs.SetInt("Language", PlayerPrefs.GetInt("Language") == 0 ? 1 : 0);
        if (PlayerPrefs.GetInt("Sound") == 1 && _audio != null)
            _audio.Play();
        SetLanguage();
    }
}