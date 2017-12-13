using UnityEngine;

namespace Assets.Scripts.EventSystem.EventPackets
{
    class ListeningDevicePlacedPacket
    {
        public Room PlacedRoom { get; set; }
        public GameObject Device { get; set; }
    }
}
