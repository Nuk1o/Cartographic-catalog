using UnityEngine;

namespace DefaultNamespace.Pin
{
    public class PinModel
    {
        public PinModel(string name, string imageURL, string description, Sprite sprite = null)
        {
            Name = name;
            ImageURL = imageURL;
            Description = description;
            Image = sprite;
        }

        public string Name { get; set; }
        public string ImageURL { get; set; }
        public string Description { get; set; }
        public Sprite Image { get; set; }
    }
}