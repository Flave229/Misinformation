using System.Collections.Generic;
using Assets.Scripts.AI;
using Assets.Scripts.General;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public List<GameObject> ListeningDevList = new List<GameObject>();
        public List<GameObject> GeneralList = new List<GameObject>();
		public List<GameObject> TechList = new List<GameObject> ();
        public List<Name> GeneralNameList = new List<Name>();
		public GameObject ActiveTech;
		public int ActiveTechNum;
        public int Days;
        public ConversationPanel ConversePanel;
        public Objective CurrentObjective;

        [SerializeField]
        private int _fundingAmount;
        private static GameManager _instance;
        private Text _fundingText;
        private Daily _dailyManager;

        private int CurrentDay = 1;
        private int CurrentMouth = 1;
        private int CurrentYear = 1940;
        private int CurrentRefNum = 10450;

        private DailyReport _dailyReport;
        public bool RecievingFunding;
        private bool _pause;
        private bool _pendingStart;
        private bool _gameover;
        private bool _usingDesk = false;

        public int FundingAmount
        {
            get { return _fundingAmount; }
            set { _fundingAmount = value; _fundingText.text = "£" + FundingAmount.ToString("0000"); }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(this.gameObject);
            }

            CurrentObjective = new Objective();
            CurrentObjective.pevent = Objective.Event;
            CurrentObjective.pplace = Objective.Place;
            CurrentObjective.ptime = Objective.Time;

            AwakeSingletonManagers();
            _dailyManager = UnityEngine.Object.FindObjectOfType<Daily>();
        }

        internal void Pause(bool pause)
        {
            _pause = pause;

            if (_pause == false)
                _pendingStart = true;

        }

        private void AwakeSingletonManagers()
        {
            //SoundManager.Instance();            
            InputManager.Instance();
            AITaskManager.Instance();
        }

        public void Start()
        {
            GeneralList.AddRange(GameObject.FindGameObjectsWithTag("General"));
            ListeningDevList.AddRange(GameObject.FindGameObjectsWithTag("ListeningDevice"));
			TechList.AddRange (GameObject.FindGameObjectsWithTag ("Player"));
			TechList [0].gameObject.GetComponent<Technician> ().IsActive = true;
			ActiveTech = TechList [0];
			ActiveTechNum = 0;

            _fundingText = GameObject.FindGameObjectsWithTag("FundingText")[0].GetComponent<Text>();
            _fundingText.text = "£" + FundingAmount.ToString("0000");
            _dailyReport = UnityEngine.Object.FindObjectOfType<DailyReport>();
        }

        public void Update()
        {
            if (_pause)
                return;

            if (_pendingStart)
            {
                TechList.Clear();
                GeneralList.Clear();
                ListeningDevList.Clear();
                TechList.AddRange(GameObject.FindGameObjectsWithTag("Player"));
                GeneralList.AddRange(GameObject.FindGameObjectsWithTag("General"));
                ListeningDevList.AddRange(GameObject.FindGameObjectsWithTag("ListeningDevice"));
                CurrentObjective = GameObject.FindObjectOfType<Objective>();
                ActiveTech = TechList[0];
                SoundManager.Instance().Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
                SoundManager.Instance().MusicSource = SoundManager.Instance().Camera.gameObject.AddComponent<AudioSource>();
                _fundingText = GameObject.FindGameObjectsWithTag("FundingText")[0].GetComponent<Text>();
                AITaskManager.GeneralsAwaitingConversation.Clear();
                _pendingStart = false;
            }

            if (ActiveTech == null)
                ActiveTech = TechList[0];

            InputManager.Instance().Update();
            AITaskManager.Instance().Update(GeneralList);
            CurrentObjective.pevent = Objective.Event;
            CurrentObjective.pplace = Objective.Place;
            CurrentObjective.ptime = Objective.Time;
            //Debug.Log("Event" + currentObjective.pevent + "Place" + currentObjective.pplace + "Time" + currentObjective.ptime);

            if (_dailyManager.TransitioningDay == false && Timer.Instance().GetRemainingTime() <= 0)
            {
                _dailyManager.EndDay();
            }
				
			if(Input.GetKeyUp(KeyCode.Tab) && _usingDesk == false)
			{
				CycleTech ();
			}

            if (FundingAmount < 0)
                FundingAmount = 0;
        }

        public static GameManager Instance()
        {
            return _instance ?? (_instance = new GameManager());
        }
        
		public void CycleTech()
		{
            TechList.Clear();
            TechList.AddRange(GameObject.FindGameObjectsWithTag("Player"));
            ActiveTech.gameObject.GetComponent<Technician> ().IsActive = false;
			if (ActiveTechNum == (TechList.Count - 1))
				ActiveTechNum = 0;
			else
				ActiveTechNum++;

			ActiveTech = TechList [ActiveTechNum];
			ActiveTech.gameObject.GetComponent<Technician>().IsActive = true;
			Camera.main.GetComponent<Camera2DFollow> ().target = ActiveTech.transform;
		}

		public void Salary()
		{
			foreach(GameObject t in TechList)
			{
				t.GetComponent<Technician> ().UpdateSalary ();
				FundingAmount = FundingAmount - t.GetComponent<Technician> ().Salary;
			}
		}
        
        public List<GameObject> GetListeningDevices()
        {
            return ListeningDevList;
        }

        public List<GameObject> GetGenList()
        {
            return GeneralList;
        }

		public List<GameObject> GetTechList()
		{
			return TechList;
		}

        public void UpdateCurrentDate()
        {
            CurrentDay++;
            if (CurrentDay > System.DateTime.DaysInMonth(CurrentYear, CurrentMouth))
            {
                CurrentDay = 1;
                CurrentMouth++;
                if (CurrentMouth > 12)
                {
                    CurrentMouth = 1;
                    CurrentYear++;
                }
            }
        }

        public string GetCurrentDate()
        {
            string date = CurrentDay + " ";
            if (CurrentMouth == 1) { date += "January "; } else
            if (CurrentMouth == 2) { date += "Feburary "; } else
            if (CurrentMouth == 3) { date += "March "; } else
            if (CurrentMouth == 4) { date += "April "; } else
            if (CurrentMouth == 5) { date += "May "; } else
            if (CurrentMouth == 6) { date += "June "; } else
            if (CurrentMouth == 7) { date += "July "; } else
            if (CurrentMouth == 8) { date += "August "; } else
            if (CurrentMouth == 9) { date += "September "; } else
            if (CurrentMouth == 10) { date += "October "; } else
            if (CurrentMouth == 11) { date += "November "; } else
            if (CurrentMouth == 12) { date += "December "; }
            date += CurrentYear + "";
            return date;
        }

        public string GetRefNum()
        {
            string REFNUM = "";
            CurrentRefNum += UnityEngine.Random.Range(1, 15);
            REFNUM = "" + CurrentRefNum;
            return REFNUM;
        }

        public DailyReport GetDailyReport()
        {
            return _dailyReport;
        }

        public bool GetUsingDesk()
        {
            return _usingDesk;
        }

        public void SetUsingDesk(bool v)
        {
            _usingDesk = v;
        }
    }
}