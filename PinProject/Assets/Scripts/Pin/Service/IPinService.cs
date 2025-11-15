using DefaultNamespace.UI;
using UnityEngine;

namespace DefaultNamespace.Pin
{
    public interface IPinService
    {
        void CreatePin(GameObject prefab, Vector3 position, Transform parent,PinModel models, PinPopupView popupView);
        void CreatePinTest();
    }
}