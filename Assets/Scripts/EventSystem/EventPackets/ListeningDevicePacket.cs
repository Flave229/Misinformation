using UnityEngine;

namespace Assets.Scripts.EventSystem.EventPackets
{
    public class ListeningDevicePacket
    {
        public ListeningDevice Device { get; set; }
        public Technician TechnicianListening { get; set; }
        public int Num { get; set; }
    }
}