using UnityEngine;

namespace DefaultNamespace.Pin
{
    public class PinStorage
    {
        public PinModel[] PinModels;

        public void SavePins()
        {
            Debug.Log("Save");
        }

        public void LoadingPins()
        {
            Debug.Log("Loading");
        }
    }
}