using System.Collections.Generic;
using DefaultNamespace.Pin;
using DefaultNamespace.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace.Map
{
    public class MapView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private PinView _pinPrefab;
        [SerializeField] private Transform _pinContainer;
        [SerializeField] private CreatePinWindow _pinWindow;
        [SerializeField] private PinPopupView _pinPopupView;

        private readonly Dictionary<int, PinView> _pins = new();

        public void OnPointerClick(PointerEventData eventData)
        {
            _pinWindow.gameObject.SetActive(true);
            _pinWindow.SetEditMode(false);
            _pinWindow.SetPosition(eventData.position);
        }

        public void ShowPin(PinModel model)
        {
            if (_pins.ContainsKey(model.ID))
            {
                Debug.LogWarning($"Метка с ID {model.ID} уже существует. ");
                return;
            }
            
            var pin = Instantiate(_pinPrefab, model.Position, Quaternion.identity, _pinContainer);
            pin.Initialization(model);
            pin.OnPinEnter += ShowPopup;
            pin.OnPinExit += HidePopup;
            _pins.Add(model.ID, pin);
        }

        public void ClearAllPins()
        {
            foreach (var pin in _pins.Values)
            {
                pin.OnPinEnter -= ShowPopup;
                pin.OnPinExit -= HidePopup;
                Destroy(pin.gameObject);
            }
    
            _pins.Clear();
        }

        public void HidePin(int id)
        {
            if (!_pins.TryGetValue(id, out var pin))
                return;
            pin.OnPinEnter -= ShowPopup;
            pin.OnPinExit -= HidePopup;
            Destroy(pin.gameObject);
            _pins.Remove(id);
        }

        private void ShowPopup(PinModel model)
        {
            _pinPopupView.InitializeWithPin(model);
            _pinPopupView.ShowPopup();
        }

        private void HidePopup()
        {
            _pinPopupView.HidePopup();
        }
    }
}