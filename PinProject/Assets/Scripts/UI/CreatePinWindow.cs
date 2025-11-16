using System;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Pin;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class CreatePinWindow : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _nameInput;
        [SerializeField] private TMP_InputField _imageUrlInput;
        [SerializeField] private TMP_InputField _descriptionInput;
        [SerializeField] private Button _saveButton;
        
        [SerializeField] private RectTransform _panel;
        [SerializeField] private CanvasGroup _panelCanvasGroup;
        private Vector2 _originalPosition;

        private IPinService _pinService;
        private PinStorage _pinStorage;
        private PinModel _pinModel;
        private Vector3 _position;

        private bool _isEditMode;

        public Action<PinModel> OnCreatePin;
        
        private void Awake()
        {
            if (_panel != null)
            {
                _originalPosition = _panel.GetComponent<RectTransform>().anchoredPosition;
            }
        }

        public void Initialization(PinStorage storage, IPinService service)
        {
            _pinService = service;
            _pinStorage = storage;
        }

        public void SetPosition(Vector3 position)
        {
            _position = position;
        }

        public void SetEditMode(bool isEditMode)
        {
            _isEditMode = isEditMode;
        }

        public void SetPinModel(PinModel model)
        {
            _pinModel = model;

            if (!_isEditMode)
                return;

            _nameInput.text = model.Name;
            _imageUrlInput.text = model.ImageURL;
            _descriptionInput.text = model.Description;
        }

        private void HandleSaveButtonClick()
        {
            if (_isEditMode)
            {
                EditPin(_pinModel);
            }
            else
            {
                SavePin();
            }
        }

        private async UniTask SavePin()
        {
            if (ValidateInputs())
            {
                var image = await _pinStorage.LoadSpriteAsync(_imageUrlInput.text);
                var model = new PinModel(_nameInput.text, _imageUrlInput.text, _descriptionInput.text, _position,
                    image);
                OnCreatePin.Invoke(model);
            }
            else
            {
                Debug.Log("Введите все данные!");
            }

            gameObject.SetActive(false);
        }

        private void EditPin(PinModel model)
        {
            if (ValidateInputs())
            {
                _pinService.EditPin(model, _nameInput.text, _imageUrlInput.text, _descriptionInput.text);
            }
            else
            {
                Debug.Log("Введите все данные!");
            }

            gameObject.SetActive(false);
        }

        private bool ValidateInputs() =>
            _nameInput.text.Length > 0 && _imageUrlInput.text.Length > 0 && _descriptionInput.text.Length > 0;

        private void OnEnable()
        {
            _saveButton.onClick.AddListener(HandleSaveButtonClick);
            ResetInputFields();
            AnimatePanelIn();
        }
        
        private void AnimatePanelIn()
        {
            _panel.anchoredPosition = _originalPosition + new Vector2(0, -150);
            _panelCanvasGroup.alpha = 0f;
            _panelCanvasGroup.blocksRaycasts = false;

            var panelSequence = DOTween.Sequence();
        
            panelSequence
                .Append(_panel.DOAnchorPos(_originalPosition, 0.6f)
                    .SetEase(Ease.OutQuint))
                .Join(_panelCanvasGroup.DOFade(1f, 0.5f))
                .OnComplete(() => _panelCanvasGroup.blocksRaycasts = true)
                .SetLink(gameObject);
        }

        private void ResetInputFields()
        {
            _nameInput.text = string.Empty;
            _imageUrlInput.text = string.Empty;
            _descriptionInput.text = string.Empty;
        }

        private void OnDisable()
        {
            _saveButton.onClick.RemoveListener(HandleSaveButtonClick);
        }
    }
}