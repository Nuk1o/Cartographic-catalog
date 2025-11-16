using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Map;
using DefaultNamespace.UI;

namespace DefaultNamespace.Pin
{
    public class PinService : IPinService
    {
        private readonly CreatePinWindow _pinWindow;
        private readonly MapView _mapView;
        private readonly PinStorage _pinStorage;
        private List<PinModel> _pinModels { get; set; } = new ();
        private int _nextPinId = 1;

        public PinService(CreatePinWindow pinWindow, MapView mapView, PinStorage storage)
        {
            _pinWindow = pinWindow;
            _pinStorage = storage;
            _mapView = mapView;

            _pinWindow.OnCreatePin += CreatePin;
            InitializeNextPinId();
        }
        
        private void InitializeNextPinId()
        {
            if (_pinModels.Count > 0)
            {
                _nextPinId = _pinModels.Max(m => m.ID) + 1;
            }
        }

        private void CreatePin(PinModel model)
        {
            model.ID = _nextPinId++;
            _pinModels.Add(model);
            ShowPin(model);
        }

        private void ShowPin(PinModel model)
        {
            _mapView.ShowPin(model);
        }

        public void DeletePin(PinModel model)
        {
            _mapView.HidePin(model.ID);
            _pinModels.Remove(model);
        }

        public async UniTask LoadPins()
        {
            _pinModels = new List<PinModel>();
            _mapView.ClearAllPins();
            _pinModels.Clear();
            _pinModels = await _pinStorage.LoadPins();
            _nextPinId = _pinModels.Count > 0 
                ? _pinModels.Max(m => m.ID) + 1 
                : 1;
            foreach (var model in _pinModels)
            {
                ShowPin(model);
            }
        }

        public async UniTask SavePins()
        {
            await _pinStorage.SavePins(_pinModels);
        }

        public async void EditPin(PinModel model, string nameText, string imageURLText, string descriptionText)
        {
            model.Name = nameText;
            model.Description = descriptionText;
            model.ImageURL = imageURLText;
            var sprite = await _pinStorage.LoadSpriteAsync(imageURLText);
            model.Image = sprite;
        }
    }
}