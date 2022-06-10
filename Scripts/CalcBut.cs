using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class CalcBut : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Calculator calc;
    public char character;
    Image image;
    Color[] color = new Color[2];

    IEnumerator Anim(int range)
    {
        while (image.pixelsPerUnitMultiplier != range)
        {
            image.pixelsPerUnitMultiplier = Mathf.Lerp(image.pixelsPerUnitMultiplier, range, Time.deltaTime * 30);
            image.SetVerticesDirty();
            image.color = Color.Lerp(image.color, color[range == 1 ? 1 : 0], Time.deltaTime * 20);
            transform.localScale = Vector3.Lerp(transform.localScale, range == 1 ? new Vector3(0.9f, 0.9f, 1) : Vector3.one, Time.deltaTime * 15);
            yield return null;
        }
    }

    void Start()
    {
        image = GetComponent<Image>();
        color[0] = image.color;
        color[1] = color[0] + new Color(0.1f, 0.1f, 0.1f, 0.1f);
    }

    public void OnPointerDown(PointerEventData data)
    {
        StopAllCoroutines();
        StartCoroutine(Anim(1));
    }

    public void OnPointerUp(PointerEventData data)
    {
        StopAllCoroutines();
        StartCoroutine(Anim(25));
        calc.Add(character);
    }
}
