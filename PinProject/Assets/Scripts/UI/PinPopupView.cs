using System.Threading;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Pin;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class PinPopupView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private PinInfoView _pinInfoView;

        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private Button _openDetailsButton;

        [Header("Animation")]
        [SerializeField] private RectTransform _windowRectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Vector2 _hidePosition = new Vector2(0, -500);
        [SerializeField] private float _animationDuration = 0.6f;
        [SerializeField] private Ease _showEase = Ease.OutBack;
        [SerializeField] private Ease _hideEase = Ease.InBack;

        private PinModel _pinModel;
        private CancellationTokenSource _hideDelayCancellation;

        public void InitializeWithPin(PinModel model)
        {
            _pinModel = model;
            _image.sprite = model.Image;
            _name.text = model.Name;
            _description.text = model.Description;
        }

        public void ShowPopup()
        {
            gameObject.SetActive(true);
            DOTween.KillAll();
            _canvasGroup.alpha = 1;

            gameObject.transform.position = new Vector3(_pinModel.Position.x - 200, _pinModel.Position.y - 100, 0);
        }

        public async UniTask HidePopup()
        {
            _hideDelayCancellation = new CancellationTokenSource();

            await UniTask.Delay(1000, cancellationToken: _hideDelayCancellation.Token);
            DOTween.KillAll();

            DOTween.Sequence()
                .AppendInterval(0.8f)
                .Append(_windowRectTransform.DOAnchorPos(_hidePosition, _animationDuration).SetEase(_hideEase))
                .Join(_canvasGroup.DOFade(0f, _animationDuration))
                .OnComplete(() => gameObject.SetActive(false))
                .Play();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            DOTween.KillAll();
            _hideDelayCancellation.Cancel();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HidePopup();
        }

        private void OpenPinDetails()
        {
            HidePopup();
            _pinInfoView.SetPinData(_pinModel.Image == null ? _image.sprite : _pinModel.Image, _pinModel);
            _pinInfoView.gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            _openDetailsButton.onClick.AddListener(OpenPinDetails);
        }

        private void OnDisable()
        {
            _openDetailsButton.onClick.RemoveListener(OpenPinDetails);
            _hideDelayCancellation?.Dispose(); 
        }
    }
}