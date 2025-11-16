using DefaultNamespace.Map;
using DefaultNamespace.Pin;
using DefaultNamespace.UI;
using UnityEngine;

namespace DefaultNamespace
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private HUDController _hudController;
        [SerializeField] private PinPopupView _pinPopup;
        [SerializeField] private MapView _mapView;
        [SerializeField] private CreatePinWindow _pinWindow;
        [SerializeField] private PinInfoView _pinInfoView;

        [Space]
        [Header("Objects")]
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Transform _parent;

        private async void Awake()
        {
            var pinStorage = new PinStorage();
            var pinService = new PinService(_pinWindow,_mapView,pinStorage);

            _hudController.Initialization(pinService);
            _pinWindow.Initialization(pinStorage,pinService);
            _pinInfoView.Initialization(pinService,_pinWindow);

            await pinStorage.LoadLocalImageCache();
            await pinService.LoadPins();
        }
    }
}
