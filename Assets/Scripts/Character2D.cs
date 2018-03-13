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
        //public MovementAI MovementAi;
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
            if (JobType == JobType.GENERAL)
            {
                Tasks.AddToStack(new IdleTask(new IdleData
                {
                    General = this.gameObject
                }));
            }

            SubscribeToEvents();
        }

        private void OnDestroy()
        {
            if (Tasks != null)
                Tasks.Destroy();
        }

        private void FixedUpdate()
        {
            if (Timer.Instance().Paused || Pause)
                return;

            Tasks.Update();            
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

        public void FaceRight()
        {
            if (transform.localScale.x > 0.0f)
                Flip();
        }

        public void FaceLeft()
        {
            if (transform.localScale.x < 0.0f)
                Flip();
        }

        public void ConsumeEvent(Event subscribeEvent, object eventPacket)
        {
            switch (subscribeEvent)
            {
                case Event.PLACE_LISTENING_DEVICE:
                    //Vector3 mousePlacement = (Vector3)eventPacket;
                    //MovementAi.CreatePathTo(mousePlacement);
                    //break;
			    case Event.LEFT_MOUSE_CLICK:
				    Vector2 mouseClickPosition = (Vector2)eventPacket;
                    if (this.gameObject.GetComponent<Technician>().IsActive == false)
                        break;

                    Tasks.AddToStack(new PathfindToLocationTask(new PathfindData
                    {
                        Character = this,
                        Location = mouseClickPosition
                    }));
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

        public void ClearTasks()
        {
            if (Tasks != null)
                Tasks.Destroy();
        }
    }
}