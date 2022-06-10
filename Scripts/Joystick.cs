using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Transform cam, _null;
    public CharacterController characterController;
    public RectTransform aimDot, stick, center;
    public AudioClip[] step;
    AudioSource walk;
    Vector2 joystickPos;
    float distance, rect = 70;
    int stepNumber;
    public static bool isClick;
    bool isCame;

    IEnumerator LerpAimDot()
    {
        while (true)
        {
            if (aimDot.gameObject.activeSelf)
            {
                rect = Mathf.Lerp(rect, 65 + Mathf.Clamp(Mathf.Abs(joystickPos.x) + Mathf.Abs(joystickPos.y), 0, 0.01f) * 1500, 15 * Time.deltaTime);
                aimDot.sizeDelta = new Vector2(rect, rect);
            }
            yield return null;
        }
    }

    void Start()
    {
        isClick = false;
        walk = GetComponent<AudioSource>();
        distance = Screen.width * Screen.width / Screen.height * 0.06f * ((float)Screen.height / Screen.width);                                         // расчёт допустимого откланения от центра джостика в зависимости от размера экрана
        StartCoroutine(LerpAimDot());
    }

    void Update()
    {
        if (isClick)
        {
            characterController.Move((characterController.transform.forward * joystickPos.y                                                             // расчёт перемещения вперёд, назад
                                     + characterController.transform.right * joystickPos.x) * 0.9f * Time.deltaTime + new Vector3(0, -10f, 0) * Time.deltaTime);        // расчёт перемещения вправо, влево и вниз
            if (isCame)
            {
                cam.localPosition = Vector3.Lerp(cam.localPosition, new Vector3(0, 0.36f, 0), Time.deltaTime * 1.8f * characterController.velocity.magnitude);
                if (cam.localPosition.y > 0.33f)
                    isCame = false;
            }
            else
            {
                cam.localPosition = Vector3.Lerp(cam.localPosition, new Vector3(0, 0.24f, 0), Time.deltaTime * 2.2f * characterController.velocity.magnitude);
                if (cam.localPosition.y < 0.27f)
                {
                    isCame = true;
                    walk.PlayOneShot(step[stepNumber++]);
                    if (stepNumber == step.Length)
                        stepNumber = 0;
                }
            }
        }
        _null.localPosition = Vector3.Lerp(_null.localPosition, new Vector3(Fire.isAim ? -0.182f : 0, cam.localPosition.y + (Fire.isAim ? 0.06f : 0), _null.localPosition.z), Time.deltaTime * 50);
    }

    void Set(float scale, float opacity)
    {
        stick.localScale = Vector3.one * scale;                                                     // изменение размера джостика от нажатия
        stick.GetComponent<Image>().color = new Color(1, 1, 1, opacity);                            // изменение цвета цвета джостика
        GetComponent<Image>().color = new Color(1, 1, 1, opacity * 0.8f);                           // изменение цвета цвета заднего джостика
    }

    public void OnPointerDown(PointerEventData data)
    {
        isClick = true;
        Set(1.1f, 1);
    }

    public void OnDrag(PointerEventData data)
    {
        if (isClick)                                                                                // проверка, для устранения бага с паузой
        {
            if (Vector2.Distance(center.position, data.position) < distance)                        // провнерка на выход места касания за границы джостика
                stick.position = data.position;                                                     // измение положения джостика на место касания
            else
                stick.position = center.position + (new Vector3(data.position.x, data.position.y, 0) - center.position).normalized * distance;  // измение положения джостика если места касания за границами джостика
            joystickPos = (stick.position - center.position) / distance * 2.4f * (Fire.isAim ? 0.5f : 1) * (GameController.isRun ? (0.2f * PlayerPrefs.GetInt("Speed")) : 1f);    // сохранение позиции джостика
        }
    }

    public void OnPointerUp(PointerEventData data)
    {
        isClick = false;
        stick.position = center.position;                                                           // возвращение джостка в центр
        joystickPos = Vector2.zero;
        Set(1, 0.5f);
    }
}