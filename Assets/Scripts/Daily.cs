using Assets.Scripts.FileIO;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.General;
using System.Linq;

namespace Assets.Scripts
{
    public class Daily : MonoBehaviour
    {
        private const int _maxGeneralsInHouse = 8;
        private const int _maxGeneralsPerDay = 3;

        public bool TransitioningDay = true;
        public GameObject GeneralGameObject;
        public Room SpawnRoom;


        public List<Name> leavingGenerals;
        public List<Name> arrivingGenerals;

        public List<GameObject> _technicans = new List<GameObject>();
        public int _prevTechs = 1;

        void Start()
        {
            leavingGenerals = new List<Name>();
            arrivingGenerals = new List<Name>();
        }

        void Update()
        {
        }

        public void StartDay()
        {
            SoundManager.Instance().PlayBGM();
            GameManager.Instance().Days++;
            GameManager.Instance().UpdateCurrentDate();
            Timer.Instance().ResetRemainingTime();
			GameManager.Instance ().Salary ();
            TransitioningDay = false;
            
            _technicans = GameManager.Instance().TechList;  // not adding to techlist on second hire
            for (int i = _prevTechs; i < _technicans.Count; ++i)//Will continue to make more everyday... need to fix...  --- This fixes it.
            {
                HireTechs test = Resources.FindObjectsOfTypeAll<HireTechs>().ToList().First().GetComponent<HireTechs>();
                Technician tech = test.SelectedTech;
                _technicans[i] = Resources.Load<GameObject>("Player");
                Vector3 placementPosition = new Vector3(0f - i, -12.24f, 0f);
                _technicans[i] = UnityEngine.Object.Instantiate(_technicans[i], placementPosition, Quaternion.identity);
                _technicans[i].AddComponent<Technician>().SetSkills(tech.GetTranslationSkill(), tech.GetEquipmentSkill(), tech.GetMotivationSkill());
            }
            _prevTechs = _technicans.Count; //Will continue to make more everyday... need to fix...  --- This fixes it.
            GameManager.Instance().ActiveTech = _technicans[0]; //Need to be able to delete techs as hiring new ones are almost complete.
        }
        
        public void EndDay()
        {
            GameManager.Instance().GetDailyReport().Show();
            TransitioningDay = true;

            var generalList = GameManager.Instance().GeneralList;
            foreach (GameObject gameObject in generalList)
            {
                var general = gameObject.GetComponent<General.General>();
                general.UpdateKnowledgeValue(-1);
                general.UpdateTrustValue(1);
                general.GetComponent<Character2D>().ClearTasks();
            }

            foreach (GameObject gameObject in GameManager.Instance().TechList)
            {
                var technician = gameObject.GetComponent<Technician>();
                if (technician.GetComponent<Character2D>() != null)
                    technician.GetComponent<Character2D>().ClearTasks();
            }
            foreach (GameObject gameObject in GameManager.Instance().FireTechList)
            {
                GameManager.Instance().ActiveTech = GameManager.Instance().TechList[0];
                Destroy(gameObject);
            }

        }

        public void GenerateGenerals()
        {
            float paddingSpawn = 1.06f;

            int generalsToAdd = 0;
            if (_maxGeneralsInHouse - GameManager.Instance().GeneralList.Count < _maxGeneralsPerDay)
            {
                int maxGeneralsForToday = _maxGeneralsInHouse - GameManager.Instance().GeneralList.Count;
                generalsToAdd = Random.Range(1, maxGeneralsForToday);
            }
            if (_maxGeneralsInHouse - GameManager.Instance().GeneralList.Count != 0)
            {
                generalsToAdd = Random.Range(1, _maxGeneralsPerDay);
            }

            arrivingGenerals.Clear();
            for (int i = 0; i < generalsToAdd; ++i)
            {
                Vector2 generalPos = new Vector2(Random.Range(-24.45f, -39.91f) * paddingSpawn, -9.5f);
                GameObject general = Instantiate(GeneralGameObject, generalPos, Quaternion.identity);
                var characterComponent = general.GetComponent<Character2D>();
                characterComponent.CurrentRoom = SpawnRoom;
                GameManager.Instance().GeneralList.Add(general);
                var generalComponent = general.GetComponent<General.General>();
                generalComponent.Start();
                arrivingGenerals.Add(generalComponent.Name);
            }
        }

        public void DestructGenerals()
        {
            System.Random random = new System.Random();
            int generalsToRemove = random.Next(0, 2);

            for (int i = 0; i < generalsToRemove; ++i)
            {
                var generalList = GameManager.Instance().GeneralList;
                int randomGeneralIndex = Random.Range(0, generalList.Count - 1);
                GameObject general = GameManager.Instance().GeneralList[randomGeneralIndex];
                var generalComponent = general.GetComponent<General.General>();
                generalComponent.Start();
                leavingGenerals.Clear();
                leavingGenerals.Add(generalComponent.Name);
                Destroy(GameManager.Instance().GeneralList[randomGeneralIndex]);
                GameManager.Instance().GeneralList.RemoveAt(randomGeneralIndex);
                GameManager.Instance().GeneralNameList.Add(GameManager.Instance().GeneralList[randomGeneralIndex].GetComponent<General.General>().Name);
                NameGenerator.RemoveNameFromPool(GameManager.Instance().GeneralList[randomGeneralIndex].GetComponent<General.General>().Name);
            }
        }
    }
}