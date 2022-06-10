using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Image filled;
    public GameController gameController;

    IEnumerator FillAnimation()
    {
        while (true)
        {
            filled.fillAmount += Time.deltaTime * 2;
            if (filled.fillAmount == 1)
            {
                gameController.Drop();
                OnPointerUp(null);
            }
            yield return null;
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (gameController.ActiveThing > -1)
            StartCoroutine(FillAnimation());
        GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 0.4f);
    }

    public void OnPointerUp(PointerEventData data)
    {
        StopAllCoroutines();
        filled.fillAmount = 0;
        GetComponent<Image>().color = new Color(0.05f, 0.05f, 0.05f, 0.6f);
    }
}