using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityComponentUI.Engine
{
    public class MouseOverDetector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public UnityAction OnEnter { get; set; }
        public UnityAction OnLeave { get; set; }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnEnter?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnLeave?.Invoke();
        }
    }
}
