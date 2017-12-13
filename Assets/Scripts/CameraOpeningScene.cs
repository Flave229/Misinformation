using UnityEngine;

namespace Assets.Scripts
{
    public class CameraOpeningScene : MonoBehaviour
    {
        public GameObject       m_MainCamera;

        private Camera2DFollow  m_CamFollow;
        private Camera          m_Camera;

        public float    m_TimeToLerp = 10.0f;
        public Vector3  m_FinalPosition;
        public float    m_FinalFOV;

        public float    m_MoveSpeed = 0.5f;
        public float    m_FovSpeed = 0.5f;

        void Awake ()
        {
            m_Camera = transform.GetComponent<Camera>();
        }

        void Start()
        {
        }

        // Use this for initialization
        private void Update()
        {
            //float smooth = 1.0f - Mathf.Pow(0.5f, Time.deltaTime * speed);
            if(Vector3.Distance(m_Camera.transform.position, m_FinalPosition) > 0.1f)
                m_Camera.transform.position = Vector3.Lerp(m_Camera.transform.position, m_FinalPosition, Time.deltaTime * m_MoveSpeed);
            if (m_Camera.fieldOfView > m_FinalFOV)
                m_Camera.fieldOfView = Mathf.Lerp(m_Camera.fieldOfView, 0, Time.deltaTime * m_FovSpeed);
            //Debug.Log("fov = " + m_Camera.fieldOfView);

            if (m_Camera.fieldOfView <= m_FinalFOV && Vector3.Distance(m_Camera.transform.position, m_FinalPosition) <= 0.1f)
            {
                Debug.Log("Complete"+ m_Camera.transform.position);

                transform.gameObject.SetActive(false);
                m_MainCamera.SetActive(true);

                m_MainCamera.transform.GetComponent<Camera>().fieldOfView = m_Camera.fieldOfView;
                m_MainCamera.transform.position = m_Camera.transform.position;
            }
        }
    }
}
