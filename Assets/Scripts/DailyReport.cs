using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class DailyReport : MonoBehaviour
    {
        private List<GameObject> DailyReportComponents = new List<GameObject>();
        private bool reportScreenVisible = false;
        [SerializeField]
        private List<string> correctResponses = new List<string>();
        [SerializeField]
        private List<Dropdown> dropdownsToCheck = new List<Dropdown>();
        public bool QueueHide;
        public GameObject player;
        public GameObject Typewriter;

        private void Start()
        {
            //Get all child elements
            foreach (RectTransform rectTransform in GetComponentInChildren<RectTransform>())
            {
                rectTransform.gameObject.SetActive(false);
                DailyReportComponents.Add(rectTransform.gameObject);
            }
        }

        private void Update()
        {
            float distance = Vector3.Distance(player.transform.position, Typewriter.transform.position);
            if (distance < 1)
            {
                if (Input.GetKeyDown(KeyCode.L) && Timer.Instance().GetRemainingTime() > 0)
                {
                    if (reportScreenVisible)
                    {
                        Hide();
                    }
                    else
                    {
                        SoundManager.Instance().PlaySingle("Typewriter And Bell-tamskp");
                        Show();
                    }
                }
            }
        }

        public void Show()
        {
            reportScreenVisible = true;
            Time.timeScale = 0;
            Timer.Instance().Pause();
            foreach (GameObject component in DailyReportComponents)
                component.SetActive(true);
        }

        public void Hide()
        {
            reportScreenVisible = false;
            Time.timeScale = 1;
            Timer.Instance().Play();
            reportScreenVisible = false;
            foreach (GameObject component in DailyReportComponents)
                component.SetActive(false);
            SoundManager.Instance().PlaySingle("Page_Turn-Mark DiAngelo");
        }

        public bool CheckValueParity()
        {
            //int index = Objective.Time.IndexOf();
            
            //Dropdown tempDropdownCheck;

            //    //run through all strings and check them by the combo boxes and store correct responses in list -R.Walters
            foreach (Dropdown dropdown in dropdownsToCheck)
            {
                if (dropdown.options[dropdown.value].text == Objective.Event)
                {
                    correctResponses.Add(dropdown.options[dropdown.value].text);
                }
                else if (dropdown.options[dropdown.value].text == Objective.Place)
                {
                    correctResponses.Add(dropdown.options[dropdown.value].text);
                }
                else if (dropdown.options[dropdown.value].text == Objective.Hour)
                {
                    correctResponses.Add(dropdown.options[dropdown.value].text);
                }
                else if (dropdown.options[dropdown.value].text == Objective.Day.ToString())
                {
                    correctResponses.Add(dropdown.options[dropdown.value].text);
                }
                else if (dropdown.options[dropdown.value].text == Objective.Month.ToUpper())
                {
                    correctResponses.Add(dropdown.options[dropdown.value].text);
                }
            }

            if(correctResponses.Count == 5)
            {
                GameManager.Instance().FundingAmount += 2000;
                GameManager.Instance().RecievingFunding = true;
                return true;
            }
            //dropdownsToCheck.AddRange(GameObject.FindObjectsOfType(Dropdown));
            else
                GameManager.Instance().RecievingFunding = false;
            //split the answer for ptime into parts - the day, the hour and the months and year - R.Walters
            return false;
        }
    }
}