using UnityEngine;

namespace Assets.Scripts
{
    public class Room : MonoBehaviour
    {
        public int  m_Depth;
        public Door m_DoorDown;
        public Door m_DoorUp;
        public bool m_Accessible = true;

        // Use this for initialization
        void Start ()
        {
        }
	
        // Update is called once per frame
        void FixedUpdate ()
        {
		
        }
    }
}
