using UnityEngine;

namespace DefaultNamespace.Pin
{
    public class PinModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public string Description { get; set; }
        public Vector3 Position { get; set; }
        public Sprite Image { get; set; }

        public PinModel(string name, string imageURL, string description, Vector3 position, Sprite sprite = null)
        {
            Name = name;
            ImageURL = imageURL;
            Description = description;
            Image = sprite;
            Position = position;
        }

        public PinModel(int id, string name, string imageURL, string description, Vector3 position,
            Sprite sprite = null)
        {
            ID = id;
            Name = name;
            ImageURL = imageURL;
            Description = description;
            Image = sprite;
            Position = position;
        }
    }
}