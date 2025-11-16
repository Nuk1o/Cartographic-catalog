using MessagePack;
using UnityEngine;

namespace DefaultNamespace.Pin
{
    [MessagePackObject]
    public class PinData
    {
        [Key(0)] public int ID { get; set; }
        [Key(1)] public string Name { get; set; }
        [Key(2)] public string ImageURL { get; set; }
        [Key(3)] public string Description { get; set; }
        [Key(4)] public Vector3 Position { get; set; }
    }
}