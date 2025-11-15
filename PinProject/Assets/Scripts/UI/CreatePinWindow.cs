using DefaultNamespace.Pin;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class CreatePinWindow : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _name;
        [SerializeField] private TMP_InputField _imageURL;
        [SerializeField] private TMP_InputField _description;

        [SerializeField] private Button _save;

        private IPinService _pinService;
        private GameObject _prefab;
        private Transform _transform;
        private Vector3 _position;
        private PinPopupView _pinPopupView;

        public void Initialization(GameObject prefab, Transform parent, IPinService service, PinPopupView pinPopup)
        {
            _pinService = service;
            _prefab = prefab;
            _transform = parent;
            _pinPopupView = pinPopup;
        }
        
        public void SetPosition(Vector3 position)
        {
            _position = position;
        }

        private void SavePin()
        {
            if (CheckingInputs())
            {
                Sprite image = null; //TODO Create loading sprite
                var model = new PinModel(_name.text,_imageURL.text,_description.text, image);
                _pinService.CreatePin(_prefab,_position,_transform,model,_pinPopupView);
            }
            else
            {
                Debug.Log("Введите все данные!");
            }
            gameObject.SetActive(false);
        }

        private bool CheckingInputs() => _name.text.Length > 0 || _imageURL.text.Length > 0 || _description.text.Length > 0;

        private void OnEnable()
        {
            _save.onClick.AddListener(SavePin);
        }

        private void OnDisable()
        {
            _save.onClick.RemoveListener(SavePin);
        }
    }
}