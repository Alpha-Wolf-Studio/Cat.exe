using UnityEngine;
using UnityEngine.EventSystems;

public class UIEmptyIcon : MonoBehaviour, IDropHandler
{
    [SerializeField] private RectTransform holder = null;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            UiButtonEffect uiButton = eventData.pointerDrag.GetComponent<UiButtonEffect>();
            if (uiButton != null && uiButton.IsDraggeable)
            {
                int index = holder.GetSiblingIndex();
                holder.SetSiblingIndex(eventData.pointerDrag.transform.GetSiblingIndex());
                eventData.pointerDrag.transform.SetSiblingIndex(index);
            }
        }
    }
}
