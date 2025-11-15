using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class PinPopupView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private Button _open;
        
        public void SetDataInPin(Sprite sprite, string name, string description)
        {
            _image.sprite = sprite;
            _name.text = name;
            _description.text = description;
        }

        public void ToggleShowWindow(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        private void OnEnable()
        {
            _open.onClick.AddListener(OpenWindow);
        }

        private void OnDisable()
        {
            _open.onClick.RemoveListener(OpenWindow);
        }

        private void OpenWindow()
        {
            
        }
    }
}