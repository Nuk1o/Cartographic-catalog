using DefaultNamespace.Map;
using DefaultNamespace.Pin;
using DefaultNamespace.UI;
using UnityEngine;

namespace DefaultNamespace
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private HUDController _hudController;
        [SerializeField] private PinPopupView _pinPopup;
        [SerializeField] private MapController _mapController;
        [SerializeField] private CreatePinWindow _pinWindow;

        [Space] [Header("Objects")]
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Transform _parent;

        private void Awake()
        {
            var pinStorage = new PinStorage();
            var pinService = new PinService();
            
            _hudController.Initialization(pinStorage);
            _pinWindow.Initialization(_prefab,_parent,pinService,_pinPopup);
            _mapController.Initialization(_pinWindow);
        }
    }
}
