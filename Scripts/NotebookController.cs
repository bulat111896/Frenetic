using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class NotebookController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform cursor;
    public Cell[] images;
    public GameObject[] panels;
    Text text;
    public bool isUSB;
    bool isAllGood;

    public void OnPointerDown(PointerEventData data)
    {
        cursor.sizeDelta = new Vector2(45, 45);
        cursor.position = data.position + new Vector2(7, -17);
    }

    public void OnDrag(PointerEventData data)
    {
        cursor.position = data.position + new Vector2(7, -17);
    }

    public void OnPointerUp(PointerEventData data)
    {
        cursor.sizeDelta = new Vector2(40, 40);
        DisableAllCells();
    }

    public void OpenWindow(int index)
    {
        for (int i = 0; i < panels.Length; i++)
            panels[i].SetActive(false);
        panels[index].SetActive(true);
    }

    public void DisableAllCells()
    {
        for (int i = 0; i < images.Length; i++)
            images[i].Disable();
    }

    IEnumerator Cmd()
    {
        for (int i = 0; i < 3; i++)
        {
            text.text = "▐";
            yield return new WaitForSeconds(0.3f);
            text.text = "";
            yield return new WaitForSeconds(0.3f);
        }
        text.text = "Maniac Corporation [Version 10.2.19352.1729]";
        yield return new WaitForSeconds(0.3f);
        text.text += "\n\nprogram start >>\n\n";
        yield return new WaitForSeconds(0.3f);
        text.text += "One-time hash:\t\t\t\t\t";
        string chars = "0123456789abcdefghijklmnopqrstuvwxyz";
        for (int i = 0; i < 32; i++)
        {
            text.text += chars[Random.Range(0, chars.Length)];
        }
        text.text += "\nIPv4:\t\t\t\t\t\t\t131.89.13.224\nLogin:\t\t\t\t\t\t\t616\nPassword:\t\t\t\t\t\tadmin\nfirst_name:\t\t\t\t\t\tJack\nlast_name:\t\t\t\t\t\tNicholson\n";
        for (int i = 0; i < 25; i++)
        {
            text.text += ". ";
            yield return new WaitForSeconds(Random.Range(0f, 0.1f));
        }
        text.text += isUSB ? "Connection established\n\n" : "Connection error\n\nno external devices found, the program will end in 5 seconds...";
        if (!isUSB)
        {
            for (int i = 4; i > -1; i--)
            {
                yield return new WaitForSeconds(1);
                text.text = text.text.Substring(0, text.text.Length - 12) + i + " seconds...";
            }
            text.text += "\n\n\nreturn 0";
        }
        else
        {
            string saveText = text.text, progress = "";
            int[] testedPasswords = new int[101];
            int testedPasswordsLength = 0;
            for (int i = 1; i < 101; i++)
            {
                testedPasswordsLength += Random.Range(100, 700);
                testedPasswords[i] = testedPasswordsLength;
            }
            for (int i = 0; i < 20; i++)
            {
                progress += "░";
            }
            for (int i = 0; i < 101; i++)
            {
                yield return new WaitForSeconds((Random.Range(0, 30) % 20 == 0) ? Random.Range(0.1f, 0.2f) : Random.Range(0f, 0.05f));
                if (i % 5 == 0)
                    progress = "▓" + progress.Substring(0, progress.Length - 1);
                text.text = saveText + (i < 10 ? "  " + i : (i < 100 ? " " + i : i.ToString())) + "% " + progress + "\t|\t(" + testedPasswords[i] + "/" + testedPasswordsLength + " tested passwords)";
            }
            text.text += "\n\n\nreturn 1";
        }
        for (int i = 0; i < 3; i++)
        {
            text.text += "▐";
            yield return new WaitForSeconds(0.2f);
            text.text = text.text.Substring(0, text.text.Length - 1);
            yield return new WaitForSeconds(0.2f);
        }
        if (isUSB)
        {
            isAllGood = true;
            PlayerPrefs.SetInt("Hacks", PlayerPrefs.GetInt("Hacks") + 1);
            CanvasGroup line = transform.GetChild(1).GetComponent<CanvasGroup>();
            float a = 0;
            bool isSetActive = false;
            while (a < 0.99f)
            {
                a = Mathf.Lerp(a, 1, Time.deltaTime * 5);
                GetComponent<Image>().color = new Color(a, a, a);
                line.alpha = 1 - a;
                if (a > 0.3f && !isSetActive)
                {
                    isSetActive = true;
                    transform.GetChild(0).gameObject.SetActive(false);
                    transform.GetChild(1).gameObject.SetActive(false);
                    StartCoroutine(Wait());
                }
                yield return null;
            }
            GetComponent<Image>().color = new Color(1, 1, 1);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator Wait()
    {
        CanvasGroup cells = transform.GetChild(2).GetComponent<CanvasGroup>();
        CanvasGroup butOff = transform.GetChild(3).GetComponent<CanvasGroup>();
        Image cursor = transform.GetChild(transform.childCount - 1).GetComponent<Image>();
        cells.blocksRaycasts = true;
        butOff.blocksRaycasts = true;
        while (cells.alpha < 1)
        {
            cells.alpha += Time.deltaTime * 3;
            butOff.alpha = cells.alpha;
            cursor.color = new Color(0.1f, 0.1f, 0.1f, cells.alpha);
            yield return null;
        }
    }

    void OnEnable()
    {
        if (!isAllGood)
            StartCoroutine(Cmd());
    }

    void Awake()
    {
        text = transform.GetChild(0).GetComponent<Text>();
    }
}