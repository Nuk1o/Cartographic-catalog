using DefaultNamespace.Pin;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class PinInfoView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _description;

        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _deleteButton;
        [SerializeField] private Button _editButton;

        private PinModel _model;
        private IPinService _service;
        private CreatePinWindow _createPinWindow;

        public void SetPinData(Sprite sprite, PinModel pinModel)
        {
            _image.sprite = sprite;
            _name.text = pinModel.Name;
            _description.text = pinModel.Description;
            _model = pinModel;
        }

        public void Initialization(IPinService pinService, CreatePinWindow pinWindow)
        {
            _service = pinService;
            _createPinWindow = pinWindow;
        }
        
        private void SubscribeButtons()
        {
            _closeButton.onClick.AddListener(Close);
            _deleteButton.onClick.AddListener(DeleteCurrentPin);
            _editButton.onClick.AddListener(OpenEditWindow);
        }

        private void UnsubscribeButtons()
        {
            _closeButton.onClick.RemoveListener(Close);
            _deleteButton.onClick.RemoveListener(DeleteCurrentPin);
            _editButton.onClick.RemoveListener(OpenEditWindow);
        }

        private void OnEnable()
        {
            SubscribeButtons();
        }

        private void OnDisable()
        {
            UnsubscribeButtons();
        }
        private void Close()
        {
            gameObject.SetActive(false);
        }

        private void DeleteCurrentPin()
        {
            _service.DeletePin(_model);
            Close();
        }

        private void OpenEditWindow()
        {
            _createPinWindow.gameObject.SetActive(true);
            _createPinWindow.SetEditMode(true);
            _createPinWindow.SetPinModel(_model);
            Close();
        }
        
        private void OnDestroy()
        {
            UnsubscribeButtons();
        }
    }
}