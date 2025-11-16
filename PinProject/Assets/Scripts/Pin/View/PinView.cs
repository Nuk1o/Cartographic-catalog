using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace.Pin
{
    public class PinView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler
    {
        [SerializeField] private Sprite _sprite;
        
        public Action<PinModel> OnPinEnter;
        public Action OnPinExit;

        private PinModel _model;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _model.Position = eventData.position;
            OnPinEnter.Invoke(_model);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnPinExit.Invoke();
        }

        public void Initialization(PinModel model)
        {
            _model = model;
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnPinExit.Invoke();
            gameObject.transform.position = eventData.position;
        }
    }
}