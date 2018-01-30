using System.Collections.Generic;
using Assets.Scripts.AI.TaskData;
using Assets.Scripts.AI.Tasks;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class RadialMenu : MonoBehaviour
    {
        //Public
        public RadialButton buttonPrefab;
        public RadialButton selected;
        public Vector3 mouseLocation;
        //Private
        private InputManager inputManager;
        private GameObject technician;
        private Character2D character;
        private Text nameObj;
        private Text defaultName;
        //Add interactable objects here
        private GameObject[] chairObj;
        private GameObject[] plantObj;
        private GameObject[] bedObj;
        List<GameObject> InteractableObjs = new List<GameObject>();
        List<RadialButton> Buttons = new List<RadialButton>();
        private GameObject _Camera;

        private void Start()
        {
            inputManager = InputManager.Instance();
            character = technician.GetComponent<Character2D>();
            _Camera = GameObject.FindGameObjectWithTag("MainCamera");
            AddToObjList();
        }

        private void InitialiseInteractableList()
        {
            //ADD NEW TAGGED ITEMS HERE
            chairObj = GameObject.FindGameObjectsWithTag("Chair");
            plantObj = GameObject.FindGameObjectsWithTag("Plant");
            bedObj = GameObject.FindGameObjectsWithTag("Bed");//Change this to something else that doesnt use tags
            technician = GameObject.FindGameObjectWithTag("Player"); //Keep this here otherwise cant place stuffs
            defaultName = this.GetComponent<Text>();
        }

        private void AddToObjList()
        {
            int i = 0;
            for (i = 0; i < chairObj.Length; ++i)
            {
                InteractableObjs.Add(chairObj[i]);
            }
            for (i = 0; i < plantObj.Length; ++i)
            {
                InteractableObjs.Add(plantObj[i]);
            }
            for (i = 0; i < bedObj.Length; ++i)
            {
                InteractableObjs.Add(bedObj[i]);
            }
        }

        public void SpawnButtons(Interactable obj)
        {
            for (int i = 0; i < obj.options.Length; i++)
            {
                RadialButton newButton = Instantiate(buttonPrefab) as RadialButton;
                newButton.transform.SetParent(transform, false);
                float theta = (2 * Mathf.PI / obj.options.Length) * i;
                float xPos = Mathf.Sin(theta);
                float yPos = Mathf.Cos(theta);
                newButton.transform.localPosition = new Vector3(xPos, yPos, 0f) * 50f;
                newButton.circle.color = obj.options[i].color;
                newButton.icon.sprite = obj.options[i].sprite;
                newButton.title = obj.options[i].title;
                newButton.myMenu = this;
                newButton.AnimateIn();
                Buttons.Add(newButton);
            }
            RadialMenuText();
        }

        private void DoAIStuff(float num)
        {
            Stack<ITask> taskChain = new Stack<ITask>();

            if (num == 1.0f)
            {
                taskChain.Push(new PlaceListeningDeviceTask(new PathfindData
                {
                    GeneralMovementAI = GameManager.Instance().ActiveTech.GetComponent<Character2D>().MovementAi,
                    Location = mouseLocation
                }));

                taskChain.Push(new PathfindToLocationTask(new PathfindData
                {
                    GeneralMovementAI = GameManager.Instance().ActiveTech.GetComponent<Character2D>().MovementAi,
                    Location = mouseLocation
                }));
                character.Tasks.AddToStack(new AITaskChain(taskChain));
            }
            if (num == 0.0f)
            {
                taskChain.Push(new PathfindToLocationTask(new PathfindData
                {
                    GeneralMovementAI = GameManager.Instance().ActiveTech.GetComponent<Character2D>().MovementAi,
                    Location = mouseLocation
                }));
                character.Tasks.AddToStack(new AITaskChain(taskChain));
            }
        }

        private void PlayerFarFromMenu()
        {
            Vector3 cameraPosition = _Camera.transform.position;
            Camera camera = _Camera.transform.GetComponent<Camera>();
            Rect cameraRect = camera.pixelRect;
            float top = cameraRect.yMin;
            float bottom = cameraRect.yMax;
            float left = cameraRect.xMin;
            float right = cameraRect.xMax;

            Vector2 screenBounds = camera.WorldToScreenPoint(new Vector2(mouseLocation.x, mouseLocation.y));

            if (screenBounds.x < left)
                Destroy(gameObject);
            if (screenBounds.x > right)
                Destroy(gameObject);
            if (screenBounds.y < top)
                Destroy(gameObject);
            if (screenBounds.y > bottom)
                Destroy(gameObject);

        }

        private void ShowRadialMenu()
        {
            //this.GetComponent<Text>() = nameObj;
            mouseLocation.z = 0f;
            //RadialMenuText();

            if (selected)
            {
                if (selected.title == "PlaceObject")
                {
                    if (GameManager.Instance().FundingAmount > 0) //May need to change depending what direction listening devices taken.
                    {
                        for (int i = 0; i < InteractableObjs.Count; i++)
                        {
                            if (Vector2.Distance(mouseLocation, InteractableObjs[i].transform.position) < 2.0f)
                            {
                                DoAIStuff(1.0f);

                                Debug.Log(selected.title + " was selected");
                            }
                        }
                    }
                }
                if (selected.title == "RemoveObject")
                {
                    Debug.Log(selected.title + " was selected");
                }
                if (selected.title == "MoveObject")
                {
                    DoAIStuff(0.0f);
                    Debug.Log(selected.title + " was selected");
                }
            }
            Destroy(gameObject);
        }

        private void RadialMenuText()
        {
            InitialiseInteractableList();
            for (int i = 0; i < chairObj.Length; i++)
            {
                if (Vector2.Distance(mouseLocation, chairObj[i].transform.position) < 2.0f)
                {
                    defaultName.text = "Chair";
                }
            }
            for (int i = 0; i < plantObj.Length; i++)
            {
                if (Vector2.Distance(mouseLocation, plantObj[i].transform.position) < 2.0f)
                {
                    defaultName.text = "Plant";
                }
            }
            for (int i = 0; i < bedObj.Length; i++)
            {
                if (Vector2.Distance(mouseLocation, bedObj[i].transform.position) < 2.0f)
                {
                    defaultName.text = "Bed";
                }
            }
            //defaultName.text = "Help, I'm trapped in a menu";
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(1))
            {
                ShowRadialMenu();
                PlayerFarFromMenu();
            }
            PlayerFarFromMenu();  //THIS REMOVES RADIAL MENU WHEN OUT OF CAMERA SPACE
        }
    }
}

