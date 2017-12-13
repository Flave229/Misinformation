using Assets.Scripts.EventSystem;
using UnityEngine;
using Event = Assets.Scripts.EventSystem.Event;

namespace Assets.Scripts
{
    public class InputManager
    {
        public bool m_Button1 = false;
        public bool m_Button2 = false;
        public bool m_Button3 = false;
        public bool m_Button4 = false;
        public bool m_ClickLeft = false;
        public bool m_ClickRight = false;

        public float m_DirectionHorizontal = 0.0f;
        public float m_DirectionVertical = 0.0f;
    
        public Vector2 m_MouseClickLocation = new Vector2(0.0f, 0.0f);

        private float _defaultPlaceCooldown = 1.0f;
        private float _currentPlaceCooldown = 0.0f;
        private static InputManager _instance;

        private InputManager()
        {
        }

        public static InputManager Instance()
        {
            if (_instance == null)
                _instance = new InputManager();

            return _instance;
        }

        void Start ()
        {
        }
	
        public void Update ()
        {
            m_Button1 = Input.GetButtonDown("Button1");
            m_Button2 = Input.GetButtonDown("Button2");
            m_Button3 = Input.GetButtonDown("Button3");
            m_Button4 = Input.GetButtonDown("Button4");
            m_ClickLeft = Input.GetButtonDown("Fire1");
            m_ClickRight = Input.GetButtonDown("Fire2");
        
            m_DirectionHorizontal = Input.GetAxis("Horizontal");
            m_DirectionVertical   = Input.GetAxis("Vertical");

            if (m_ClickLeft)
            {
                m_MouseClickLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                EventMessenger.Instance().FireEvent(Event.LEFT_MOUSE_CLICK, m_MouseClickLocation);
            }

            if (m_ClickRight)
                m_MouseClickLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (_currentPlaceCooldown > 0.0f)
            {
                _currentPlaceCooldown -= Time.deltaTime;
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                /* _currentPlaceCooldown = _defaultPlaceCooldown;
            GameObject technician = GameObject.FindGameObjectWithTag("Player");*/
            }
        }
    }
}