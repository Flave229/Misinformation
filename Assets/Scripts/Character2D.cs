using Assets.Scripts.AI;
using Assets.Scripts.AI.Movement_AI;
using Assets.Scripts.AI.TaskData;
using Assets.Scripts.AI.Tasks;
using Assets.Scripts.EventSystem;
using UnityEngine;
using Event = Assets.Scripts.EventSystem.Event;

namespace Assets.Scripts
{
    public class Character2D : MonoBehaviour, IEventListener
    {
        [SerializeField]
        private InputManager _inputManager;

        public AIStack Tasks;
        public Animator Animator;
        public IMovementAI MovementAi;
        public Room CurrentRoom;
        public JobType JobType;
        public float WalkSpeed = 0.1f;
        public bool FacingRight = true;
        public bool Pause;

        public void Awake()
        {
            Animator = GetComponent<Animator>();
            _inputManager = InputManager.Instance();
            Tasks = new AIStack();
        }

        public void Start()
        {
            if (JobType == JobType.TECHNICIAN)
                	MovementAi = new PlayerMovementAI(this, new AStarPathfinding());
            else
            {
                MovementAi = new NPCMovementAI(this, new AStarPathfinding());
                Tasks.AddToStack(new IdleTask(new IdleData
                {
                    General = this.gameObject
                }));
            }

            SubscribeToEvents();
        }

        private void OnDestroy()
        {
            if (MovementAi != null)
                MovementAi.ClearPath();
            if (Tasks != null)
                Tasks.Destroy();
        }

        private void FixedUpdate()
        {
            if (Timer.Instance().Paused || Pause)
                return;

            Tasks.Update();
            
            Move();
        }

        private void Move()
        {
            //Do the actual movement.
            //Check we are facing the correct direction.
//            if (JobType == JobType.TECHNICIAN && _inputManager.m_DirectionHorizontal != 0.0f)
//            {
//                if (_inputManager.m_DirectionHorizontal < 0.0f)
//                    FaceRight();
//                else if (_inputManager.m_DirectionHorizontal > 0.0f)
//                    FaceLeft();
//
//                Animator.SetBool("IDLE", false);
//                MovementAi.ClearPath();
//                transform.position = new Vector3(transform.position.x + (WalkSpeed * _inputManager.m_DirectionHorizontal), transform.position.y, transform.position.z);
//            }

            var movementPath = MovementAi.GetCurrentPath();

            if (movementPath.Count <= 0)
            {
                Animator.SetBool("IDLE", true);
                return;
            }

            Vector2 targetPosition = movementPath[movementPath.Count - 1].Position;
            float distance = targetPosition.x - transform.position.x;
            if (distance < 0.0f)
            {
                FaceRight();
                distance = -1.0f;
                Animator.SetBool("IDLE", false);
            }
            else if (distance > 0.0f)
            {
                FaceLeft();
                distance = 1.0f;
                Animator.SetBool("IDLE", false);
            }
            else
                Animator.SetBool("IDLE", true);

            transform.position = new Vector3(transform.position.x + (WalkSpeed * distance), transform.position.y, transform.position.z);
        }

        public void Flip()
        {
            // Switch the way the player is labelled as facing.
            FacingRight = !FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        private void FaceRight()
        {
            if (transform.localScale.x > 0.0f)
                Flip();
        }

        private void FaceLeft()
        {
            if (transform.localScale.x < 0.0f)
                Flip();
        }

        public void ConsumeEvent(Event subscribeEvent, object eventPacket)
        {
            switch (subscribeEvent)
            {
                case Event.PLACE_LISTENING_DEVICE:
                    Vector3 mousePlacement = (Vector3)eventPacket;
                    MovementAi.CreatePathTo(mousePlacement);
                    break;
			    case Event.LEFT_MOUSE_CLICK:
				    Vector2 mouseClickPosition = (Vector2)eventPacket;
				    if (this.gameObject.GetComponent<Technician> ().IsActive) 
					    MovementAi.CreatePathTo (mouseClickPosition);
                    break;
            }
        }

        public void SubscribeToEvents()
        {
            if (JobType == JobType.GENERAL)
                EventMessenger.Instance().SubscribeToEvent(this, Event.PLACE_LISTENING_DEVICE);
            else
                EventMessenger.Instance().SubscribeToEvent(this, Event.LEFT_MOUSE_CLICK);
        }
    }
}