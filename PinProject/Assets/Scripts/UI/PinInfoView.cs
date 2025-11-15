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
        
        [SerializeField] private Button _exit;

        private void SetDataInPin(Sprite sprite, string name, string description)
        {
            _image.sprite = sprite;
            _name.text = name;
            _description.text = description;
        }

        private void OnEnable()
        {
            _exit.onClick.AddListener(ExitClick);
        }

        private void OnDisable()
        {
            _exit.onClick.RemoveListener(ExitClick);
        }

        private void ExitClick()
        {
            gameObject.SetActive(false);
        }
    }
}