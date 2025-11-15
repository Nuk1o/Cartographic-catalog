using DefaultNamespace.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace.Pin
{
    public class PinView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IPointerClickHandler
    {
        [SerializeField] private Sprite _sprite;
        private PinPopupView _pinPopupView;
        private PinModel _model;

        private void OnMouseEnter()
        {
            Debug.Log($"Test 2");
            // _pinPopup.ToggleShowWindow(true);
            // _pinPopup.SetDataInPin(_image.sprite,_name.text,_description.text);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _pinPopupView.ToggleShowWindow(true);
            _pinPopupView.SetDataInPin(_model.Image == null ? _sprite : _model.Image, _model.Name, _model.Description);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _pinPopupView.ToggleShowWindow(false);
        }

        public void Initialization(PinPopupView pinPopupView, PinModel model)
        {
            _pinPopupView = pinPopupView;
            _model = model;
        }

        public void OnDrag(PointerEventData eventData)
        {
            gameObject.transform.position = eventData.position;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"OPEN WINDOW");
        }
    }
}