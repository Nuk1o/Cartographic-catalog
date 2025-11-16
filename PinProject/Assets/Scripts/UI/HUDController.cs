using DefaultNamespace.Pin;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _loadPinsButton;
        [SerializeField] private Button _savePinsButton;

        private IPinService _pinService;

        public void Initialization(IPinService pinService)
        {
            _pinService = pinService;
        }

        private void Start()
        {
            SubscribeButtons();
        }

        private void OnEnable()
        {
            SubscribeButtons();
        }
        
        private void OnDisable()
        {
            UnsubscribeButtons();
        }

        private void SubscribeButtons()
        {
            _exitButton.onClick.AddListener(QuitApplication);
            _loadPinsButton.onClick.AddListener(LoadingPins);
            _savePinsButton.onClick.AddListener(SavePins);
        }

        private void UnsubscribeButtons()
        {
            _exitButton.onClick.RemoveListener(QuitApplication);
            _loadPinsButton.onClick.RemoveListener(LoadingPins);
            _savePinsButton.onClick.RemoveListener(SavePins);
        }

        private void LoadingPins()
        {
            _pinService.LoadPins();
        }

        private void SavePins()
        {
            _pinService.SavePins();
        }

        private void OnDestroy()
        {
            UnsubscribeButtons();
        }

        private static void QuitApplication()
        {
            Application.Quit();
        }
    }
}