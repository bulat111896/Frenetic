using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class ShopButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public PurchaseManager purchaseManager;
    public int index;
    Transform[] objects = new Transform[4];
    Image circle;

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.3f);
        StopAllCoroutines();
    }

    IEnumerator Anim(float alpha)
    {
        while (true)
        {
            circle.color = Color.Lerp(circle.color, new Color(1, 1, 1, alpha), Time.deltaTime * 25);
            yield return null;
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        StopAllCoroutines();
        StartCoroutine(Anim(0.35f));
        StartCoroutine(Wait());
    }

    public void OnPointerUp(PointerEventData data)
    {
        StopAllCoroutines();
        StartCoroutine(Anim(0));
        StartCoroutine(Wait());
    }

    public void OnPointerClick(PointerEventData data)
    {
        purchaseManager.BuyConsumable(index);
    }

    void Start()
    {
        for (int i = 0; i < 4; i++)
            objects[i] = transform.GetChild(i);
        circle = transform.GetChild(5).GetComponent<Image>();
    }

    void Update()
    {
        for (int i = 0; i < 4; i++)
            objects[i].Rotate(Vector3.forward * Time.deltaTime * 50 * (i % 2 == 0 ? 1 : -1));
    }
}
