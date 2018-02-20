﻿using Assets.Scripts.AI.Tasks;
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
        public static List<GameObject> GeneralsAwaitingConversation;

        private AITaskManager()
        {
            _startingTimeInSeconds = Timer.Instance().GetStartingTimeInSeconds();
            _randomGenerator = new Random();
            GeneralsAwaitingConversation = new List<GameObject>();
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

            PairConversations();
            ChanceToSearchForListeningDevices(generalList);
        }

        private void PairConversations()
        {
            if (GeneralsAwaitingConversation.Count < 2)
                return;

            GameObject generalOne = GeneralsAwaitingConversation[0];
            GameObject generalTwo = GeneralsAwaitingConversation[1];
            GeneralsAwaitingConversation.RemoveRange(0, 2);

            int padding = 1;
            int directionModifier = UnityEngine.Random.Range(0, 1);
            if (directionModifier == 1)
                padding = padding * -1;

            List<Room> rooms = Object.FindObjectsOfType(typeof(Room)).OfType<Room>().ToList();
            rooms = rooms.Where(room => room.m_Accessible).ToList();

            int randomIndex = _randomGenerator.Next(0, rooms.Count);
            Vector3 roomPosition = rooms[randomIndex].transform.position;
            Vector3 colliderOffset = rooms[randomIndex].GetComponent<BoxCollider2D>().offset;
            Vector3 targetLocation = new Vector3(roomPosition.x + colliderOffset.x, roomPosition.y + colliderOffset.y, 0);

            var general1ConverseData = new ConverseData
            {
                ReadyToTalk = false,
                General = generalOne.GetComponent<General.General>()
            };
            var general2ConverseData = new ConverseData
            {
                ReadyToTalk = false,
                General = generalTwo.GetComponent<General.General>()
            };
            general1ConverseData.ConversationPartnerTaskData = general2ConverseData;
            general2ConverseData.ConversationPartnerTaskData = general1ConverseData;

            Stack<ITask> general1TaskChain = new Stack<ITask>();
            Stack<ITask> general2TaskChain = new Stack<ITask>();
            general1TaskChain.Push(new ConverseTask(general1ConverseData));
            general2TaskChain.Push(new ConverseTask(general2ConverseData));

            general1TaskChain.Push(new PathfindToLocationTask(new PathfindData
            {
                Location = targetLocation,
                MovementAi = generalOne.GetComponent<Character2D>().MovementAi
            }));
            general2TaskChain.Push(new PathfindToLocationTask(new PathfindData
            {
                Location = new Vector3(targetLocation.x + padding, targetLocation.y),
                MovementAi = generalTwo.GetComponent<Character2D>().MovementAi
            }));

            AITaskChain general1TaskChainTask = new AITaskChain(general1TaskChain);
            general1TaskChainTask.SetCeilingLock(true);
            AITaskChain general2TaskChainTask = new AITaskChain(general2TaskChain);
            general2TaskChainTask.SetCeilingLock(true);
            generalOne.GetComponent<Character2D>().Tasks.AddToStack(general1TaskChainTask);
            generalTwo.GetComponent<Character2D>().Tasks.AddToStack(general2TaskChainTask);
        }

        public static void GoToToilet(GameObject generalGameObject)
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
                Toilet = chosenToilet
            }));
            taskChain.Push(new PathfindToLocationTask(new PathfindData
            {
                MovementAi = character.MovementAi,
                Location = toiletPosition
            }));

            character.Tasks.AddToStack(new AITaskChain(taskChain));
        }

        public static void GoToBed(GameObject generalGameObject)
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
                Bed = chosenBed
            }));
            taskChain.Push(new PathfindToLocationTask(new PathfindData
            {
                MovementAi = generalOne.MovementAi,
                Location = bedPosition
            }));

            generalOne.Tasks.AddToStack(new AITaskChain(taskChain));
        }

        public static void SitDown(GameObject generalGameObject)
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
                Chair = chosenChair
            }));
            taskChain.Push(new PathfindToLocationTask(new PathfindData
            {
                MovementAi = character.MovementAi,
                Location = chairPosition
            }));

            character.Tasks.AddToStack(new AITaskChain(taskChain));
        }

        public static void LookAtArt(GameObject generalGameObject)
        {
            GameObject[] interestingObjects = GameObject.FindGameObjectsWithTag("Art");
            if (interestingObjects.Length <= 0)
                return;
            GameObject chosenObject = interestingObjects[_randomGenerator.Next(0, interestingObjects.Length - 1)];
            Vector2 objectPosition = chosenObject.transform.position;

            Character2D character = generalGameObject.GetComponent<Character2D>();
            Stack<ITask> taskChain = new Stack<ITask>();
            taskChain.Push(new LookAtArtTask());
            taskChain.Push(new PathfindToLocationTask(new PathfindData
            {
                MovementAi = character.MovementAi,
                Location = objectPosition
            }));
            character.Tasks.AddToStack(new AITaskChain(taskChain));
        }

        private void ChanceToSearchForListeningDevices(List<GameObject> generalList)
        {
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
                    MovementAi = generalOne.MovementAi,
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

        public static void AwaitConversation(GameObject gameObject)
        {
            GeneralsAwaitingConversation.Add(gameObject);
        }
    }
}