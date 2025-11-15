using System.Collections.Generic;
using DefaultNamespace.UI;
using UnityEngine;

namespace DefaultNamespace.Pin
{
    public class PinService : IPinService
    {
        public List<PinModel> PinModels { get; set; } = new List<PinModel>();

        public void CreatePin(GameObject prefab, Vector3 position, Transform parent, PinModel model,
            PinPopupView popupView)
        {
            var pin = Object.Instantiate(prefab, position, Quaternion.identity, parent);
            pin.GetComponent<PinView>().Initialization(popupView,model);
            PinModels.Add(model);
            Debug.Log($"List {PinModels.Count}");
        }

        public void CreatePinTest()
        {
            Debug.Log($"Test create");
        }
    }
}