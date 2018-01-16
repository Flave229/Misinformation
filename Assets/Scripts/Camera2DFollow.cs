using UnityEngine;

namespace Assets.Scripts
{
    public class Camera2DFollow : MonoBehaviour
    {
        private bool m_Active = true;

        public Transform target;
		public Camera camera;
		public bool useKeyboardInput = true;
		public bool useMouseInput = true;
		public bool allowScreenEdgeMovement = true;
        public float damping = 1;
        public float lookAheadFactor = 3;
        public float lookAheadReturnSpeed = 0.5f;
        public float lookAheadMoveThreshold = 0.1f;
		public float maximumZoom = 1.0f;
		public float minimumZoom = 20.0f;
		public float lookDamper = 5f;
		public int screenEdgeSize = 10;
		public float screenEdgeSpeed = 1.0f;
		public bool smoothing = true;
		public float smoothingFactor = 0.1f;
		public float panSpeed = 1.0f;

        private float m_OffsetZ;
        private Vector3 m_LastTargetPosition;
        private Vector3 m_CurrentVelocity;
        private Vector3 m_LookAheadPos;
		private Vector3 lastMousePos;
		private Vector3 lastPanSpeed = Vector3.zero;

        // Use this for initialization
        private void Start()
        {
            m_LastTargetPosition = target.position;
            m_OffsetZ            = (transform.position - target.position).z;
            transform.parent     = null;
			lastMousePos = Vector3.zero;
        }


        // Update is called once per frame
        private void Update()
        {
			target = GameManager.Instance ().ActiveTech.transform;
			if (Input.GetKeyDown(KeyCode.O))
			{
				m_Active = !m_Active;
			}
            if (m_Active)
            {
                // only update lookahead pos if accelerating or changed direction
                float xMoveDelta = (target.position - m_LastTargetPosition).x;

                bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

                if (updateLookAheadTarget)
                {
                    m_LookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
                }
                else
                {
                    m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
                }

                Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward * m_OffsetZ;
                Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

                transform.position = newPos;

                m_LastTargetPosition = target.position;
            }
			else
			{
				MouseLook ();
			}
			Zoom ();

			lastMousePos = Input.mousePosition;

        }

        public void SetActive(bool yesNo)
        {
            m_Active = yesNo;
        }

		private void Zoom()
		{
			var newSize = camera.orthographicSize - Input.GetAxis("Mouse ScrollWheel");
			newSize = Mathf.Clamp(newSize, maximumZoom, minimumZoom);
			camera.orthographicSize = newSize;
		}

		private void MouseLook() {
			Vector3 moveVector = new Vector3(0, 0, 0);
			if (Input.GetMouseButton (2) && useMouseInput == true) {
				Vector3 deltaMousePos = (Input.mousePosition - lastMousePos);
				moveVector += new Vector3 (deltaMousePos.x, deltaMousePos.y, 0) * lookDamper;
			} 
			if (allowScreenEdgeMovement == true) {
				if (Input.mousePosition.x < screenEdgeSize) {
					moveVector.x -= screenEdgeSpeed;
				} else if (Input.mousePosition.x > Screen.width - screenEdgeSize) {
					moveVector.x += screenEdgeSpeed;
				}
				if (Input.mousePosition.y < screenEdgeSize) {
					moveVector.y -= screenEdgeSpeed;
				} else if (Input.mousePosition.y > Screen.height - screenEdgeSize) {
					moveVector.y += screenEdgeSpeed;
				}
			} 
			if (useKeyboardInput == true) {
				if (Input.GetKey(KeyCode.A))
				{
					moveVector.x -= 1;
				}
				if (Input.GetKey(KeyCode.S))
				{
					moveVector.y -= 1;
				}
				if (Input.GetKey(KeyCode.D))
				{
					moveVector.x += 1;
				}
				if (Input.GetKey(KeyCode.W))
				{
					moveVector.y += 1;
				}
			}

			var effectivePanSpeed = moveVector;
			if (smoothing)
			{
				effectivePanSpeed = Vector3.Lerp(lastPanSpeed, moveVector, smoothingFactor);
				lastPanSpeed = effectivePanSpeed;
			}

			transform.position = transform.position + transform.TransformDirection(effectivePanSpeed) * panSpeed * Time.deltaTime;
		}

		private Vector2 getMouseMovement() {
			return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
		}
    }
}
