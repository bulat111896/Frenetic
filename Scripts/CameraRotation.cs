using UnityEngine;
using UnityEngine.EventSystems;

public class CameraRotation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public static float sensitivity;
    public Transform cam, player;
    public static Vector2 lastPos, pos, savePos;

    void Start()
    {
        Remove();
    }

    public static void Remove()
    {
        lastPos = Vector2.zero;
        pos = Vector2.zero;
        savePos = Vector2.zero;
    }

    public void OnPointerDown(PointerEventData data)
    {
        savePos = data.position;                                                                         // сохранение местоположения первого касания, для вычитания из общей позиции
    }

    public void OnDrag(PointerEventData data)
    {
        pos = lastPos + (data.position - savePos) * (Fire.isAim ? 0.5f : 1) * sensitivity;               // расчёт корректной позиции касания
        Quaternion rotate = Quaternion.AngleAxis(pos.x, Vector3.up) *                                    // расчёт вращения по x
                            Quaternion.AngleAxis(Mathf.Clamp(pos.y, -80, 80), -Vector3.right);           // расчёт вращения по y
        player.eulerAngles = new Vector3(0, rotate.eulerAngles.y, 0);                                    // вращение персонажа по y
        cam.eulerAngles = new Vector3(rotate.eulerAngles.x, cam.eulerAngles.y, 0);                       // вращение камеры по x и z
    }

    public void OnPointerUp(PointerEventData data)
    {
        lastPos = pos;                                                                                  // сохранение местоположения последнего касания, для сложения с общей позицией
    }
}