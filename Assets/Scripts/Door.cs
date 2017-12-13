using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.AI.Movement_AI;
using UnityEngine;
using System.Linq;

namespace Assets.Scripts
{
    public enum DoorType
    {
        Door_Up,
        Door_Down,
        Door_Left,
        Door_Right
    };

    public class Door : MonoBehaviour, ICollisionEvent
    {
        Animator animator;

        private Dictionary<Character2D, float> _delay = new Dictionary<Character2D, float>();

        float _defaultCooldown  = 1.0f;


        private static Character2D m_Player;

        private static float m_Transparency = 0.35f;

        private IEnumerator coroutine;
        private Vector3 defaultSize;

        public DoorType m_DoorType;

        public Room m_ParentRoom;
        public Door m_ConnectingDoor;

        public bool m_OpenClose;

        public Node m_Node;

        // Use this for initialization
        void Awake()
        {
            m_ParentRoom = transform.parent.GetComponent<Room>();
            m_Node = new Node
            {
                Position = transform.position,
                Owner = this
            };

            if (m_DoorType == DoorType.Door_Down)
            {
                // Set this door type to be transparent.
                SpriteRenderer sr = transform.GetComponent<SpriteRenderer>();
                Color col = sr.color;
                col.a = m_Transparency;
                sr.color = col;
            }
        }

        void Start()
        {
            var playerGameObject = GameObject.FindGameObjectWithTag("Player");
            m_Player = playerGameObject.GetComponent<Character2D>();
            if (m_ConnectingDoor != null)
            {
                m_Node.ConnectingNodes = new List<Node>
                {
                    m_ConnectingDoor.m_Node
                };
            }

            // Get all doors in the room and make it a connecting node
            foreach (Transform childTransform in m_ParentRoom.transform)
            {
                var doorComponent = childTransform.GetComponent<Door>();
                if (doorComponent != null && childTransform != transform)
                    m_Node.ConnectingNodes.Add(doorComponent.m_Node);
            }
            animator = this.GetComponent<Animator>();
            defaultSize = transform.localScale;
        }

        // Update is called once per frame
        void Update()
        {
            m_OpenClose = animator.GetBool("OpenClose");

            //_delay = _delay.ToDictionary(element => element.Key, element => (element.Value - Time.deltaTime));

            //List<Character2D> charactersToRemove = new List<Character2D>();
            //foreach(var a in _delay)
            //{
            //    if(a.Value <= 0.0f)
            //    {
            //        a.Key.transform.position = m_ConnectingDoor.transform.position;
            //        a.Key.CurrentRoom = m_ConnectingDoor.m_ParentRoom;
            //        charactersToRemove.Add(a.Key);
            //    }
            //}
            //foreach(Character2D character in charactersToRemove)
            //{
            //    _delay.Remove(character);
            //}
        }

        public void ActivateEvent(Character2D character)
        {
            if (m_ConnectingDoor != null)
            {
                coroutine = OpenDoor();
                StartCoroutine(coroutine);

                //if (m_ConnectingDoor.m_DoorType == DoorType.Door_Down || m_ConnectingDoor.m_DoorType == DoorType.Door_Up)
                //{
                //    _delay.Add(character, _defaultCooldown);
                //}
                //else
                //{
                    character.transform.position = m_ConnectingDoor.transform.position;
                    character.CurrentRoom = m_ConnectingDoor.m_ParentRoom;
                //}
            }
        }

        public void ActivateEventS(Character2D c)
        {
            _delay.Add(c, _defaultCooldown);
        }

        void CloseDoor()
        {
            animator.SetBool("OpenClose", false);
            transform.localScale = defaultSize;
            gameObject.GetComponent<Renderer>().sortingOrder = 1;
            m_ConnectingDoor.animator.SetBool("OpenClose", false);
            m_ConnectingDoor.transform.localScale = defaultSize;
            m_ConnectingDoor.GetComponent<Renderer>().sortingOrder = 1;
        }

        IEnumerator OpenDoor()
        {
            animator.SetBool("OpenClose", true);
            this.transform.localScale = new Vector3(1, 1, 1);
            this.gameObject.GetComponent<Renderer>().sortingOrder = 2;
            m_ConnectingDoor.animator.SetBool("OpenClose", true);
            m_ConnectingDoor.transform.localScale = new Vector3(1, 1, 1);
            m_ConnectingDoor.gameObject.GetComponent<Renderer>().sortingOrder = 2;
            yield return new WaitForSeconds(1);
            CloseDoor();
        }
    }
}