using UnityEngine;
using UnityEngine.EventSystems;

public class Cursor : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public NotebookController notebook;

    public void OnPointerDown(PointerEventData data)
    {
        notebook.OnPointerDown(data);
    }

    public void OnDrag(PointerEventData data)
    {
        notebook.OnDrag(data);
    }

    public void OnPointerUp(PointerEventData data)
    {
        notebook.OnPointerUp(data);
    }
}