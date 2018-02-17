using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.AI.Movement_AI;
using UnityEngine;
using System.Linq;
using UnityEngine.Serialization;

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
        private Animator _animator;
        private Dictionary<Character2D, float> _delay = new Dictionary<Character2D, float>();
        private float _defaultCooldown  = 1.0f;
        private static float m_Transparency = 0.35f;
        private IEnumerator _coroutine;
        private Vector3 _defaultSize;

        public DoorType DoorType;
        public Room ParentRoom;
        public Door ConnectingDoor;
        public bool OpenClose;
        public Node Node;

        // Use this for initialization
        void Awake()
        {
            ParentRoom = transform.parent.GetComponent<Room>();
            Node = new Node
            {
                Position = transform.position,
                Owner = this
            };

            if (DoorType == DoorType.Door_Down)
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
            if (ConnectingDoor != null)
            {
                Node.ConnectingNodes = new List<Node>
                {
                    ConnectingDoor.Node
                };
            }

            // Get all doors in the room and make it a connecting node
            foreach (Transform childTransform in ParentRoom.transform)
            {
                var doorComponent = childTransform.GetComponent<Door>();
                if (doorComponent != null && childTransform != transform)
                    Node.ConnectingNodes.Add(doorComponent.Node);
            }
            _animator = this.GetComponent<Animator>();
            _defaultSize = transform.localScale;
        }
        
        void Update()
        {
            OpenClose = _animator.GetBool("OpenClose");

            _delay = _delay.ToDictionary(element => element.Key, element => (element.Value - Time.deltaTime));

            List<Character2D> charactersToRemove = new List<Character2D>();
            foreach(var a in _delay)
            {
                if(a.Value <= 0.0f)
                {
                    a.Key.transform.position = ConnectingDoor.transform.position;
                    a.Key.CurrentRoom = ConnectingDoor.ParentRoom;
                    a.Key.Pause = false;
                    a.Key.gameObject.SetActive(true);
                    charactersToRemove.Add(a.Key);
                }
            }
            foreach(Character2D character in charactersToRemove)
            {
                _delay.Remove(character);
            }
        }

        public void ActivateEvent(Character2D character)
        {
            if (ConnectingDoor != null)
            {
                _coroutine = OpenDoor();
                StartCoroutine(_coroutine);

                if (ConnectingDoor.DoorType == DoorType.Door_Down || ConnectingDoor.DoorType == DoorType.Door_Up)
                {
                    _delay.Add(character, _defaultCooldown);
                    character.Pause = true;
                    character.gameObject.SetActive(false);
                }
                else
                {
                    character.transform.position = ConnectingDoor.transform.position;
                    character.CurrentRoom = ConnectingDoor.ParentRoom;
                }
            }
        }

        public void ActivateEventS(Character2D c)
        {
            _delay.Add(c, _defaultCooldown);
        }

        void CloseDoor()
        {
            _animator.SetBool("OpenClose", false);
            transform.localScale = _defaultSize;
            gameObject.GetComponent<Renderer>().sortingOrder = 1;
            ConnectingDoor._animator.SetBool("OpenClose", false);
            ConnectingDoor.transform.localScale = _defaultSize;
            ConnectingDoor.GetComponent<Renderer>().sortingOrder = 1;
        }

        IEnumerator OpenDoor()
        {
            _animator.SetBool("OpenClose", true);
            this.transform.localScale = new Vector3(1, 1, 1);
            this.gameObject.GetComponent<Renderer>().sortingOrder = 2;
            ConnectingDoor._animator.SetBool("OpenClose", true);
            ConnectingDoor.transform.localScale = new Vector3(1, 1, 1);
            ConnectingDoor.gameObject.GetComponent<Renderer>().sortingOrder = 2;
            SoundManager.Instance().PlaySingle("InsideDoorOriginal");
            yield return new WaitForSeconds(1);
            CloseDoor();
        }
    }
}