using System.Collections.Generic;
using Assets.Scripts.AI.TaskData;
using Assets.Scripts.AI.Tasks;
using Assets.Scripts.HouseholdItems;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Assets.Scripts
{
    public class RadialMenu : MonoBehaviour
    {
        //Public
        public RadialButton buttonPrefab;
        public RadialButton selected;
        public Vector3 mouseLocation;
        public GameObject CanvasHireFire;
        //Private
        private LineRenderer lineRenderer;
        private InputManager inputManager;
        private GameObject technician;
        private Character2D character;
        private Text nameObj;
        private Text defaultName;
        //Add interactable objects here
        List<RadialButton> Buttons = new List<RadialButton>();
        List<GameObject> Buggable;
        private GameObject _Camera;
        public bool DrawingLine;

        private void Start()
        {
            inputManager = InputManager.Instance();
            character = technician.GetComponent<Character2D>();
            _Camera = GameObject.FindGameObjectWithTag("MainCamera");
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            /* line is purple, i don't know why, i think it is lacking a materia?
            lineRenderer.startColor = Color.blue;
            lineRenderer.endColor = Color.green;
            */
            lineRenderer.widthMultiplier = 0.05f;
            lineRenderer.positionCount = 2;
            CanvasHireFire = RadialMenuSpawner.ins.CanvasHireFire;
        }

        private void InitialiseInteractableList()
        {
            Buggable = UnityEngine.GameObject.FindObjectsOfType<BuggableFurniture>().Select(x => x.gameObject).ToList();

            technician = GameObject.FindGameObjectWithTag("Player"); //Keep this here otherwise cant place stuffs
            defaultName = this.GetComponent<Text>();
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

        private void MenuRelease()
        {
            mouseLocation.z = 0f;

            if (selected)
            {
                if (selected.title == "PlaceObject")
                {
                    if (GameManager.Instance().FundingAmount > 0) //May need to change depending what direction listening devices taken.
                    {
                        for (int i = 0; i < Buggable.Count; i++)
                        {
                            if (Vector2.Distance(mouseLocation, Buggable[i].transform.position) < 2.0f)
                            {
                                PlaceListeningDevice();
                                i += Buggable.Count;
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
                    MoveToLocation();
                }
                if (selected.title == "Board")
                {
                    MoveToLocation();
                    CanvasHireFire.SetActive(true); //TODO: Add function to to pause game when menu is open. Add close button to canvas to make canvas inactive.
                }
            }

            Destroy(gameObject);
        }

        private void PlaceListeningDevice()
        {
            Stack<ITask> taskChain = new Stack<ITask>();
            taskChain.Push(new PlaceListeningDeviceTask(new PlaceListeningDeviceData
            {
                PlacedBy = GameManager.Instance().ActiveTech.GetComponent<Technician>(),
                Location = mouseLocation
            }));

            taskChain.Push(new PathfindToLocationTask(new PathfindData
            {
                MovementAi = GameManager.Instance().ActiveTech.GetComponent<Character2D>().MovementAi,
                Location = mouseLocation
            }));

            character.Tasks.AddToStack(new AITaskChain(taskChain));
        }

        private void MoveToLocation()
        {
            character.Tasks.AddToStack(new PathfindToLocationTask(new PathfindData
            {
                MovementAi = GameManager.Instance().ActiveTech.GetComponent<Character2D>().MovementAi,
                Location = mouseLocation
            }));
        }

        private void RadialMenuText()
        {
            InitialiseInteractableList();
            for (int i = 0; i < Buggable.Count; i++)
            {
                if (Vector2.Distance(mouseLocation, Buggable[i].transform.position) < 2.0f)
                {
                    defaultName.text = "buggable";
                }
            }
            //defaultName.text = "Help, I'm trapped in a menu";
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(1))
            {
                MenuRelease();
                DrawingLine = false;
                PlayerFarFromMenu();
            }
            else
            {
                drawline();
                PlayerFarFromMenu();  //THIS REMOVES RADIAL MENU WHEN OUT OF CAMERA SPACE
            }
        }

        private void drawline()
        {
            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            if (DrawingLine)
            {
                Vector3 temp;
                temp = mouseLocation;
                temp.z = -0.2f;
                lineRenderer.SetPosition(0, temp);

                temp = Camera.main.ScreenToWorldPoint(transform.position);
                //temp.y += 0.2f;
                temp.z = -0.2f;
                lineRenderer.SetPosition(1, temp);

            }
        }
    }

}

