using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Cell : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public NotebookController screen;
    Image image;
    bool isStartWait, isSecondClick;

    public void Disable()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
        isStartWait = false;
        if (isSecondClick)
        {
            isSecondClick = false;
            if (gameObject.name == "Folder")
                screen.OpenWindow(0);
            else if (gameObject.name == "Calc")
                screen.OpenWindow(1);
            else if(gameObject.name == "Chrome")
                screen.OpenWindow(2);
        }
    }

    void Start()
    {
        image = transform.GetChild(0).GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        screen.DisableAllCells();
        screen.OnPointerDown(eventData);
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.1f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        screen.OnPointerUp(eventData);
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.25f);
        if (!isStartWait)
        {
            isStartWait = true;
            StartCoroutine(Wait());
        }
        else
            isSecondClick = true;
    }
}