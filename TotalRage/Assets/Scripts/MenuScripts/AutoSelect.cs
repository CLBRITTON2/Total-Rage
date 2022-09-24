using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AutoSelect : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] Selectable selectable = null;
    public void OnPointerEnter(PointerEventData eventData)
    {
        selectable.Select();
    }
}