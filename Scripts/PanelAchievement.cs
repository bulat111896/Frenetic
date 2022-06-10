using UnityEngine;
using UnityEngine.UI;

public class PanelAchievement : MonoBehaviour
{
    public GameObject debiting;
    public RectTransform content;
    public Text money;
    public bool isPanel;

    void Start()
    {
        content.position -= new Vector3(0, 1000, 0);
    }

    public void SetPos(bool isOpen)
    {
        if (!isPanel && isOpen)
        {
            isPanel = true;
            GetComponent<RectTransform>().localPosition -= new Vector3(0, 1000, 0);
        }
        else if (isPanel && !isOpen)
        {
            isPanel = false;
            GetComponent<RectTransform>().localPosition += new Vector3(0, 1000, 0);
        }
    }

    void SetRewardText(bool isEnglish)
    {
        if (PlayerPrefs.HasKey("Achievement"))
            content.GetChild(content.childCount - 1).GetComponent<Text>().text = isEnglish ? "The reward was received" : "Награда была получена";
        else
            content.GetChild(content.childCount - 1).GetComponent<Text>().text = isEnglish ? "Complete all the achievements and get 100 bats" : "Заверши все достижения и получи 100 летучих мышей";
    }

    public void SetData()
    {
        bool isEnglish = PlayerPrefs.GetInt("Language") == 0;
        string[] label = new string[8];
        string[] text = new string[8];
        int[] value = new int[8];
        int[] maxValue = new int[8];
        value[0] = PlayerPrefs.GetInt("Wins");
        maxValue[0] = 3;
        label[0] = isEnglish ? "WINNER" : "ПОБЕДИТЕЛЬ";
        text[0] = isEnglish ? "Escape from the house" : "Сбежать из дома маньяка";
        value[1] = PlayerPrefs.GetInt("HardMode");
        maxValue[1] = 1;
        label[1] = isEnglish ? "PRO" : "ПРОФИ";
        text[1] = isEnglish ? "Win in the \"Difficult\" mode" : "Победить в режиме \"Сложно\"";
        value[2] = PlayerPrefs.GetInt("Deaths");
        maxValue[2] = 10;
        label[2] = isEnglish ? "UNBREAKABLE" : "НЕРУШИМЫЙ";
        text[2] = isEnglish ? "Be killed by the hammer" : "Быть убитым молотом";
        value[3] = PlayerPrefs.GetInt("Kills");
        maxValue[3] = 5;
        label[3] = isEnglish ? "KILLER" : "КИЛЛЕР";
        text[3] = isEnglish ? "Shoot the maniac" : "Выстрелить в маньяка";
        value[4] = PlayerPrefs.GetInt("Auditions");
        maxValue[4] = 15;
        label[4] = isEnglish ? "MUSIC LOVER" : "МЕЛОМАН";
        text[4] = isEnglish ? "Play music on the radio" : "Включить музыку на радио";
        value[5] = PlayerPrefs.GetInt("TotalMinutes");
        maxValue[5] = 60;
        label[5] = isEnglish ? "GAMER" : "ИГРОМАН";
        text[5] = isEnglish ? "Play for a total of 60 minutes" : "Играть в сумме 60 минут";
        value[6] = PlayerPrefs.GetInt("Fix");
        maxValue[6] = 12;
        label[6] = isEnglish ? "PLUMBER" : "САНТЕХНИК";
        text[6] = isEnglish ? "Remove the siphon blockage" : "Устранить засор сифона";
        value[7] = PlayerPrefs.GetInt("Hacks");
        maxValue[7] = 7;
        label[7] = isEnglish ? "HACKER" : "ХАКЕР";
        text[7] = isEnglish ? "Crack the laptop password" : "Взломать пароль от ноутбука";
        for (int i = 0; i < value.Length; i++)
        {
            if (value[i] > 0)
            {
                content.GetChild(i).GetChild(0).GetComponent<Slider>().maxValue = maxValue[i];
                content.GetChild(i).GetChild(0).GetComponent<Slider>().value = value[i];
            }
            else
                content.GetChild(i).GetChild(0).GetChild(1).gameObject.SetActive(false);
            if (value[i] >= maxValue[i])
            {
                content.GetChild(i).GetChild(1).GetComponent<CanvasGroup>().alpha = 0.9f;
                PlayGames._instance.UnlockAchievement(i);
            }
            content.GetChild(i).GetChild(2).GetComponent<Text>().text = "◖ " + label[i] + " ◗\n_ _ _ _ _ _ _ _ _ _ _";
            content.GetChild(i).GetChild(3).GetComponent<Text>().text = text[i] + ((i == 1 || i == 5) ? "" : (" " + maxValue[i] + (isEnglish ? " times" : ((maxValue[i] > 1 && maxValue[i] < 5) ? " раза" : " раз"))));
        }
        SetRewardText(isEnglish);
        if (!PlayerPrefs.HasKey("Achievement"))
        {
            for (int i = 0; i < value.Length; i++)
                if (value[i] < maxValue[i])
                    return;
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + 100);
            PlayerPrefs.SetInt("Achievement", 1);
            money.text = PlayerPrefs.GetInt("Money").ToString();
            debiting.SetActive(false);
            debiting.SetActive(true);
            debiting.GetComponent<Text>().text = "+100";
            SetRewardText(isEnglish);
        }
    }
}