using Assets.Scripts.AI.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using Assets.Scripts.HouseholdItems;
using System.Linq;
using Assets.Scripts.AI.TaskData;

namespace Assets.Scripts.AI
{
    public class AITaskManager
    {
        private static AITaskManager _instance;
        private int _startingTimeInSeconds;
        private static Random _randomGenerator;
        public static Dictionary<GameObject, NeedStatus> GeneralsAwaitingConversation;

        private AITaskManager()
        {
            _startingTimeInSeconds = Timer.Instance().GetStartingTimeInSeconds();
            _randomGenerator = new Random();
            GeneralsAwaitingConversation = new Dictionary<GameObject, NeedStatus>();
        }

        public static AITaskManager Instance()
        {
            if (_instance == null)
                _instance = new AITaskManager();

            return _instance;
        }

        public void Update(List<GameObject> generalList)
        {
            int generalCount = generalList.Count;
            
            ChanceToSearchForListeningDevices(generalList);
        }

        public static void GoToToilet(GameObject generalGameObject, NeedStatus bladderNeed)
        {
            Character2D character = generalGameObject.GetComponent<Character2D>();

            List<Toilet> toilets = new List<Toilet>(Object.FindObjectsOfType<Toilet>()).Where(x => x.Occupied == false).ToList();
            if (toilets.Count <= 0)
                return;
            Toilet chosenToilet = toilets[_randomGenerator.Next(0, toilets.Count - 1)];
            chosenToilet.Occupied = true;
            Vector2 toiletPosition = chosenToilet.transform.position;

            Stack<ITask> taskChain = new Stack<ITask>();
            taskChain.Push(new UseToiletTask(new ToiletData
            {
                General = character,
                Toilet = chosenToilet,
                BladderNeed = bladderNeed
            }));
            taskChain.Push(new PathfindToLocationTask(new PathfindData
            {
                Character = character,
                Location = toiletPosition
            }));

            character.Tasks.AddToStack(new AITaskChain(taskChain));
        }

        public static void GoToBed(GameObject generalGameObject, NeedStatus restNeed)
        {
            Character2D generalOne = generalGameObject.GetComponent<Character2D>();

            List<Bed> beds = new List<Bed>(Object.FindObjectsOfType<Bed>()).Where(x => x.Occupied == false).ToList();
            if (beds.Count <= 0)
                return;
            Bed chosenBed = beds[_randomGenerator.Next(0, beds.Count - 1)];
            chosenBed.Occupied = true;
            Vector2 bedPosition = chosenBed.transform.position;

            Stack<ITask> taskChain = new Stack<ITask>();
            taskChain.Push(new SleepTask(new SleepData
            {
                General = generalOne,
                Bed = chosenBed,
                RestNeed = restNeed
            }));
            taskChain.Push(new PathfindToLocationTask(new PathfindData
            {
                Character = generalOne,
                Location = bedPosition
            }));

            generalOne.Tasks.AddToStack(new AITaskChain(taskChain));
        }

        public static void SitDown(GameObject generalGameObject, NeedStatus restNeed)
        {
            Character2D character = generalGameObject.GetComponent<Character2D>();

            List<Chair> chairs = new List<Chair>(GameObject.FindObjectsOfType<Chair>()).Where(x => x.Occupied == false).ToList();
            if (chairs.Count <= 0)
                return;
            Chair chosenChair = chairs[_randomGenerator.Next(0, chairs.Count - 1)];
            chosenChair.Occupied = true;
            Vector2 chairPosition = chosenChair.transform.position;

            Stack<ITask> taskChain = new Stack<ITask>();
            taskChain.Push(new SitTask(new SitData
            {
                General = character,
                Chair = chosenChair,
                RestNeed = restNeed
            }));
            taskChain.Push(new PathfindToLocationTask(new PathfindData
            {
                Character = character,
                Location = chairPosition
            }));

            character.Tasks.AddToStack(new AITaskChain(taskChain));
        }

        public static void LookAtArt(GameObject generalGameObject, NeedStatus entertainmentNeed)
        {
            GameObject[] interestingObjects = GameObject.FindGameObjectsWithTag("Art");
            if (interestingObjects.Length <= 0)
                return;
            GameObject chosenObject = interestingObjects[_randomGenerator.Next(0, interestingObjects.Length - 1)];
            Vector2 objectPosition = chosenObject.transform.position;

            Character2D character = generalGameObject.GetComponent<Character2D>();
            Stack<ITask> taskChain = new Stack<ITask>();
            taskChain.Push(new LookAtArtTask(entertainmentNeed));
            taskChain.Push(new PathfindToLocationTask(new PathfindData
            {
                Character = character,
                Location = objectPosition
            }));
            character.Tasks.AddToStack(new AITaskChain(taskChain));
        }

        private void ChanceToSearchForListeningDevices(List<GameObject> generalList)
        {
            if (generalList.Count == 0)
                return; 


            List<GameObject> untrustingGenerals = generalList.Where(x => x.GetComponent<General.General>().GetTrust() < 3).ToList();
            int untrustingGeneralCount = untrustingGenerals.Count;

            float taskChance = (float)(untrustingGeneralCount * 15) / (_startingTimeInSeconds) * Time.deltaTime;
            Random randomGenerator = new Random();
            if (randomGenerator.NextDouble() <= taskChance)
            {
                int generalIndex = randomGenerator.Next(0, untrustingGeneralCount);
                Character2D generalOne = generalList[generalIndex].GetComponent<Character2D>();

                GameObject targetedFurniture = FindPotentialListeningDeviceObject();

                if (targetedFurniture == null)
                    return;

                generalOne.Tasks.AddToStack(new FindListeningDeviceTask(new FindListeningDeviceData
                {
                    General = generalList[generalIndex].GetComponent<General.General>(),
                    Furniture = targetedFurniture.GetComponent<BuggableFurniture>()
                }));
                generalOne.Tasks.AddToStack(new PathfindToLocationTask(new PathfindData
                {
                    Character = generalOne,
                    Location = targetedFurniture.transform.position
                }));

            }
        }

        private GameObject FindPotentialListeningDeviceObject()
        {
            List<GameObject> potentialListeningDevices = GameObject.FindObjectsOfType<BuggableFurniture>().Select(x => x.gameObject).OfType<GameObject>().ToList();

            if (potentialListeningDevices.Count == 0)
                return null;

            System.Random random = new System.Random();
            int randomIndex = random.Next(0, potentialListeningDevices.Count);
            return potentialListeningDevices.ElementAt(randomIndex);
        }

        public static void LookForConversation(GameObject gameObject, NeedStatus socialNeed)
        {
            General.General generalComponent = gameObject.GetComponent<General.General>();
            NeedStatus generalOneSocialNeed = generalComponent.GetNeed(NeedType.SOCIAL);
            
            var general1ConverseData = new ConverseData
            {
                ReadyToTalk = false,
                General = generalComponent,
                SocialNeed = generalOneSocialNeed
            };

            gameObject.GetComponent<Character2D>().Tasks.AddToStack(new ConverseTask(general1ConverseData));
        }
    }
}