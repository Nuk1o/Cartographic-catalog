using DefaultNamespace.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace.Map
{
    public class MapController : MonoBehaviour, IPointerClickHandler
    {
        private CreatePinWindow _pinWindow;

        public void Initialization(CreatePinWindow pinWindow)
        {
            _pinWindow = pinWindow;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Create pin on map");
            _pinWindow.gameObject.SetActive(true);
            _pinWindow.SetPosition(eventData.position);
        }
    }
}