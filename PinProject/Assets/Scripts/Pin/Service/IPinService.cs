using Cysharp.Threading.Tasks;

namespace DefaultNamespace.Pin
{
    public interface IPinService
    {
        void DeletePin(PinModel model);
        UniTask LoadPins();
        UniTask SavePins();
        void EditPin(PinModel model, string nameText, string imageURLText, string descriptionText);
    }
}