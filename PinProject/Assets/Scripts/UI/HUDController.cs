using DefaultNamespace.Pin;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] private Button _exitApp;
        [SerializeField] private Button _loadingPins;
        [SerializeField] private Button _savePins;

        private PinStorage _pinStorage;

        public void Initialization(PinStorage pinStorage)
        {
            _pinStorage = pinStorage;
        }

        private void Start()
        {
            _exitApp.onClick.AddListener(ExitApp);
            _loadingPins.onClick.AddListener(LoadingPins);
            _savePins.onClick.AddListener(SavePins);
        }

        private void ExitApp()
        {
            Application.Quit();
        }
        
        private void LoadingPins()
        {
            _pinStorage.LoadingPins();
        }
        
        private void SavePins()
        {
            _pinStorage.SavePins();
        }

        private void OnDestroy()
        {
            _exitApp.onClick.RemoveListener(ExitApp);
            _loadingPins.onClick.RemoveListener(LoadingPins);
            _savePins.onClick.RemoveListener(SavePins);
        }
    }
}